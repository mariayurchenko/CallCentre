namespace SB.WebShared.Dynamics
{
    public static class SBCustomSettingsModel
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_sbcustomsettings";

		#region Attribute Names
		public static class Fields
		{
			public const string
				D365URL = "sb_d365url",
				PhoneCallsClosing = "sb_phonecallsclosing",
				Phonecallsfortheday = "sb_phonecallsfortheday",
				PrimaryId = "sb_sbcustomsettingsid",
				PrimaryName = "sb_name",
				TokenLifetime = "sb_tokenlifetime",
				UserName = "sb_username",
				UserPassword = "sb_userpassword";

			public static string[] All => new[] { D365URL,
				PhoneCallsClosing,
				Phonecallsfortheday,
				PrimaryId,
				PrimaryName,
				TokenLifetime,
				UserName,
				UserPassword };
		}
		#endregion
    }
}