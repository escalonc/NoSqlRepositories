using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace IntegrationTests.MongoTests
{
    public class DeleteDocument : DatabaseFixture
    {
        [Fact]
        public async Task Delete_Document_IsNotFound()
        {
            var document = new TestDocument {Name = $"Sample {Guid.NewGuid()}"};
            await Repository.Add(document);

            await Repository.Delete(document);

            var actualDocument = await Repository.Find(x => x.Name == document.Name);

            actualDocument.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteBatch_Document_IsNotFound()
        {
            var document = new TestDocument {Name = $"Sample {Guid.NewGuid()}"};
            await Repository.Add(document);

            await Repository.Delete(document);

            var actualDocument = await Repository.Find(x => x.Name == document.Name);

            actualDocument.Should().BeEmpty();
        }
    }
}