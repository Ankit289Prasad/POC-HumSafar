using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HumSafar.DL.Entities
{
    public class TourCategory
    {
        [Key]
        public int TravelId { get; set; }
        public string CategoryName { get; set; }
    }
}
