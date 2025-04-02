using Core.Entities;
using Core.Modals;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define DbSets for each entity model
        public DbSet<District> Districts { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Personel> Personel { get; set; }
        public DbSet<Positions> Positions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Inservice> Inservices { get; set; }
        public DbSet<TopicType> TopicType { get; set; }
        public DbSet<WorkshopType> WorkshopType { get; set; }
        public DbSet<Closing> Closings { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Timesheet> Timesheet { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<WorkshopMember> WorkshopMembers { get; set; }
        public DbSet<WorkshopTopic> WorkshopTopics { get; set; }
        public DbSet<Workshop> Workshops { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<SchedularTimesheet> SchedularTimesheets { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<AbsenceReasons> AbsenceReasons { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<WaiversReceived> WaiversReceived { get; set; }
        public DbSet<CertificateType> CertificateType { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<WaiversSent> WaiversSent { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<EmployeeScheduleImportStatus> EmployeeScheduleImportStatus { get; set; }


        // Add other DbSets here as needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<District>().ToTable("Districts");
            modelBuilder.Entity<Site>().ToTable("Sites");
            modelBuilder.Entity<Personel>().ToTable("Personel");
            modelBuilder.Entity<Positions>().ToTable("Positions");
            modelBuilder.Entity<Report>().ToTable("Reports");
            modelBuilder.Entity<School>().ToTable("Schools");

            base.OnModelCreating(modelBuilder);
        }

    }
}
