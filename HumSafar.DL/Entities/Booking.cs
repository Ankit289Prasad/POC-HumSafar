using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HumSafar.DL.Entities
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        [ForeignKey("User")]
        public string HumSafarUserId { get; set; }
        public HumSafarUser HumSafarUser { get; set; }
        public string TransportName { get; set; }
        public string TransportNumber { get; set; }
        [ForeignKey("TourCategory")]
        public int TourCategoryId { get; set; }
        public TourCategory TourCategory { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime? DateOfJourney { get; set; }
        public double Fare { get; set; }
        public string OriginImage { get; set; }
        public string DestinationImage { get; set; }
        public string PassengersName { get; set; }
        public string PaymentStatus { get; set; }
        public string BookingStatus { get; set; }
    }
}
