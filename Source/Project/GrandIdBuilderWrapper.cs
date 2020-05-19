using System;
using ActiveLogin.Authentication.GrandId.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace RegionOrebroLan.ActiveLogin.Authentication
{
	public class GrandIdBuilderWrapper : ActiveLoginBuilder
	{
		#region Constructors

		public GrandIdBuilderWrapper(IGrandIdBuilder grandIdBuilder)
		{
			this.GrandIdBuilder = grandIdBuilder ?? throw new ArgumentNullException(nameof(grandIdBuilder));
		}

		#endregion

		#region Properties

		protected internal virtual IGrandIdBuilder GrandIdBuilder { get; }

		#endregion

		#region Methods

		public override void UseSimulatedEnvironment()
		{
			this.GrandIdBuilder.UseSimulatedEnvironment();
		}

		public override void UseSimulatedEnvironment(string givenName, string surname)
		{
			this.GrandIdBuilder.UseSimulatedEnvironment(givenName, surname);
		}

		public override void UseSimulatedEnvironment(string givenName, string personalIdentityNumber, string surname)
		{
			this.GrandIdBuilder.UseSimulatedEnvironment(givenName, surname, personalIdentityNumber);
		}

		#endregion
	}
}