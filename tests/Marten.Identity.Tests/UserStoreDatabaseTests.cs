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
            var identityResult = await userStore.CreateAsync(new IdentityUser { UserName = "satish" }, CancellationToken.None); ;
            identityResult.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Should_be_Able_to_Update_user()
        {
            IUserStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(fixture.documentStore);
            var id = Guid.NewGuid().ToString();
            var creationResult= await userStore.CreateAsync(new IdentityUser { Id = id, UserName = "satish" }, CancellationToken.None);
            if (creationResult.Succeeded)
            {
                var identityResult = await userStore.UpdateAsync(new IdentityUser { Id = id, UserName = "satish1" }, CancellationToken.None);
                identityResult.Succeeded.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Should_be_able_to_delete_user()
        {
            IUserStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(fixture.documentStore);
            var id = Guid.NewGuid().ToString();
            var creationResult = await userStore.CreateAsync(new IdentityUser { Id = id, UserName = "satish" }, CancellationToken.None);
            if (creationResult.Succeeded)
            {
                var identityResult = await userStore.DeleteAsync(new IdentityUser { Id = id, UserName = "satish1" }, CancellationToken.None);
                identityResult.Succeeded.Should().BeTrue();
            }
        }


        [Fact]
        public async Task Should_be_able_to_Query_userbyUser()
        {
            IUserStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(fixture.documentStore);
            var id = Guid.NewGuid().ToString();
            var creationResult = await userStore.CreateAsync(new IdentityUser { Id = id, UserName = "satish3" }, CancellationToken.None);
            if (creationResult.Succeeded)
            {
                var result = await userStore.FindByNameAsync("satish3" , CancellationToken.None);
                result.Id.Should().Be(id);
            }
        }
    }
}
