using ActiveLogin.Authentication.GrandId.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Web.Authentication.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration.Registrators
{
	public class GrandIdOtherDeviceRegistrator : GrandIdRegistrator
	{
		#region Methods

		protected internal override void Add(IGrandIdBuilder grandIdBuilder, IConfiguration configuration, string name, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			// ReSharper disable AssignNullToNotNullAttribute
			grandIdBuilder.AddBankIdOtherDevice(name, schemeRegistrationOptions?.DisplayName, options => { this.Bind(configuration, options, schemeRegistrationOptions); });
			// ReSharper restore AssignNullToNotNullAttribute
		}

		#endregion
	}
}