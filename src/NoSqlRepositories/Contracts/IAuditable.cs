using System;

namespace NoSqlRepositories.Contracts
{
    public interface IAuditable
    {
        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}