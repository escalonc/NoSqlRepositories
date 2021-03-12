using Core.Contracts;

namespace Core.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string? ConnectionString { get; set; }
        
        public string? DatabaseName { get; set; }
    }
}