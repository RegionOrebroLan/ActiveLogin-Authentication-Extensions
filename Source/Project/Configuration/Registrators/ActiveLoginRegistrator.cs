using Microsoft.Extensions.Configuration;
using RegionOrebroLan.Web.Authentication.Configuration;
using RegionOrebroLan.Web.Authentication.Configuration.Registrators;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration.Registrators
{
	public abstract class ActiveLoginRegistrator<T> : Registrator where T : ActiveLoginConfigurationOptions, new()
	{
		#region Methods

		protected internal virtual T GetConfigurationOptions(IConfiguration configuration, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			var options = new T();

			this.Bind(configuration, options, schemeRegistrationOptions);

			return options;
		}

		#endregion
	}
}