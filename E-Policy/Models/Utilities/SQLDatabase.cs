using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace E_Policy.Models.Utilities
{
    public class SQLDatabase
    {
        SqlConnection sqlConnection;
        SqlTransaction sqlTransaction;
        SqlDataAdapter sqlDataAdapter;

        public SQLDatabase(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
        }

        public DataTable ExecuteQuery(string query, CommandType commandType, List<SqlParameter> sqlParameters = null)
        {
            DataTable dataTable = new DataTable("MyData");

            if (sqlParameters == null) sqlParameters = new List<SqlParameter>();

            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = query;
                sqlCommand.CommandType = commandType;
                sqlCommand.CommandTimeout = 900; //15 Minutes

                foreach (SqlParameter sqlParameter in sqlParameters)
                {
                    sqlCommand.Parameters.Add(sqlParameter);
                }

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                dataTable.TableName = "MyData";
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            return dataTable;
        }

        public void TransferToDataBase(DataTable dataTable, string tableName)
        {
            DataTable dtTemporary = new DataTable();

            try
            {
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();

                sqlDataAdapter = new SqlDataAdapter("SELECT TOP 0 * FROM " + tableName, sqlConnection);
                sqlDataAdapter.SelectCommand.Transaction = sqlTransaction;
                sqlDataAdapter.Fill(dtTemporary);

                SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                if (dataTable.GetChanges(DataRowState.Added) != null)
                {
                    sqlDataAdapter.InsertCommand = sqlCommandBuilder.GetInsertCommand();
                    sqlDataAdapter.Update(dataTable);
                }
                if (dataTable.GetChanges(DataRowState.Deleted) != null)
                {
                    sqlDataAdapter.DeleteCommand = sqlCommandBuilder.GetDeleteCommand();
                    sqlDataAdapter.Update(dataTable);
                }
                if (dataTable.GetChanges(DataRowState.Modified) != null)
                {
                    sqlDataAdapter.UpdateCommand = sqlCommandBuilder.GetUpdateCommand();
                    sqlDataAdapter.Update(dataTable);
                }

                sqlTransaction.Commit();
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();
                throw;
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }
        }
    }
}