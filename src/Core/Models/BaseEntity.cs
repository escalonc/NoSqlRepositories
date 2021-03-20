using System;
using Core.Contracts;

namespace Core.Models
{
    public class BaseEntity : IAuditable
    {
        public Guid Id { get; set; }

        public bool Enabled { get; set; } = true;

        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}