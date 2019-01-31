namespace BeerCouponApi.Models
{
    public class Bars
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string About { get; set; }
        public string TAGS { get; set; }
        public string BarPic { get; set; }
        public int FkCityId { get; set; }
    }
}