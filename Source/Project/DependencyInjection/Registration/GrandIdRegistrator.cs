using ActiveLogin.Authentication.GrandId.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.ActiveLogin.Authentication.Configuration;
using RegionOrebroLan.Web.Authentication;
using RegionOrebroLan.Web.Authentication.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication.DependencyInjection.Registration
{
	public abstract class GrandIdRegistrator : ActiveLoginRegistrator<ActiveLoginConfigurationOptions>
	{
		#region Methods

		public override void Add(ExtendedAuthenticationBuilder authenticationBuilder, string name, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			base.Add(authenticationBuilder, name, schemeRegistrationOptions);

			authenticationBuilder.AddGrandId(builder =>
			{
				var grandIdConfiguration = this.GetConfigurationOptions(authenticationBuilder, schemeRegistrationOptions);

				// ReSharper disable ConvertIfStatementToSwitchStatement
				if(grandIdConfiguration.Environment == ActiveLoginEnvironment.Simulated)
				{
					new GrandIdBuilderWrapper(builder).UseSimulatedEnvironment(grandIdConfiguration.SimulatedPerson);
				}
				else
				{
					if(grandIdConfiguration.Environment == ActiveLoginEnvironment.Test)
						builder.UseTestEnvironment(environmentConfiguration => this.Bind(authenticationBuilder, environmentConfiguration, schemeRegistrationOptions));
					else
						builder.UseProductionEnvironment(environmentConfiguration => this.Bind(authenticationBuilder, environmentConfiguration, schemeRegistrationOptions));
				}
				// ReSharper restore ConvertIfStatementToSwitchStatement

				this.Add(authenticationBuilder, builder, name, schemeRegistrationOptions);
			});
		}

		protected internal abstract void Add(ExtendedAuthenticationBuilder authenticationBuilder, IGrandIdBuilder grandIdBuilder, string name, SchemeRegistrationOptions schemeRegistrationOptions);

		#endregion
	}
}