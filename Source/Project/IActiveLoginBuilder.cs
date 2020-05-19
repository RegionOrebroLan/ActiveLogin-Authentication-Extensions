using RegionOrebroLan.ActiveLogin.Authentication.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication
{
	public interface IActiveLoginBuilder
	{
		#region Methods

		void UseSimulatedEnvironment(SimulatedPersonOptions simulatedPerson);

		#endregion
	}
}