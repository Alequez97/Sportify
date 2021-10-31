namespace DomainEntities
{
    /// <summary>
    /// A class that represents City model
    /// </summary>
    public class City : EntityBase
    {
        public string Name { get; set; }

        public Country Country { get; set; }
    }
}
