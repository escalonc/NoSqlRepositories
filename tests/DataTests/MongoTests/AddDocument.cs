using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using FluentAssertions;
using Xunit;

namespace DataTests.MongoTests
{
    public class AddDocument : DatabaseFixture
    {
        [Fact]
        public async Task Add_Document_IsFound()
        {
            var document = new TestDocument
            {
                Name = $"Sample {Guid.NewGuid().ToString()}"
            };

           
           
        }

        [Fact]
        public async Task AddBatch_Documents_AreFound()
        {
            var documents = Enumerable.Range(1, 10).Select(i => new TestDocument
            {
                Name = $"Sample {Guid.NewGuid().ToString()}"
            }).ToList();

            await Repository.AddBatch(documents);

            
        }

        [Fact]
        public async Task Add_Inserts_Entity()
        {
            var mongoContext = new TestContext(new DatabaseSettings
            {
                ConnectionString =
                    "mongodb+srv://escalonc:admin#123#@cluster0.ywtwn.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "tests"
            });

            var testRepository = new TestRepository(mongoContext);

            await testRepository.Add(new TestDocument {Name = $"Hello {Guid.NewGuid().ToString()}"});
        }
    }
}