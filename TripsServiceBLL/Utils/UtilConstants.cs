namespace TripsServiceBLL.Utils
{
	public static class UtilConstants
	{
		public const string JwtKey = "9NniFIgxk18oQt48SdNb0zKxyCRfo1Ed";

		public const string JwtIssuer = "planyourtrips.com";

		public const string AesKey = "cgJOF9VOMJ2FM1ZRCfPdvWcNWr5fPPsd";

		public const string AesSecret = "Nl8NaASrhR77xy6t";

		public const string ImagesFolderName = "images";

		public static UtilDelegates.StringMapperDelegate GetExistingCredentialMessage = 
			(credential) => $"This {credential} is already taken";

		public const double AuthorizationExpirationInDays = 7;

		public const double JwtExpirationInHours = 1;

		public const string UserIdClaimName = "userId";

		public const string InvalidCredentialsMessage = "Invalid credentials. Please, try again";

		public const string JwtTokenCookiesAlias = "jwt";

		public const string DriversFolderName = "drivers";

		public const string DefaultDatabaseExceptionMessage = "Oops, something went wrong. Please, try again";

		public const double DaysInYear = 365.25;

		public const double DaysInMonth = 30.4375;
	}
}
