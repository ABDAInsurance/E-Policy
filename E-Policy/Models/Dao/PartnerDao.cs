using System;

namespace E_Policy.Models.Dao
{
    public class PartnerDao
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string TOC { get; set; }
        public string InsuredId { get; set; }
        public string SOBId { get; set; }
        public string PolicyType { get; set; }
     
        public PartnerDao()
        {
            Code = "Default";
            Name = "Default";
            TOC = string.Empty;
            InsuredId = string.Empty;
            SOBId = string.Empty;
            PolicyType = string.Empty;
        }
    }
}