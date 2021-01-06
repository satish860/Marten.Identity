using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Marten.Identity.Tests
{
    public class UserStoreTests
    {
        [Fact]
        public async Task Should_be_Able_to_Get_User_Id_by_User()
        {
            IUserStore<IdentityUser> userStore = new UserStore<IdentityUser,IdentityRole>(null);
            var id= await userStore.GetUserIdAsync(new IdentityUser { Id = "1" },CancellationToken.None);
            id.Should().Be("1");
        }

        [Fact]
        public async Task Should_be_Able_to_get_user_By_Name()
        {
            IUserStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(null);
            var name = await userStore.GetUserNameAsync(new IdentityUser { Id = "1",UserName="Satish" }, CancellationToken.None);
            name.Should().Be("Satish");
        }

        [Fact]
        public async Task Should_be_Able_to_get_user_NormalizedName()
        {
            IUserStore<IdentityUser> userStore = new UserStore<IdentityUser, IdentityRole>(null);
            var name = await userStore.GetNormalizedUserNameAsync(new IdentityUser { Id = "1", UserName = "Satish" }, CancellationToken.None);
            name.Should().Be("satish");
        }
    } 
}
