﻿namespace BeerCouponApi.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int FkBarId { get; set; }
    }
}