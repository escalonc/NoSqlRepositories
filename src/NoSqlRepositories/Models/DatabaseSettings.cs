using NoSqlRepositories.Contracts;

namespace NoSqlRepositories.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string? ConnectionString { get; set; }

        public string? DatabaseName { get; set; }
    }
}