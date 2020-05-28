using RegionOrebroLan.Web.Authentication;
using RegionOrebroLan.Web.Authentication.Configuration;
using RegionOrebroLan.Web.Authentication.Configuration.Registration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration.Registration
{
	public abstract class ActiveLoginRegistrator<T> : Registrator where T : ActiveLoginConfigurationOptions, new()
	{
		#region Methods

		protected internal virtual T GetConfigurationOptions(ExtendedAuthenticationBuilder authenticationBuilder, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			var options = new T();

			this.Bind(authenticationBuilder, options, schemeRegistrationOptions);

			return options;
		}

		#endregion
	}
}