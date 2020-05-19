using ActiveLogin.Authentication.BankId.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Web.Authentication.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration.Registrators
{
	public class BankIdSameDeviceRegistrator : BankIdRegistrator
	{
		#region Methods

		protected internal override void Add(IBankIdBuilder bankIdBuilder, IConfiguration configuration, string name, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			// ReSharper disable AssignNullToNotNullAttribute
			bankIdBuilder.AddSameDevice(name, schemeRegistrationOptions?.DisplayName, options => { this.Bind(configuration, options, schemeRegistrationOptions); });
			// ReSharper restore AssignNullToNotNullAttribute
		}

		#endregion
	}
}