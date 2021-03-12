using System;

namespace Core.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool Enabled { get; set; } = true;
    }
}