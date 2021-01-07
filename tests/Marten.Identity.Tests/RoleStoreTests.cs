using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Marten.Identity.Tests
{
    public class RoleStoreTests : DatabaseCollectionBase
    {
        private readonly MartenTestFixture martenTestFixture;

        public RoleStoreTests(MartenTestFixture martenTestFixture)
        {
            this.martenTestFixture = martenTestFixture;
        }

        [Fact]
        public async Task Should_be_able_to_Create_role_for_user()
        {
            IRoleStore<IdentityRole> userStore = new RoleStore<IdentityRole>(martenTestFixture.documentStore);
            var identityResult = await userStore.CreateAsync(new IdentityRole { Name = "Worker" }, CancellationToken.None); ;
            identityResult.Succeeded.Should().BeTrue();
        }
    }
}
