using System;
using ActiveLogin.Authentication.BankId.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace RegionOrebroLan.ActiveLogin.Authentication
{
	public class BankIdBuilderWrapper : ActiveLoginBuilder
	{
		#region Constructors

		public BankIdBuilderWrapper(IBankIdBuilder bankIdBuilder)
		{
			this.BankIdBuilder = bankIdBuilder ?? throw new ArgumentNullException(nameof(bankIdBuilder));
		}

		#endregion

		#region Properties

		protected internal virtual IBankIdBuilder BankIdBuilder { get; }

		#endregion

		#region Methods

		public override void UseSimulatedEnvironment()
		{
			this.BankIdBuilder.UseSimulatedEnvironment();
		}

		public override void UseSimulatedEnvironment(string givenName, string surname)
		{
			this.BankIdBuilder.UseSimulatedEnvironment(givenName, surname);
		}

		public override void UseSimulatedEnvironment(string givenName, string personalIdentityNumber, string surname)
		{
			this.BankIdBuilder.UseSimulatedEnvironment(givenName, surname, personalIdentityNumber);
		}

		#endregion
	}
}