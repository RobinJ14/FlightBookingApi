﻿using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Context
{
    [ExcludeFromCodeCoverage]
    public class newContext : DbContext
    {


        public DbSet<Admin> Admins { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightOwner> FlightsOwners { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Models.Route> Routes { get; set; }
        public DbSet<PassengerBooking> PassengerBookings { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Refund> Refunds { get; set; }

        public newContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Models.Route>()
                .HasOne(r => r.SourceAirport)
                .WithMany()
                .HasForeignKey(r => r.SourceAirportId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Models.Route>()
                .HasOne(r => r.DestinationAirport)
                .WithMany()
                .HasForeignKey(r => r.DestinationAirportId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }

    }

}