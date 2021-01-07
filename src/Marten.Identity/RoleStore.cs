using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Marten.Identity
{
    public class RoleStore<TRole> : IRoleStore<TRole>
                where TRole:IdentityRole,new()
    {
        private readonly IDocumentStore documentStore;

        public RoleStore(IDocumentStore store)
        {
            this.documentStore = store;
        }

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            using (IDocumentSession session = this.documentStore.LightweightSession())
            {
                session.Store(role);
                await session.SaveChangesAsync();
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            using (IDocumentSession session = this.documentStore.LightweightSession())
            {
                session.Delete(role);
                await session.SaveChangesAsync();
            }
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            
        }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            using (IQuerySession session = this.documentStore.QuerySession())
            {
                return await session.Query<TRole>().SingleOrDefaultAsync(p => p.Id == roleId);
            }
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            using (IQuerySession session = this.documentStore.QuerySession())
            {
                return await session.Query<TRole>().SingleOrDefaultAsync(p => p.Name == normalizedRoleName);
            }
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name.ToLowerInvariant());
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.Name = normalizedName.ToLowerInvariant();
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            using (IDocumentSession session = this.documentStore.LightweightSession())
            {
                session.Update(role);
                await session.SaveChangesAsync();
            }
            return IdentityResult.Success;
        }
    }
}
