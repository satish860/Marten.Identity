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
    public class UserPasswordStoreTests : DatabaseCollectionBase
    {
        private readonly MartenTestFixture martenTestFixture;
        private IUserPasswordStore<IdentityUser> userPasswordStore;

        public UserPasswordStoreTests(MartenTestFixture martenTestFixture)
        {
            this.martenTestFixture = martenTestFixture;
            this.userPasswordStore = new UserStore<IdentityUser, IdentityRole>(this.martenTestFixture.documentStore);
        }

        [Fact]
        public async Task Should_be_to_get_the_Password_Hash()
        {
            var hash = await userPasswordStore.GetPasswordHashAsync(new IdentityUser { PasswordHash = "SomeHash" }, CancellationToken.None);
            hash.Should().Be("SomeHash");
        }

        [Fact]
        public async Task Should_be_able_to_Set_the_Password_Hash()
        {
            var identity = new IdentityUser { PasswordHash = "SomeHash" };
            await userPasswordStore.SetPasswordHashAsync(identity, "SomeHash1", CancellationToken.None);
            identity.PasswordHash.Should().Be("SomeHash1");
        }

        [Fact]
        public async Task Should_be_able_to_Check_if_the_Password_Hash_Is_Set()
        {
            var identity = new IdentityUser { PasswordHash = "SomeHash" };
            var isPasswordAsync = await userPasswordStore.HasPasswordAsync(identity, CancellationToken.None);
            isPasswordAsync.Should().BeTrue();
        }
    }
}
