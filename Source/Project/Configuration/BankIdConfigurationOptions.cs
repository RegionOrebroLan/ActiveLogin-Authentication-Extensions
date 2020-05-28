using RegionOrebroLan.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration
{
	public class BankIdConfigurationOptions : ActiveLoginConfigurationOptions
	{
		#region Properties

		public virtual DynamicOptions ClientCertificate { get; set; }
		public virtual DynamicOptions RootCertificate { get; set; }

		#endregion
	}
}