using System;
using System.Diagnostics;
using System.IO;

using NLog;

namespace E_Policy.Service
{
    public static class MessageLog
    {
        static Logger logger;
        static string methodNameCaller;

        static MessageLog()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public static void Info(string Message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                methodNameCaller = stackTrace.GetFrame(1).GetMethod().DeclaringType + " | " + stackTrace.GetFrame(1).GetMethod().Name;

                logger.Info(methodNameCaller + " | " + Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Debug(string Message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                methodNameCaller = stackTrace.GetFrame(1).GetMethod().DeclaringType + " | " + stackTrace.GetFrame(1).GetMethod().Name;

                logger.Debug(methodNameCaller + " | " + Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Warning(string Message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                methodNameCaller = stackTrace.GetFrame(1).GetMethod().DeclaringType + " | " + stackTrace.GetFrame(1).GetMethod().Name;

                logger.Warn(methodNameCaller + " | " + Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Error(string Message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                methodNameCaller = stackTrace.GetFrame(1).GetMethod().DeclaringType + " | " + stackTrace.GetFrame(1).GetMethod().Name;

                logger.Error(methodNameCaller + " | " + Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
