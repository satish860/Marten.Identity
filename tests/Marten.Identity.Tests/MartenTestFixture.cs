using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Databases;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Marten.Identity.Tests
{
    public class MartenTestFixture : IDisposable,IAsyncLifetime
    {
        private readonly PostgreSqlTestcontainer _testContainer;
        public IDocumentStore documentStore { get; private set; } 

        public MartenTestFixture()
        {
            var testContainerBuilder = new TestcontainersBuilder<PostgreSqlTestcontainer>()
                 .WithCleanUp(true)
                 .WithDatabase(new PostgreSqlTestcontainerConfiguration
                 {
                     Database = "aspnetidentity",
                     Username = "aspnetidentity",
                     Password = "aspnetidentity"
                 })
                 .WithImage("clkao/postgres-plv8");

            _testContainer = testContainerBuilder.Build();

        }

       
        public void Dispose()
        {
            
        }

        public async Task InitializeAsync()
        {
            await _testContainer.StartAsync();

            var result = await _testContainer.ExecAsync(new[]
            {
                "/bin/sh", "-c",
                "psql -U aspnetidentity -c \"CREATE EXTENSION plv8; SELECT extversion FROM pg_extensions WHERE extname = 'plv8';\""
            });
            this.documentStore = DocumentStore.For(_testContainer.ConnectionString);
        }

        public async Task DisposeAsync()
        {
            await _testContainer.StopAsync();
        }
    }
}
