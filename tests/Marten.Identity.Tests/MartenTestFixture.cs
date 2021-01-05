using System;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace Marten.Identity.Tests
{
    public class MartenTestFixture : IDisposable
    {
        private IContainerService containerService;

        public MartenTestFixture()
        {
            this.StartDocker();

        }

        private void StartDocker()
        {
            //containerService = new Builder().UseContainer()
            //    .UseImage("postgres")
            //    .WithEnvironment("POSTGRES_PASSWORD=mysecretpassword")
            //    .ReuseIfExists()
            //    .WithLabel("MartenTestDB")
            //    .ExposePort(5432, 5432)
            //    .WaitForPort("5432/tcp", 10000 /*10s*/, "127.0.0.1")
            //    .Build();
            //containerService.RemoveOnDispose = true;
            //containerService.Start();
        }

        public void Dispose()
        {
           // containerService.Dispose();
        }
    }
}
