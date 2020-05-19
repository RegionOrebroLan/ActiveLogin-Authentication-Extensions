using RegionOrebroLan.ActiveLogin.Authentication.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication
{
	public abstract class ActiveLoginBuilder : IActiveLoginBuilder
	{
		#region Methods

		public abstract void UseSimulatedEnvironment();

		public virtual void UseSimulatedEnvironment(SimulatedPersonOptions simulatedPerson)
		{
			if(simulatedPerson == null)
				this.UseSimulatedEnvironment();
			else if(simulatedPerson.PersonalIdentityNumber == null)
				this.UseSimulatedEnvironment(simulatedPerson.GivenName, simulatedPerson.Surname);
			else
				this.UseSimulatedEnvironment(simulatedPerson.GivenName, simulatedPerson.PersonalIdentityNumber, simulatedPerson.Surname);
		}

		public abstract void UseSimulatedEnvironment(string givenName, string surname);
		public abstract void UseSimulatedEnvironment(string givenName, string personalIdentityNumber, string surname);

		#endregion
	}
}