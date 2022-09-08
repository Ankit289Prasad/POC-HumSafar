using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HumSafar.DL.Entities
{
    public class Tour
    {
        [Key]
        public int TourId { get; set; }
        public string TransportName { get; set; }
        public string TransportNumber { get; set; }

        [ForeignKey("TourCategory")]
        public int TourCategoryId { get; set; }
        public TourCategory TourCategory { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime? DateOfJourney { get; set; }
        public double Fare { get; set; }
        public double Discount { get; set; }
        public int SeatAvailable { get; set; }
        public string AvailabilityStatus { get; set; }
        public string OriginImage { get; set; }
        public string DestinationImage { get; set; }
        
    }
}
