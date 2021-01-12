
namespace Marten.Identity
{
    /// <summary>
    /// Defines the <see cref="IdentityUserClaim" />.
    /// </summary>
    public class IdentityUserClaim
    {
        /// <summary>
        /// Gets or sets the ClaimType.
        /// </summary>
        public string ClaimType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ClaimValue.
        /// </summary>
        public string ClaimValue { get; set; } = string.Empty;
    }
}
