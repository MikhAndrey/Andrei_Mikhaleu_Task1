namespace TripsServiceBLL.Utils
{
    public static class Constants
    {
        public const string JwtKey = "9NniFIgxk18oQt48SdNb0zKxyCRfo1Ed";

        public const string JwtIssuer = "planyourtrips.com";

        public const string AesKey = "cgJOF9VOMJ2FM1ZRCfPdvWcNWr5fPPsd";

        public const string AesIV = "Nl8NaASrhR77xy6t";

        public const string ImagesFolderName = "images";

        public const string ExistingUserNameMessage = "This username is already taken";

        public const string ExistingEmailMessage = "This email is already taken";

        public const double AuthorizationExpirationInDays = 7;

        public const double JwtExpirationInHours = 1;

        public const string UserIdClaimName = "userId";

        public const string InvalidCredentialsMessage = "Invalid credentials. Please, try again";

        public const string JwtTokenCookiesAlias = "jwt";
    }
}
