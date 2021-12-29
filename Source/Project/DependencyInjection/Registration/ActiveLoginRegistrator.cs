using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RegionOrebroLan.ActiveLogin.Authentication.Configuration;
using RegionOrebroLan.ActiveLogin.Authentication.Decoration;
using RegionOrebroLan.Web.Authentication;
using RegionOrebroLan.Web.Authentication.Configuration;
using RegionOrebroLan.Web.Authentication.Decoration.Configuration;
using RegionOrebroLan.Web.Authentication.DependencyInjection.Registration;

namespace RegionOrebroLan.ActiveLogin.Authentication.DependencyInjection.Registration
{
	public abstract class ActiveLoginRegistrator : Registrator
	{
		#region Fields

		private static bool _serviceConfigurationEnsured;

		#endregion

		#region Properties

		protected internal virtual bool ServiceConfigurationEnsured
		{
			get => _serviceConfigurationEnsured;
			set => _serviceConfigurationEnsured = value;
		}

		#endregion

		#region Methods

		public override void Add(ExtendedAuthenticationBuilder authenticationBuilder, string name, SchemeRegistrationOptions schemeRegistrationOptions)
		{
			this.EnsureServiceConfiguration(authenticationBuilder);
		}

		protected internal virtual void EnsureServiceConfiguration(ExtendedAuthenticationBuilder authenticationBuilder)
		{
			if(authenticationBuilder == null)
				throw new ArgumentNullException(nameof(authenticationBuilder));

			if(this.ServiceConfigurationEnsured)
				return;

			this.ServiceConfigurationEnsured = true;

			authenticationBuilder.Services.TryAddTransient<ActiveLoginAuthenticationPropertiesDecorator>();
			authenticationBuilder.Services.TryAddTransient<ActiveLoginCallbackDecorator>();
			authenticationBuilder.Services.Configure<ExtendedAuthenticationOptions>(options =>
			{
				options.AuthenticationPropertiesDecorators.Add("Active-Login-Authentication-Properties-Decorator", new DecoratorOptions
				{
					AuthenticationSchemes =
					{
						{ "*", 1000 }
					},
					Type = typeof(ActiveLoginAuthenticationPropertiesDecorator).AssemblyQualifiedName
				});

				options.CallbackDecorators.Add("Active-Login-Callback-Decorator", new DecoratorOptions
				{
					AuthenticationSchemes =
					{
						{ "*", 1000 }
					},
					Type = typeof(ActiveLoginCallbackDecorator).AssemblyQualifiedName
				});
			});
		}

		#endregion
	}

	public abstract class ActiveLoginRegistrator<T> : ActiveLoginRegistrator where T : ActiveLoginConfigurationOptions, new()
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