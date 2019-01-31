using System;

namespace BeerCouponApi.Models
{
    public class Events
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int FkBarId { get; set; }
    }
}