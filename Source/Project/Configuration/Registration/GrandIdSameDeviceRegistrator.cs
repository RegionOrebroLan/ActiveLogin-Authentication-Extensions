using ActiveLogin.Authentication.GrandId.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Web.Authentication;
using RegionOrebroLan.Web.Authentication.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration.Registration
{
	public class GrandIdSameDeviceRegistrator : GrandIdRegistrator
	{
		#region Methods

		protected internal override void Add(ExtendedAuthenticationBuilder authenticationBuilder, IGrandIdBuilder grandIdBuilder, string name, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			// ReSharper disable AssignNullToNotNullAttribute
			grandIdBuilder.AddBankIdSameDevice(name, schemeRegistrationOptions?.DisplayName, options => { this.Bind(authenticationBuilder, options, schemeRegistrationOptions); });
			// ReSharper restore AssignNullToNotNullAttribute
		}

		#endregion
	}
}