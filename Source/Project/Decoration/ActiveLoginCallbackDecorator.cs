using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.Logging.Extensions;
using RegionOrebroLan.Security.Claims;
using RegionOrebroLan.Web.Authentication;
using RegionOrebroLan.Web.Authentication.Decoration;
using RegionOrebroLan.Web.Authentication.Security.Claims;
using RegionOrebroLan.Web.Authentication.Security.Claims.Extensions;

namespace RegionOrebroLan.ActiveLogin.Authentication.Decoration
{
	/// <inheritdoc cref="ActiveLoginDecorator" />
	/// <inheritdoc cref="IAuthenticationDecorator" />
	[ServiceConfiguration(Lifetime = ServiceLifetime.Transient)]
	public class ActiveLoginCallbackDecorator : ActiveLoginDecorator, IAuthenticationDecorator
	{
		#region Constructors

		public ActiveLoginCallbackDecorator(IAuthenticationSchemeLoader authenticationSchemeLoader, ILoggerFactory loggerFactory) : base(authenticationSchemeLoader, loggerFactory) { }

		#endregion

		#region Properties

		public virtual string IdentityProvider { get; set; } = "BankID";
		protected internal virtual string OriginalIdentityProviderClaimType { get; set; } = "original-identity-provider";

		#endregion

		#region Methods

		public virtual async Task DecorateAsync(AuthenticateResult authenticateResult, string authenticationScheme, IClaimBuilderCollection claims, AuthenticationProperties properties)
		{
			try
			{
				if(authenticateResult == null)
					throw new ArgumentNullException(nameof(authenticateResult));

				if(claims == null)
					throw new ArgumentNullException(nameof(claims));

				if(!this.IsActiveLoginAuthenticationScheme(authenticationScheme))
					return;

				var identityProviderClaim = claims.FindFirstIdentityProviderClaim();

				if(identityProviderClaim == null)
				{
					identityProviderClaim = new ClaimBuilder
					{
						Type = ExtendedClaimTypes.IdentityProvider,
					};

					claims.Add(identityProviderClaim);
				}

				identityProviderClaim.Issuer = identityProviderClaim.OriginalIssuer = null;
				identityProviderClaim.Value = this.IdentityProvider;

				var originalIdentityProviderClaim = authenticateResult.Principal.Claims.FindFirstIdentityProviderClaim();

				if(originalIdentityProviderClaim != null)
				{
					claims.Add(new ClaimBuilder(originalIdentityProviderClaim)
					{
						Type = this.OriginalIdentityProviderClaimType
					});
				}

				await Task.CompletedTask.ConfigureAwait(false);
			}
			catch(Exception exception)
			{
				const string message = "Could not decorate active-login-callback.";

				this.Logger.LogErrorIfEnabled(exception, message);

				throw new InvalidOperationException(message, exception);
			}
		}

		#endregion
	}
}