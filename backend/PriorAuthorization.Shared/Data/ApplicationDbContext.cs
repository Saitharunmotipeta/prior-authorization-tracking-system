using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PriorAuthorization.Shared.Entities;

namespace PriorAuthorization.Shared.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditHistory> AuditHistories { get; set; }

    public virtual DbSet<AuthorizationRequest> AuthorizationRequests { get; set; }

    public virtual DbSet<AuthorizationService> AuthorizationServices { get; set; }

    public virtual DbSet<Cptcode> Cptcodes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Encounter> Encounters { get; set; }

    public virtual DbSet<Facility> Facilities { get; set; }

    public virtual DbSet<Icdcode> Icdcodes { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Payer> Payers { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=PriorAuthorizationDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditHistory>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__AuditHis__5AF33E33C9BD28E5");

            entity.HasOne(d => d.Auth).WithMany(p => p.AuditHistories).HasConstraintName("FK_AuditHistory_Auth");

            entity.HasOne(d => d.Encounter).WithMany(p => p.AuditHistories).HasConstraintName("FK_AuditHistory_Encounter");
        });

        modelBuilder.Entity<AuthorizationRequest>(entity =>
        {
            entity.HasKey(e => e.AuthId).HasName("PK__Authoriz__6531B6F5BD59FE4D");

            entity.HasOne(d => d.Encounter).WithOne(p => p.AuthorizationRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorizationRequest_Encounter");

            entity.HasOne(d => d.Payer).WithMany(p => p.AuthorizationRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorizationRequest_Payer");
        });

        modelBuilder.Entity<AuthorizationService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Authoriz__3E0DB8AF05E40307");

            entity.HasOne(d => d.Auth).WithMany(p => p.AuthorizationServices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorizationService_Auth");

            entity.HasOne(d => d.CptCodeNavigation).WithMany(p => p.AuthorizationServices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorizationService_CPT");

            entity.HasOne(d => d.IcdCodeNavigation).WithMany(p => p.AuthorizationServices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorizationService_ICD");
        });

        modelBuilder.Entity<Cptcode>(entity =>
        {
            entity.HasKey(e => e.CptCode1).HasName("PK__CPTCode__6E3A1E26574C30E4");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__C2232422F8509D03");

            entity.HasOne(d => d.Facility).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Department_Facility");
        });

        modelBuilder.Entity<Encounter>(entity =>
        {
            entity.HasKey(e => e.EncounterId).HasName("PK__Encounte__CDF1340F4DBFBBDF");

            entity.HasOne(d => d.Department).WithMany(p => p.Encounters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Encounter_Department");

            entity.HasOne(d => d.Facility).WithMany(p => p.Encounters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Encounter_Facility");

            entity.HasOne(d => d.Patient).WithMany(p => p.Encounters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Encounter_Patient");
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__Facility__B2E8EAAE76796622");
        });

        modelBuilder.Entity<Icdcode>(entity =>
        {
            entity.HasKey(e => e.IcdCode1).HasName("PK__ICDCode__C282037F613EE440");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patient__4D5CE47646BA95FE");

            entity.Property(e => e.PatientId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Payer>(entity =>
        {
            entity.HasKey(e => e.PayerId).HasName("PK__Payer__9BC9732B89B14B98");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__Policy__47DA3F03BC80E982");

            entity.HasOne(d => d.Patient).WithMany(p => p.Policies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policy_Patient");

            entity.HasOne(d => d.Payer).WithMany(p => p.Policies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policy_Payer");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.ReminderId).HasName("PK__Reminder__E27A36289F2976AD");

            entity.HasOne(d => d.Auth).WithMany(p => p.Reminders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reminder_Auth");

            entity.HasOne(d => d.Payer).WithMany(p => p.Reminders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reminder_Payer");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
