using FluentAssertions;
using Marten;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace Marten.Identity.Tests
{
    public class MartenConnectionTests :DatabaseCollectionBase
    {
        private readonly MartenTestFixture fixture;

        public MartenConnectionTests(MartenTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Should_be_able_to_Connect_to_Document_Store()
        {
            var store = DocumentStore.For("host=localhost;database=postgres;password=mysecretpassword;username=postgres");
            IDocumentSession session= store.OpenSession();
            session.Connection.FullState.Should().Be(ConnectionState.Open);
        }
    }
}
