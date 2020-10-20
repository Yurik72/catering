namespace CateringPro.Test.Mocks
{
    using System.Threading.Tasks;
    using CateringPro.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    

    public class SignInManagerMock : SignInManager<CompanyUser>
    {
        internal const string ValidUser = "Valid@valid.com";
        internal const string TwoFactorRequired = "TwoFactor@invalid.com";
        internal const string LockedOutUser = "Locked@invalid.com";

        public SignInManagerMock(
            UserManager<CompanyUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<CompanyUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<CompanyUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<CompanyUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            if (userName == ValidUser && password == ValidUser)
            {
                return Task.FromResult(SignInResult.Success);
            }

            if (userName == TwoFactorRequired && password == TwoFactorRequired)
            {
                return Task.FromResult(SignInResult.TwoFactorRequired);
            }

            if (userName == LockedOutUser && password == LockedOutUser)
            {
                return Task.FromResult(SignInResult.LockedOut);
            }

            return Task.FromResult(SignInResult.Failed);
        }
    }
}