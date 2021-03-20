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

            await Repository.Add(null);

            var actualDocument = Repository
                .Filter()
                .Where(d => d.Name == document.Name);

            actualDocument.Should().NotBeNull();
        }

        [Fact]
        public async Task AddBatch_Documents_AreFound()
        {
            var documents = Enumerable.Range(1, 10).Select(i => new TestDocument
            {
                Name = $"Sample {Guid.NewGuid().ToString()}"
            }).ToList();

            await Repository.AddBatch(documents);

            var actualDocument = Repository
                .Filter()
                .Where(d => documents.Select(e => e.Name).Contains(d.Name));

            actualDocument.Should().AllBeEquivalentTo(documents);
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