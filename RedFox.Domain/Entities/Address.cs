namespace RedFox.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string street  { get; set; } = string.Empty;
        public string suite   { get; set; } = string.Empty;
        public string city    { get; set; } = string.Empty;
        public string zipcode { get; set; } = string.Empty;

        public int GeoId    { get; set; }
        public Geo? Geo     { get; set; }
    }
}
