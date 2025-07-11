using Microsoft.EntityFrameworkCore;

namespace WebApi.Models.Context;

public partial class dbContext : DbContext
{
    public dbContext()
    {
    }

    public dbContext(DbContextOptions<dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblHabitacion> TblHabitacions { get; set; }

    public virtual DbSet<TblReserva> TblReservas { get; set; }

    public virtual DbSet<TblRol> TblRols { get; set; }

    public virtual DbSet<TblUsuario> TblUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    //    => optionsBuilder.UseSqlServer(configuration.GetConnectionString("cnxLocal"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblHabitacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_habi__3213E83F18CAC207");

            entity.ToTable("tbl_habitacion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Habitacion)
                .HasMaxLength(10)
                .HasColumnName("habitacion");
        });

        modelBuilder.Entity<TblReserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_rese__3213E83FC8C865AE");

            entity.ToTable("tbl_reserva");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");
            entity.Property(e => e.FechaSalida).HasColumnName("fecha_salida");
            entity.Property(e => e.IdHabitacion).HasColumnName("idHabitacion");
            entity.Property(e => e.IdUser).HasColumnName("idUser");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.TblReservas)
                .HasForeignKey(d => d.IdHabitacion)
                .HasConstraintName("FK_reserva_habitacion");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblReservas)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_reserva_usuario");
        });

        modelBuilder.Entity<TblRol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_rol__3213E83F255A8514");

            entity.ToTable("tbl_rol");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Rol)
                .HasMaxLength(100)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<TblUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_usua__3213E83F8A00BC91");

            entity.ToTable("tbl_usuario");

            entity.HasIndex(e => e.NickName, "UQ__tbl_usua__48F06EC1370CC107").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__tbl_usua__AB6E6164B00B4820").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.HashPassword).HasMaxLength(500);
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("lastName");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NickName)
                .HasMaxLength(100)
                .HasColumnName("nickName");
            entity.Property(e => e.Salt).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("updatedDate");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.TblUsuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_user_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
