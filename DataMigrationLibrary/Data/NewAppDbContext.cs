using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationLibrary.Data
{
    public class NewAppDbContext : DbContext
    {
        public NewAppDbContext(DbContextOptions<NewAppDbContext> options) : base(options)
        {
        }

        public DbSet<District> Districts { get; set; } // Map this to the new District table
        public DbSet<Site> Sites { get; set; } // Map this to the old Sites table
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

        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<AbsenceReasons> AbsenceReasons { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<WaiversReceived> WaiversReceived { get; set; }
        public DbSet<CertificateType> CertificateType { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<WaiversSent> WaiversSent { get; set; }
        public DbSet<Director> Directors { get; set; }
    }
}
