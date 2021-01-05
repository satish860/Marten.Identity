using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Marten.Identity.Tests
{
    [CollectionDefinition("Database Tests")]
    public class DatabaseCollection : ICollectionFixture<MartenTestFixture>
    {
    }
}
