
namespace Marten.Identity
{
    using Marten;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="UserStore{TUser, TRole}" />.
    /// </summary>
    /// <typeparam name="TUser">.</typeparam>
    /// <typeparam name="TRole">.</typeparam>
    public class UserStore<TUser, TRole> : IUserStore<TUser>,
         IUserPasswordStore<TUser>,
         IUserClaimStore<TUser>
         where TUser : IdentityUser
         where TRole : IdentityRole
    {
        /// <summary>
        /// Defines the documentStore.
        /// </summary>
        private readonly IDocumentStore documentStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStore{TUser, TRole}"/> class.
        /// </summary>
        /// <param name="documentStore">The documentStore<see cref="IDocumentStore"/>.</param>
        public UserStore(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        /// <summary>
        /// The CreateAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IdentityResult}"/>.</returns>
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

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IdentityResult}"/>.</returns>
        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            using (IDocumentSession session = this.documentStore.LightweightSession())
            {
                session.Delete(user);
                await session.SaveChangesAsync();
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// The FindByIdAsync.
        /// </summary>
        /// <param name="userId">The userId<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{TUser}"/>.</returns>
        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (IQuerySession session = this.documentStore.QuerySession())
            {
                return await session.Query<TUser>().SingleOrDefaultAsync(p => p.Id == userId);
            }
        }

        /// <summary>
        /// The FindByNameAsync.
        /// </summary>
        /// <param name="normalizedUserName">The normalizedUserName<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{TUser}"/>.</returns>
        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (IQuerySession session = this.documentStore.QuerySession())
            {
                return await session.Query<TUser>().SingleOrDefaultAsync(p => p.UserName == normalizedUserName);
            }
        }

        /// <summary>
        /// The GetNormalizedUserNameAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName.ToLowerInvariant());
        }

        /// <summary>
        /// The GetUserIdAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        /// <summary>
        /// The GetUserNameAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        /// <summary>
        /// The SetNormalizedUserNameAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="normalizedName">The normalizedName<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName.ToLowerInvariant();
            return Task.CompletedTask;
        }

        /// <summary>
        /// The SetUserNameAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="userName">The userName<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IdentityResult}"/>.</returns>
        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                session.Update(user);
                await session.SaveChangesAsync();
            }
            return IdentityResult.Success;
        }

        #region IUserPasswordStore
        /// <summary>
        /// The GetPasswordHashAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// The SetPasswordHashAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="passwordHash">The passwordHash<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        /// <summary>
        /// The HasPasswordAsync.
        /// </summary>
        /// <param name="user">The user<see cref="TUser"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!String.IsNullOrEmpty(user.PasswordHash));
        }
        #endregion

        public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            IList<Claim> result = user.Claims
                .Select(c => new Claim(c.ClaimType, c.ClaimValue))
                .ToList();
            return Task.FromResult(result);
        }

        public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            IEnumerable<IdentityUserClaim> identityUserClaims = claims
                .Select(p => new IdentityUserClaim { ClaimType = p.Type, ClaimValue = p.Value });
            user.Claims.AddRange(identityUserClaims);
            return Task.CompletedTask;
        }

        public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            var indexOfClaim = user.Claims.FindIndex(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
            if (indexOfClaim != -1)
            {
                user.Claims.RemoveAt(indexOfClaim);
                await this.AddClaimsAsync(user, new[] { newClaim }, cancellationToken);
            }
        }

        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.Claims.RemoveAll(identityClaim => claims.Any(c => c.Type == identityClaim.ClaimType && c.Value == identityClaim.ClaimValue));
            return Task.CompletedTask;
        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            using (var querySession = documentStore.QuerySession())
            {
              var userList= await querySession.Query<TUser>()
                    .Where(p => p.Claims.Any(q => q.ClaimType == claim.Type && q.ClaimValue == claim.Value))
                    .ToListAsync();
                return userList.ToList();
            }
        }
        
    }
}
