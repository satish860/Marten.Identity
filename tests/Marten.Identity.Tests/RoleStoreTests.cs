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

        [Fact]
        public async Task Should_Get_Normalized_Role_Name()
        {
            IRoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(martenTestFixture.documentStore);
            var name = await roleStore.GetNormalizedRoleNameAsync(new IdentityRole {  Name = "Staff" }, CancellationToken.None);
            name.Should().Be("staff");
        }

        [Fact]
        public async Task Should_be_able_to_Id_for_role()
        {
            IRoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(martenTestFixture.documentStore);
            var id = Guid.NewGuid().ToString();
            var roleId = await roleStore.GetRoleIdAsync(new IdentityRole { Id=id,Name = "Staff" }, CancellationToken.None);
            roleId.Should().Be(id);
        }

        [Fact]
        public async Task Should_be_able_to_name_for_role()
        {
            IRoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(martenTestFixture.documentStore);
            var id = Guid.NewGuid().ToString();
            var name = await roleStore.GetRoleNameAsync(new IdentityRole { Id = id, Name = "Staff" }, CancellationToken.None);
            name.Should().Be("Staff");
        }

        [Fact]
        public async Task Should_be_able_to_update_the_Role_name()
        {
            IRoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(martenTestFixture.documentStore);
            var id = Guid.NewGuid().ToString();
            var identityResult = await roleStore.CreateAsync(new IdentityRole { Id = id, Name = "Manager sr1" }, CancellationToken.None);
            if (identityResult.Succeeded)
            {
                IdentityResult result = await roleStore.UpdateAsync(new IdentityRole { Id = id, Name = "Manager sr2" }, CancellationToken.None);
                result.Succeeded.Should().BeTrue();
            }
        }
    }
}
