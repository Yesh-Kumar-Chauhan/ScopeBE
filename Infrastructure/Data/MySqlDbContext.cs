using Core.Entities.Brainyclock;
using Core.Modals;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }

        // Define DbSets for your tables here
        public DbSet<Location> Locations { get; set; }
        public DbSet<BrainyclockAttendance> Attendance { get; set; }
        public DbSet<EmployeeSchedules> EmployeeSchedules { get; set; }
        public DbSet<Employee> Employee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("locations");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CompanyId).HasColumnName("company_id");
                entity.Property(e => e.LocationName).HasColumnName("location_name");
                entity.Property(e => e.Address).HasColumnName("address");
                entity.Property(e => e.Pincode).HasColumnName("pincode");
                entity.Property(e => e.Latitude).HasColumnName("latitude");
                entity.Property(e => e.Longitude).HasColumnName("longitude");
                entity.Property(e => e.City).HasColumnName("city");
                entity.Property(e => e.Country).HasColumnName("country");
                entity.Property(e => e.State).HasColumnName("state");
                entity.Property(e => e.GeofenceRadius).HasColumnName("geofence_radius");
                entity.Property(e => e.SiteId).HasColumnName("scope_site_id");
            });

            modelBuilder.Entity<BrainyclockAttendance>(entity =>
            {
                entity.ToTable("attendances");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CompanyId).HasColumnName("company_id");
                entity.Property(e => e.ShiftId).HasColumnName("shift_id");
                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
                entity.Property(e => e.ClockIn).HasColumnName("clock_in_time");
                entity.Property(e => e.ClockOut).HasColumnName("clock_out_time");
                entity.Property(e => e.LunchIn).HasColumnName("lunch_in_time");
                entity.Property(e => e.LunchOut).HasColumnName("lunch_out_time");
                entity.Property(e => e.Overtime).HasColumnName("overtime");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.DepartmentId).HasColumnName("department_id");
                entity.Property(e => e.MarkAttendanceBy).HasColumnName("markAttendanceBy");
                entity.Property(e => e.EventId).HasColumnName("eventId");
                entity.Property(e => e.SiteId).HasColumnName("Site_id");
                entity.Property(e => e.DistNumber).HasColumnName("distNumber");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employees");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CompanyId).HasColumnName("company_id");
                entity.Property(e => e.DepartmentId).HasColumnName("department_id");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.FName).HasColumnName("fname");
                entity.Property(e => e.FirstName).HasColumnName("firstName");
                entity.Property(e => e.LastName).HasColumnName("lastName");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.ShiftId1).HasColumnName("shift_id_1");
                entity.Property(e => e.ShiftId2).HasColumnName("shift_id_2");
                entity.Property(e => e.ShiftId3).HasColumnName("shift_id_3");
                entity.Property(e => e.LocationId).HasColumnName("location_id");
                entity.Property(e => e.OverTime).HasColumnName("overTime");
                entity.Property(e => e.HourlyRate).HasColumnName("hourlyRate");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            });
        }
    }
}
