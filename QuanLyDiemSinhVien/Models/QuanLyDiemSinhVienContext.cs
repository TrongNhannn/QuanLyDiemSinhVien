using Microsoft.AspNet.Identity.EntityFramework;
using QuanLyDiemSinhVien.Securities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace QuanLyDiemSinhVien.Models
{
    public class QuanLyDiemSinhVienConnection : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public QuanLyDiemSinhVienConnection()
            : base("name=QuanLyDiemSinhVienConnection")
        {
            Database.SetInitializer<QuanLyDiemSinhVienConnection>(null);
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<ApplicationGroup> ApplicationGroup { get; set; }
        public virtual DbSet<ApplicationUserClaim> ApplicationUserClaim { get; set; }
        public virtual DbSet<ApplicationRoleGroup> ApplicationRoleGroup { get; set; }
        public virtual DbSet<ApplicationUserGroup> ApplicationUserGroup { get; set; }

        public virtual DbSet<BangDiem> BangDiems { get; set; }
        public virtual DbSet<GiangVien> GiangViens { get; set; }
        public virtual DbSet<Khoa> Khoas { get; set; }
        public virtual DbSet<KhoaHoc> KhoaHocs { get; set; }
        public virtual DbSet<Lop_SinhVien> Lop_SinhVien { get; set; }
        public virtual DbSet<LopHoc> LopHocs { get; set; }
        public virtual DbSet<LopTinChi> LopTinChis { get; set; }
        public virtual DbSet<LopTinChi_SinhVien> LopTinChi_SinhVien { get; set; }
        public virtual DbSet<MonHoc> MonHocs { get; set; }
        public virtual DbSet<NganhDaoTao> NganhDaoTaos { get; set; }
        public virtual DbSet<NganhDaoTao_MonHoc> NganhDaoTao_MonHoc { get; set; }
        public virtual DbSet<SinhVien> SinhViens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // IMPORTANT: we are mapping the entity User to the same table as the entity ApplicationUser
            modelBuilder.Entity<IdentityUser>().HasKey(c => c.Id).ToTable("ApplicationUser");
            modelBuilder.Entity<IdentityUserRole>().ToTable("ApplicationUserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("ApplicationUserLogin");
            modelBuilder.Entity<IdentityUserClaim>().HasKey(c => c.Id).ToTable("ApplicationUserClaim");
            modelBuilder.Entity<IdentityRole>().HasKey(c => c.Id).ToTable("ApplicationRole");

            modelBuilder.Entity<ApplicationUser>().HasMany<IdentityUserRole>((ApplicationUser u) => u.Roles);

            modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) =>
                new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("ApplicationUserRole");

            // Add the group stuff here:
            modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserGroup>((ApplicationUser u) => u.UserGroups);
            modelBuilder.Entity<ApplicationUserGroup>().HasKey((ApplicationUserGroup r) => new { UserId = r.UserId, GroupId = r.GroupId }).ToTable("ApplicationUserGroup");

            // And here:
            modelBuilder.Entity<ApplicationGroup>().HasMany<ApplicationRoleGroup>((ApplicationGroup g) => g.RoleGroups);
            modelBuilder.Entity<ApplicationRoleGroup>().HasKey((ApplicationRoleGroup gr) => new { RoleId = gr.RoleId, GroupId = gr.GroupId }).ToTable("ApplicationRoleGroup");

            // And Here:
            EntityTypeConfiguration<ApplicationGroup> groupsConfig = modelBuilder.Entity<ApplicationGroup>().ToTable("ApplicationGroup");
            groupsConfig.HasKey(c => c.Id);
            groupsConfig.Property((ApplicationGroup r) => r.Name).IsRequired();

            // Leave this alone:
            EntityTypeConfiguration<IdentityUserLogin> entityTypeConfiguration =
                modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) =>
                    new { UserId = l.UserId, LoginProvider = l.LoginProvider, ProviderKey = l.ProviderKey }).ToTable("ApplicationUserLogin");

            EntityTypeConfiguration<IdentityUserClaim> table1 = modelBuilder.Entity<IdentityUserClaim>().ToTable("ApplicationUserClaim");

            modelBuilder.Entity<BangDiem>()
               .Property(e => e.DiemChu)
               .IsUnicode(false);

            modelBuilder.Entity<GiangVien>()
                .Property(e => e.MaGiangVien)
                .IsUnicode(false);

            modelBuilder.Entity<GiangVien>()
                .Property(e => e.SoDienThoai)
                .IsUnicode(false);

            modelBuilder.Entity<GiangVien>()
                .Property(e => e.SoCMT)
                .IsUnicode(false);

            modelBuilder.Entity<GiangVien>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Khoa>()
                .Property(e => e.MaKhoa)
                .IsUnicode(false);

            modelBuilder.Entity<Khoa>()
                .Property(e => e.KiHieu)
                .IsUnicode(false);

            modelBuilder.Entity<Khoa>()
                .HasMany(e => e.GiangViens)
                .WithOptional(e => e.Khoa)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Khoa>()
                .HasMany(e => e.NganhDaoTaos)
                .WithOptional(e => e.Khoa)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Khoa>()
                .HasMany(e => e.SinhViens)
                .WithOptional(e => e.Khoa)
                .WillCascadeOnDelete();

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.Lop_SinhVien)
                .WithOptional(e => e.LopHoc)
                .WillCascadeOnDelete();

            modelBuilder.Entity<LopTinChi>()
                .Property(e => e.MaLopTinChi)
                .IsUnicode(false);

            modelBuilder.Entity<LopTinChi>()
                .HasMany(e => e.LopTinChi_SinhVien)
                .WithOptional(e => e.LopTinChi)
                .HasForeignKey(e => e.LopTinhChiID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<MonHoc>()
                .Property(e => e.MaMonHoc)
                .IsUnicode(false);

            modelBuilder.Entity<MonHoc>()
                .HasMany(e => e.LopTinChis)
                .WithRequired(e => e.MonHoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MonHoc>()
                .HasMany(e => e.NganhDaoTao_MonHoc)
                .WithOptional(e => e.MonHoc)
                .WillCascadeOnDelete();

            modelBuilder.Entity<NganhDaoTao>()
                .HasMany(e => e.LopTinChis)
                .WithOptional(e => e.NganhDaoTao)
                .WillCascadeOnDelete();

            modelBuilder.Entity<NganhDaoTao>()
                .HasMany(e => e.NganhDaoTao_MonHoc)
                .WithOptional(e => e.NganhDaoTao)
                .HasForeignKey(e => e.NganhDaoTaoID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MaSinhVien)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.SoCMT)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.TonGiao)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.SoDTGD)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.SoDTDiDong)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .HasMany(e => e.Lop_SinhVien)
                .WithRequired(e => e.SinhVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SinhVien>()
                .HasMany(e => e.LopTinChi_SinhVien)
                .WithRequired(e => e.SinhVien)
                .WillCascadeOnDelete(false);

            Database.SetInitializer(new DropCreateDatabaseIfModelChangesInitializer());

            //SecurityService.InitData();
        }

        public class DropCreateDatabaseIfModelChangesInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
        {
            public DropCreateDatabaseIfModelChangesInitializer()
            {
                // IF CHANGES, RECREATE
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
            }

        }
        public class DropCreateDatabaseAlwaysInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
        {
            public DropCreateDatabaseAlwaysInitializer()
            {
                // Drop Create databasse
                Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseAlways<ApplicationDbContext>());
            }

        }

        public class CreateDatabaseIfNotExistsInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
        {
            public CreateDatabaseIfNotExistsInitializer()
            {
                // CREATE ONLY NOT EXITS
                Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
                //SecurityService.GroupInit();
                //SecurityService.RoleInit();
                //SecurityService.UserRoleInit();
            }

        }

    }
}