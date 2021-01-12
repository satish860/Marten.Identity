using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using System.Security.Claims;

namespace Marten.Identity.Tests
{
    public class UserClaimTests : DatabaseCollectionBase
    {
        private readonly MartenTestFixture martenTestFixture;
        private readonly IdentityUser identityUser;

        public UserClaimTests(MartenTestFixture martenTestFixture)
        {
            this.martenTestFixture = martenTestFixture;
            identityUser = new IdentityUser
            {
                Claims = new List<IdentityUserClaim> {
                    new IdentityUserClaim { ClaimType = "Name", ClaimValue = "Satish" },
                    new IdentityUserClaim { ClaimType = "email", ClaimValue = "satish860@gmail.com" },
                },
                Id = Guid.NewGuid().ToString(),
                UserName = "s1.t"

            };
        }

        private Task<IdentityResult> SetupUser(IdentityUser identityUser)
        {
            IUserStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(this.martenTestFixture.documentStore);
            return userStore.CreateAsync(identityUser, CancellationToken.None);
        }

        [Fact]
        public async Task Should_be_able_to_get_Claims_from_user()
        {
            IUserClaimStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(this.martenTestFixture.documentStore);
            var claims = await userStore.GetClaimsAsync(identityUser, CancellationToken.None);
            claims.Should().NotBeNull();
            claims.Should().Contain(p => p.Type == "Name");
        }

        [Fact]
        public async Task Should_be_to_Add_Claims_For_user()
        {
            IUserClaimStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(this.martenTestFixture.documentStore);
            await userStore.AddClaimsAsync(identityUser,
                new List<Claim> { new Claim("role", "somerole") },
                CancellationToken.None);
            var claims = identityUser.Claims;
            claims.Should().NotBeNull();
            claims.Should().Contain(p => p.ClaimType == "role");
        }

        [Fact]
        public async Task Should_be_able_to_replace_Claims()
        {
            IUserClaimStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(this.martenTestFixture.documentStore);
            await userStore.ReplaceClaimAsync(identityUser, new Claim("Name", "Satish"), new Claim("Name", "Satish1v"),
                CancellationToken.None);
            var claims = identityUser.Claims;
            claims.Should().NotBeNull();
            claims.Should().Contain(p => p.ClaimValue == "Satish1v");
        }

        [Fact]
        public async Task Should_be_able_to_remove_Claims()
        {
            IUserClaimStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(this.martenTestFixture.documentStore);
            await userStore.RemoveClaimsAsync(identityUser, new List<Claim> { new Claim("Name", "Satish") },
                CancellationToken.None);
            var claims = identityUser.Claims;
            claims.Should().NotContain(p => p.ClaimValue == "Satish");
        }

        [Fact]
        public async Task Should_be_able_to_Find_User_By_Claim()
        {
            var identityResult = await SetupUser(identityUser);
            if (identityResult.Succeeded)
            {
                IUserClaimStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(this.martenTestFixture.documentStore);
                var user = await userStore.GetUsersForClaimAsync(new Claim("Name", "Satish"), CancellationToken.None);
                user.Should().NotBeNull();
                user.Count.Should().Be(1);
            }
        }
    }
}
