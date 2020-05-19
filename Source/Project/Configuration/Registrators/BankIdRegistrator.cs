using System;
using System.Security.Cryptography.X509Certificates;
using ActiveLogin.Authentication.BankId.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Abstractions.Extensions;
using RegionOrebroLan.Configuration;
using RegionOrebroLan.Security.Cryptography;
using RegionOrebroLan.Security.Cryptography.Configuration;
using RegionOrebroLan.Web.Authentication.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration.Registrators
{
	public abstract class BankIdRegistrator : ActiveLoginRegistrator<BankIdConfigurationOptions>
	{
		#region Fields

		private ICertificateResolver _certificateResolver;
		private X509Certificate2 _clientCertificate;
		private ResolverOptions _clientCertificateResolverOptions;
		private X509Certificate2 _rootCertificate;
		private ResolverOptions _rootCertificateResolverOptions;

		#endregion

		#region Properties

		protected internal virtual BankIdConfigurationOptions BankIdConfiguration { get; set; }

		protected internal virtual ICertificateResolver CertificateResolver
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._certificateResolver == null)
				{
					var services = new ServiceCollection();

					services.AddSingleton(AppDomain.CurrentDomain);
					services.AddSingleton<FileCertificateResolver>();
					services.AddSingleton<IApplicationDomain, AppDomainWrapper>();
					services.AddSingleton<ICertificateResolver, CertificateResolver>();
					services.AddSingleton<StoreCertificateResolver>();

					this._certificateResolver = services.BuildServiceProvider().GetRequiredService<ICertificateResolver>();
				}
				// ReSharper restore InvertIf

				return this._certificateResolver;
			}
		}

		protected internal virtual X509Certificate2 ClientCertificate => this._clientCertificate ??= this.ClientCertificateResolverOptions.Resolve(this.CertificateResolver).Unwrap<X509Certificate2>();

		protected internal virtual Func<X509Certificate2> ClientCertificateFunction => () =>
		{
			try
			{
				return this.ClientCertificate;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException(this.GetCertificateNotSetupExceptionMessage("client"), exception);
			}
		};

		protected internal virtual ResolverOptions ClientCertificateResolverOptions => this._clientCertificateResolverOptions ??= this.CreateResolverOptions(this.BankIdConfiguration.ClientCertificateResolver);
		protected internal virtual X509Certificate2 RootCertificate => this._rootCertificate ??= this.RootCertificateResolverOptions.Resolve(this.CertificateResolver).Unwrap<X509Certificate2>();

		protected internal virtual Func<X509Certificate2> RootCertificateFunction => () =>
		{
			try
			{
				return this.RootCertificate;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException(this.GetCertificateNotSetupExceptionMessage("root"), exception);
			}
		};

		protected internal virtual ResolverOptions RootCertificateResolverOptions => this._rootCertificateResolverOptions ??= this.CreateResolverOptions(this.BankIdConfiguration.RootCertificateResolver);

		#endregion

		#region Methods

		public override void Add(AuthenticationBuilder authenticationBuilder, IConfiguration configuration, string name, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			if(authenticationBuilder == null)
				throw new ArgumentNullException(nameof(authenticationBuilder));

			this.BankIdConfiguration = this.GetConfigurationOptions(configuration, schemeRegistrationOptions);

			authenticationBuilder.AddBankId(builder =>
			{
				//builder.Configure(options => { this.Bind(configuration, options, schemeRegistrationOptions); });

				if(this.BankIdConfiguration.Environment == ActiveLoginEnvironment.Simulated)
				{
					new BankIdBuilderWrapper(builder).UseSimulatedEnvironment(this.BankIdConfiguration.SimulatedPerson);
				}
				else
				{
					if(this.BankIdConfiguration.Environment == ActiveLoginEnvironment.Test)
						builder.UseTestEnvironment();
					else
						builder.UseProductionEnvironment();

					builder.UseClientCertificate(this.ClientCertificateFunction);
					builder.UseRootCaCertificate(this.RootCertificateFunction);
				}

				builder.UseQrCoderQrCodeGenerator();

				this.Add(builder, configuration, name, schemeRegistrationOptions);
			});
		}

		protected internal abstract void Add(IBankIdBuilder bankIdBuilder, IConfiguration configuration, string name, SchemeRegistrationOptions schemeRegistrationOptions);

		protected internal virtual ResolverOptions CreateResolverOptions(DynamicOptions dynamicOptions)
		{
			if(dynamicOptions == null)
				throw new ArgumentNullException(nameof(dynamicOptions));

			var resolverOptions = (ResolverOptions) Activator.CreateInstance(Type.GetType(dynamicOptions.Type, true, true));

			dynamicOptions.Options?.Bind(resolverOptions);

			return resolverOptions;
		}

		protected internal virtual string GetCertificateNotSetupExceptionMessage(string certificateKind)
		{
			return $"The {certificateKind}-certificate for bank-id is not set up correctly. Preferably configure it as a dynamic-option in the configuration-file.";
		}

		#endregion
	}
}