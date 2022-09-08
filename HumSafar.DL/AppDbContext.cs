using HumSafar.DL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HumSafar.DL
{
    public class AppDbContext:IdentityDbContext<HumSafarUser>
    {
        public DbSet<TourCategory> TourCategories { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
