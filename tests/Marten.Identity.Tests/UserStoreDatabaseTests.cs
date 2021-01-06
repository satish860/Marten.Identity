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
    public class UserStoreDatabaseTests : DatabaseCollectionBase
    {
        private readonly MartenTestFixture fixture;

        public UserStoreDatabaseTests(MartenTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_be_able_to_Create_user()
        {
            IUserStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(fixture.documentStore);
            var identityResult = await userStore.CreateAsync(new IdentityUser {UserName = "satish" }, CancellationToken.None); ;
            identityResult.Succeeded.Should().BeTrue();
        }
    }
}
