namespace TripsServiceBLL.Utils
{
    public static class Constants
    {
        public const string JwtKey = "9NniFIgxk18oQt48SdNb0zKxyCRfo1Ed";

        public const string JwtIssuer = "planyourtrips.com";

        public const string AesKey = "cgJOF9VOMJ2FM1ZRCfPdvWcNWr5fPPsd";

        public const string AesIV = "Nl8NaASrhR77xy6t";

        public const string ImagesFolderName = "images";

        public static string GetExistingCredentialMessage(string credential)
        {
            return $"This {credential} is already taken";
        }

        public const double AuthorizationExpirationInDays = 7;

        public const double JwtExpirationInHours = 1;

        public const string UserIdClaimName = "userId";

        public const string InvalidCredentialsMessage = "Invalid credentials. Please, try again";

        public const string JwtTokenCookiesAlias = "jwt";

        public static string GetEntityNotFoundMessage(string entityName)
        {
            return $"Corresponding {entityName} was not found";
        }

        public static string GetEntityNotExistsMessage(string entityName)
        {
            return $"This {entityName} doesn't exist anymore";
        }

        public const string DriversFolderName = "drivers";

        public const string DefaultDatabaseExceptionMessage = "Oops, something went wrong. Please, try again";

        public const double DaysInYear = 365.25;

        public const double DaysInMonth = 30.4375;
    }
}
