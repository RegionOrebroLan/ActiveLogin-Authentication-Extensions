using RegionOrebroLan.Configuration;

namespace RegionOrebroLan.ActiveLogin.Authentication.Configuration
{
	public class BankIdConfigurationOptions : ActiveLoginConfigurationOptions
	{
		#region Properties

		public virtual DynamicOptions ClientCertificateResolver { get; set; }
		public virtual DynamicOptions RootCertificateResolver { get; set; }

		#endregion
	}
}