namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration
{
	public class ActiveLoginConfigurationOptions
	{
		#region Properties

		public virtual ActiveLoginEnvironment Environment { get; set; } = ActiveLoginEnvironment.Production;
		public virtual SimulatedPersonOptions SimulatedPerson { get; set; }

		#endregion
	}
}