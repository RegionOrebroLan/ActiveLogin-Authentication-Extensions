using System;
using ActiveLogin.Authentication.BankId.AspNetCore;
using ActiveLogin.Authentication.GrandId.AspNetCore;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.Web.Authentication;
using RegionOrebroLan.Web.Authentication.Decoration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Decoration
{
	/// <inheritdoc />
	public abstract class ActiveLoginDecorator : Decorator
	{
		#region Constructors

		protected ActiveLoginDecorator(IAuthenticationSchemeLoader authenticationSchemeLoader, ILoggerFactory loggerFactory) : base(loggerFactory)
		{
			this.AuthenticationSchemeLoader = authenticationSchemeLoader ?? throw new ArgumentNullException(nameof(authenticationSchemeLoader));
		}

		#endregion

		#region Properties

		protected internal virtual IAuthenticationSchemeLoader AuthenticationSchemeLoader { get; }

		#endregion

		#region Methods

		protected internal virtual bool IsActiveLoginAuthenticationScheme(string authenticationScheme)
		{
			// ReSharper disable InvertIf
			if(authenticationScheme != null)
			{
				var handlerType = this.AuthenticationSchemeLoader.GetAsync(authenticationScheme).Result?.HandlerType;

				if(handlerType != null)
				{
					if(typeof(BankIdHandler).IsAssignableFrom(handlerType) || typeof(GrandIdBankIdHandler).IsAssignableFrom(handlerType))
						return true;
				}
			}
			// ReSharper restore InvertIf

			return false;
		}

		#endregion
	}
}