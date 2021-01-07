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
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
