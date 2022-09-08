using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HumSafar.DL.Entities
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public string Description { get; set; }

        [ForeignKey("User")]
        public string HumSafarUserId { get; set; }
        public HumSafarUser HumSafarUser { get; set; }

    }
}
