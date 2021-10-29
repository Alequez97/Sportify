namespace DomainEntities
{
    /// <summary>
    /// Class that represents SportsGroundsLocation model
    /// </summary>
    class SportsGroundsLocation : EntityBase
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public int GeolocationId { get; set; }
        public Geolocation Geolocation { get; set; }
    }
}
