namespace SportifyWebApi.IntegrationTests.Constants
{
    public static class Endpoints
    {
        private static readonly string _baseUrl = "https://localhost:44314/api/"; // TODO: Replace with reading from configuration

        public static class Accounts
        {
            private static readonly string _accountsBaseUrl = string.Concat(_baseUrl, "accounts");

            public static readonly string GetAuthenticatedUserInfo = string.Concat(_accountsBaseUrl, "/auth-user-info");

            public static readonly string GetUserInfo = string.Concat(_accountsBaseUrl, "/user-info/{username}");

            public static readonly string Login = string.Concat(_accountsBaseUrl, "/login");

            public static readonly string Register = string.Concat(_accountsBaseUrl, "/register");

            public static readonly string RegisterAdmin = string.Concat(_accountsBaseUrl, "/register-admin");
        }

        public static class Events
        {
            private static readonly string _eventsBaseUrl = string.Concat(_baseUrl, "events");

            public static readonly string CreateEvent = string.Concat(_eventsBaseUrl, "/create");

            public static readonly string DeleteEvent = string.Concat(_eventsBaseUrl, "/delete/{id}");

            public static readonly string DisjoinEvent = string.Concat(_eventsBaseUrl, "/disjoin");

            public static readonly string EditEvent = string.Concat(_eventsBaseUrl, "/edit/{id}");

            public static readonly string GetEvent = string.Concat(_eventsBaseUrl, "/{id}");

            public static readonly string GetEvents = _eventsBaseUrl;

            public static readonly string GetEventsCategories = string.Concat(_eventsBaseUrl, "/categories");

            public static readonly string JoinEvent = string.Concat(_eventsBaseUrl, "/join");
        }

        public static class Map
        {
            private static readonly string _mapBaseUrl = string.Concat(_baseUrl, "map");

            public static readonly string GetLocations = _mapBaseUrl;
            
            public static readonly string GetSportsGroundTypes = string.Concat(_mapBaseUrl, "/types");

            public static readonly string SaveNewImages = string.Concat(_mapBaseUrl, "/save-images");

            public static readonly string SaveNewLocation = string.Concat(_mapBaseUrl, "/save");
        }
    }
}
