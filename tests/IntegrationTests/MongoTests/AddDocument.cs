using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace IntegrationTests.MongoTests
{
    public class AddDocument : DatabaseFixture
    {
        [Fact]
        public async Task Add_Document_IsFound()
        {
            var document = new TestDocument
            {
                Name = $"Sample {Guid.NewGuid()}"
            };

            await Repository.Add(document);
            var actualDocument = await Repository.Find(d => d.Name == document.Name);

            actualDocument.Should().NotBeNull();
        }

        [Fact]
        public async Task AddBatch_Documents_AreFound()
        {
            var documents = Enumerable.Range(1, 10).Select(i => new TestDocument
            {
                Name = $"Sample {Guid.NewGuid()}"
            }).ToList();

            await Repository.AddBatch(documents);
            var ids = documents.Select(x => x.Id);

            var actualDocuments = await Repository.Find(x => ids.Contains(x.Id));

            actualDocuments.Should().NotBeNull();
        }
    }
}