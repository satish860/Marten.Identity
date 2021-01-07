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
            IRoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(martenTestFixture.documentStore);
            var identityResult = await roleStore.CreateAsync(new IdentityRole {Id=Guid.NewGuid().ToString() ,Name = "Worker" }, CancellationToken.None); ;
            identityResult.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Should_be_able_to_Delete_role_for_user()
        {
            IRoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(martenTestFixture.documentStore);
            var id = Guid.NewGuid().ToString();
            var identityResult = await roleStore.CreateAsync(new IdentityRole { Id=id,Name = "Manager" }, CancellationToken.None);
            if (identityResult.Succeeded)
            {
                var deleteIdentityResult = await roleStore.CreateAsync(new IdentityRole { Id = id, Name = "Manager" }, CancellationToken.None);
                deleteIdentityResult.Succeeded.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Should_be_able_find_role_by_id()
        {
            IRoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(martenTestFixture.documentStore);
            var id = Guid.NewGuid().ToString();
            var identityResult = await roleStore.CreateAsync(new IdentityRole { Id = id, Name = "Manager sr" }, CancellationToken.None);
            if (identityResult.Succeeded)
            {
                IdentityRole role = await roleStore.FindByIdAsync(id, CancellationToken.None);
                role.Name.Should().Be("Manager sr");
            }
        }

        [Fact]
        public async Task Should_be_able_Find_role_by_name()
        {
            IRoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(martenTestFixture.documentStore);
            var id = Guid.NewGuid().ToString();
            var identityResult = await roleStore.CreateAsync(new IdentityRole { Id = id, Name = "Manager sr1" }, CancellationToken.None);
            if (identityResult.Succeeded)
            {
                IdentityRole role = await roleStore.FindByNameAsync("Manager sr1", CancellationToken.None);
                role.Name.Should().Be("Manager sr1");
            }
        }
    }
}
