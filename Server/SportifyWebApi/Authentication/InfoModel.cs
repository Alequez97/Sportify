using System.Collections.Generic;

namespace SportifyWebApi.Authentication
{
    public class InfoModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}
