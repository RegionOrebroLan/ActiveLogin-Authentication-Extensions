using System;
using System.Security.Cryptography.X509Certificates;
using ActiveLogin.Authentication.BankId.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Abstractions.Extensions;
using RegionOrebroLan.Configuration;
using RegionOrebroLan.Security.Cryptography.Configuration;
using RegionOrebroLan.Web.Authentication;
using RegionOrebroLan.Web.Authentication.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration.Registration
{
	public abstract class BankIdRegistrator : ActiveLoginRegistrator<BankIdConfigurationOptions>
	{
		#region Properties

		protected internal virtual X509Certificate2 ClientCertificate { get; set; }
		protected internal virtual X509Certificate2 RootCertificate { get; set; }

		#endregion

		#region Methods

		public override void Add(ExtendedAuthenticationBuilder authenticationBuilder, string name, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			base.Add(authenticationBuilder, name, schemeRegistrationOptions);

			authenticationBuilder.AddBankId(builder =>
			{
				var bankIdConfiguration = this.GetConfigurationOptions(authenticationBuilder, schemeRegistrationOptions);

				//builder.Configure(options => { this.Bind(configuration, options, schemeRegistrationOptions); });

				if(bankIdConfiguration.Environment == ActiveLoginEnvironment.Simulated)
				{
					new BankIdBuilderWrapper(builder).UseSimulatedEnvironment(bankIdConfiguration.SimulatedPerson);
				}
				else
				{
					if(bankIdConfiguration.Environment == ActiveLoginEnvironment.Test)
						builder.UseTestEnvironment();
					else
						builder.UseProductionEnvironment();

					this.ClientCertificate = this.GetCertificate(authenticationBuilder, bankIdConfiguration.ClientCertificate);
					this.RootCertificate = this.GetCertificate(authenticationBuilder, bankIdConfiguration.RootCertificate);

					builder.UseClientCertificate(() => this.ClientCertificate);
					builder.UseRootCaCertificate(() => this.RootCertificate);
				}

				builder.UseQrCoderQrCodeGenerator();

				this.Add(authenticationBuilder, builder, name, schemeRegistrationOptions);
			});
		}

		protected internal abstract void Add(ExtendedAuthenticationBuilder authenticationBuilder, IBankIdBuilder bankIdBuilder, string name, SchemeRegistrationOptions schemeRegistrationOptions);

		protected internal virtual X509Certificate2 GetCertificate(ExtendedAuthenticationBuilder authenticationBuilder, DynamicOptions dynamicOptions)
		{
			try
			{
				if(authenticationBuilder == null)
					throw new ArgumentNullException(nameof(authenticationBuilder));

				if(dynamicOptions == null)
					throw new ArgumentNullException(nameof(dynamicOptions));

				var resolverOptions = (ResolverOptions) authenticationBuilder.InstanceFactory.Create(dynamicOptions.Type);

				dynamicOptions.Options?.Bind(resolverOptions);

				var certificate = authenticationBuilder.CertificateResolver.ResolveAsync(resolverOptions).Result;

				return certificate.Unwrap<X509Certificate2>();
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not get certificate from options with type \"{dynamicOptions?.Type}\".", exception);
			}
		}

		#endregion
	}
}