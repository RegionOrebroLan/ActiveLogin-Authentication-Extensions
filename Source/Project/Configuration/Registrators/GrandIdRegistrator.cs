using ActiveLogin.Authentication.GrandId.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Web.Authentication.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration.Registrators
{
	public abstract class GrandIdRegistrator : ActiveLoginRegistrator<ActiveLoginConfigurationOptions>
	{
		#region Methods

		public override void Add(AuthenticationBuilder authenticationBuilder, IConfiguration configuration, string name, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			var grandIdConfiguration = this.GetConfigurationOptions(configuration, schemeRegistrationOptions);

			authenticationBuilder.AddGrandId(builder =>
			{
				// ReSharper disable ConvertIfStatementToSwitchStatement
				if(grandIdConfiguration.Environment == ActiveLoginEnvironment.Simulated)
				{
					new GrandIdBuilderWrapper(builder).UseSimulatedEnvironment(grandIdConfiguration.SimulatedPerson);
				}
				else
				{
					if(grandIdConfiguration.Environment == ActiveLoginEnvironment.Test)
						builder.UseTestEnvironment(environmentConfiguration => this.Bind(configuration, environmentConfiguration, schemeRegistrationOptions));
					else
						builder.UseProductionEnvironment(environmentConfiguration => this.Bind(configuration, environmentConfiguration, schemeRegistrationOptions));
				}
				// ReSharper restore ConvertIfStatementToSwitchStatement

				this.Add(builder, configuration, name, schemeRegistrationOptions);
			});
		}

		protected internal abstract void Add(IGrandIdBuilder grandIdBuilder, IConfiguration configuration, string name, SchemeRegistrationOptions schemeRegistrationOptions);

		#endregion
	}
}