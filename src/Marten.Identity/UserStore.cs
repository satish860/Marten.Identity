using Marten;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Marten.Identity
{
    public class UserStore<TUser, TRole> : IUserStore<TUser>
         where TUser : IdentityUser
         where TRole : IdentityRole
    {
        private readonly IDocumentStore documentStore;

        public UserStore(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                user.Id = Guid.NewGuid().ToString();
            }
            using (IDocumentSession session = this.documentStore.LightweightSession())
            {
                session.Store(user);
                await session.SaveChangesAsync();
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            using (IDocumentSession session = this.documentStore.LightweightSession())
            {
                session.Delete(user);
                await session.SaveChangesAsync();
            }
            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (IQuerySession session = this.documentStore.QuerySession())
            {
               return await session.Query<TUser>().SingleOrDefaultAsync(p => p.UserName == normalizedUserName);
            }
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                session.Update(user);
                await session.SaveChangesAsync();
            }
            return IdentityResult.Success;
        }
    }
}
