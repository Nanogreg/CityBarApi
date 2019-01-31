namespace BeerCouponApi.Models
{
    public class Cities
    {
        public int Id { get; set; } // Id
        public int PostCode { get; set; } // Unique 
        public string Name { get; set; }
        public string About { get; set; }
        public string BannerPic { get; set; }
    }
}