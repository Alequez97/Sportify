namespace SportifyWebApi.IntegrationTests.Constants
{
    public static class Endpoints
    {
        private static readonly string _baseUrl = "https://localhost:44314/api/"; // TODO: Replace with reading from configuration

        public static class Accounts
        {
            private static readonly string _identityBaseUrl = string.Concat(_baseUrl, "accounts");

            public static readonly string GetAuthenticatedUserInfo = string.Concat(_identityBaseUrl, "/auth-user-info");

            public static readonly string GetUserInfo = string.Concat(_identityBaseUrl, "/user-info/{username}");

            public static readonly string Login = string.Concat(_identityBaseUrl, "/login");

            public static readonly string Register = string.Concat(_identityBaseUrl, "/register");

            public static readonly string RegisterAdmin = string.Concat(_identityBaseUrl, "/register-admin");
        }

        public static class Map
        {
            private static readonly string _mapBaseUrl = string.Concat(_baseUrl, "map");

            public static readonly string GetLocations = _mapBaseUrl;
            
            public static readonly string GetSportsGroundTypes = string.Concat(_mapBaseUrl, "/types");

            public static readonly string SaveNewImage = string.Concat(_mapBaseUrl, "/save-images");

            public static readonly string SaveNewLocation = string.Concat(_mapBaseUrl, "/save");
        }
    }
}
