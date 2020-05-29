using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.Logging.Extensions;
using RegionOrebroLan.Web.Authentication;
using RegionOrebroLan.Web.Authentication.Configuration;
using RegionOrebroLan.Web.Authentication.Decoration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Decoration
{
	/// <inheritdoc cref="ActiveLoginDecorator" />
	/// <inheritdoc cref="IAuthenticationPropertiesDecorator" />
	[ServiceConfiguration(Lifetime = ServiceLifetime.Transient)]
	public class ActiveLoginAuthenticationPropertiesDecorator : ActiveLoginDecorator, IAuthenticationPropertiesDecorator
	{
		#region Constructors

		public ActiveLoginAuthenticationPropertiesDecorator(IOptions<ExtendedAuthenticationOptions> authenticationOptions, IAuthenticationSchemeLoader authenticationSchemeLoader, IOptionsMonitor<CookieAuthenticationOptions> cookieAuthenticationOptionsMonitor, ILoggerFactory loggerFactory) : base(authenticationSchemeLoader, loggerFactory)
		{
			this.AuthenticationOptions = authenticationOptions ?? throw new ArgumentNullException(nameof(authenticationOptions));
			this.CookieAuthenticationOptionsMonitor = cookieAuthenticationOptionsMonitor ?? throw new ArgumentNullException(nameof(cookieAuthenticationOptionsMonitor));
		}

		#endregion

		#region Properties

		protected internal virtual IOptions<ExtendedAuthenticationOptions> AuthenticationOptions { get; }
		protected internal virtual IOptionsMonitor<CookieAuthenticationOptions> CookieAuthenticationOptionsMonitor { get; }

		#endregion

		#region Methods

		[SuppressMessage("Design", "CA1054:Uri parameters should not be strings")]
		public virtual async Task DecorateAsync(string authenticationScheme, AuthenticationProperties properties, string returnUrl)
		{
			try
			{
				if(properties == null)
					throw new ArgumentNullException(nameof(properties));

				if(!this.IsActiveLoginAuthenticationScheme(authenticationScheme))
					return;

				var defaultScheme = this.AuthenticationOptions.Value.DefaultScheme;

				if(string.IsNullOrWhiteSpace(defaultScheme))
					return;

				var cookieAuthenticationOptions = this.CookieAuthenticationOptionsMonitor.Get(defaultScheme);

				var cancelUrlBuilder = new UriBuilder
				{
					Path = cookieAuthenticationOptions.LoginPath
				};

				if(!string.IsNullOrWhiteSpace(returnUrl))
				{
					var queryString = HttpUtility.ParseQueryString(cancelUrlBuilder.Query);

					queryString.Set(nameof(returnUrl), returnUrl);

					// ReSharper disable AssignNullToNotNullAttribute
					cancelUrlBuilder.Query = queryString.ToString();
					// ReSharper restore AssignNullToNotNullAttribute
				}

				properties.SetString("cancelReturnUrl", cancelUrlBuilder.Uri.PathAndQuery);

				await Task.CompletedTask.ConfigureAwait(false);
			}
			catch(Exception exception)
			{
				const string message = "Could not decorate active-login-authentication-properties.";

				this.Logger.LogErrorIfEnabled(exception, message);

				throw new InvalidOperationException(message, exception);
			}
		}

		#endregion
	}
}