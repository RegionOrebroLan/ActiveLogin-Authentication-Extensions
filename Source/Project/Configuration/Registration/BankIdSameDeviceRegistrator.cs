using ActiveLogin.Authentication.BankId.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Web.Authentication;
using RegionOrebroLan.Web.Authentication.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration.Registration
{
	public class BankIdSameDeviceRegistrator : BankIdRegistrator
	{
		#region Methods

		protected internal override void Add(ExtendedAuthenticationBuilder authenticationBuilder, IBankIdBuilder bankIdBuilder, string name, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			// ReSharper disable AssignNullToNotNullAttribute
			bankIdBuilder.AddSameDevice(name, schemeRegistrationOptions?.DisplayName, options => { this.Bind(authenticationBuilder, options, schemeRegistrationOptions); });
			// ReSharper restore AssignNullToNotNullAttribute
		}

		#endregion
	}
}