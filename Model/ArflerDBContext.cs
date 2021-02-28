using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Arfler.Models
{
    public partial class ArflerDBContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<CategoryDetail> CategoryDetail { get; set; }
        public virtual DbSet<RoleDetail> RoleDetail { get; set; }
        public virtual DbSet<UserDetail> UserDetail { get; set; }
        public virtual DbSet<RestaurantDetail> RestaurantDetail { get; set; }
        public virtual DbSet<ImageDetail> ImageDetail { get; set; }
        public virtual DbSet<PeopleDetail> PeopleDetail { get; set; }
        public virtual DbSet<MenuCategoryDetail> MenuCategoryDetail { get; set; }
        public virtual DbSet<MenuItemDetail> MenuItemDetail { get; set; }
        public virtual DbSet<ReviewDetail> ReviewDetail { get; set; }
        public virtual DbSet<ContactDetails> ContactDetails { get; set; }
        public virtual DbSet<ReservationDetail> ReservationDetail { get; set; }
        public virtual DbSet<privacyPolicies> privacyPolicies { get; set; }
        //public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }


        public ArflerDBContext(DbContextOptions<ArflerDBContext> options) 
            : base(options)
        { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    optionsBuilder.UseSqlServer(@"Server=VNT-ADMIN\SQLSERVER;Database=ArflerDB;Trusted_Connection=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUser<string>>();
            modelBuilder.Ignore<ApplicationUser>();

            modelBuilder.Entity<CategoryDetail>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK_categoryDetail");

                entity.ToTable("categoryDetail");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.CategoryName)
                    .HasColumnName("categoryName")
                    .HasMaxLength(150);

                entity.Property(e => e.ParentCategoryId).HasColumnName("parentCategoryId");

                entity.Property(e => e.SortOrder).HasColumnName("sortOrder");
            });

            modelBuilder.Entity<RoleDetail>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK_roleDetail");

                entity.ToTable("roleDetail");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.RoleName)
                    .HasColumnName("roleName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_userDetail");

                entity.ToTable("userDetail");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modifiedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.UserAddress1)
                    .HasColumnName("userAddress1")
                    .HasMaxLength(200);

                entity.Property(e => e.UserAddress2)
                    .HasColumnName("userAddress2")
                    .HasMaxLength(200);

                entity.Property(e => e.UserCity)
                    .HasColumnName("userCity")
                    .HasMaxLength(50);

                entity.Property(e => e.UserCountry)
                    .HasColumnName("userCountry")
                    .HasMaxLength(50);

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasColumnName("userEmail")
                    .HasMaxLength(50);

                entity.Property(e => e.UserFirstName)
                    .HasColumnName("userFirstName")
                    .HasMaxLength(35);

                entity.Property(e => e.UserLastName)
                    .HasColumnName("userLastName")
                    .HasMaxLength(35);

                entity.Property(e => e.UserMiddleName)
                    .HasColumnName("userMiddleName")
                    .HasMaxLength(35);

                entity.Property(e => e.UserName)
                    .HasColumnName("userName")
                    .HasMaxLength(50);

                entity.Property(e => e.UserPhone)
                    .HasColumnName("userPhone")
                    .HasMaxLength(15);

                entity.Property(e => e.UserPhoto)
                    .HasColumnName("userPhoto")
                    .HasMaxLength(200);

                entity.Property(e => e.UserState)
                    .HasColumnName("userState")
                    .HasMaxLength(50);

                entity.Property(e => e.UserStatus).HasColumnName("userStatus");

                entity.Property(e => e.UserZipCode)
                    .HasColumnName("userZipCode")
                    .HasMaxLength(15);

                entity.Property(e => e.UserPassword)
                    .HasColumnName("userPassword")
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<RestaurantDetail>(entity =>
            {
                entity.HasKey(e => e.id)
                    .HasName("PK_restaurantDetail");

                entity.ToTable("restaurantDetail");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.restaurantName)
                    .HasColumnName("restaurantName")
                    .HasMaxLength(50);

                entity.Property(e => e.categoryIds)
                    .HasColumnName("categoryIds")
                    .HasMaxLength(50);

                entity.Property(e => e.address1)
                   .HasColumnName("address1")
                   .HasMaxLength(100);

                entity.Property(e => e.address2)
                   .HasColumnName("address2")
                   .HasMaxLength(100);

                entity.Property(e => e.createdDate)
                   .HasColumnName("createdDate")
                   .HasColumnType("datetime");

                entity.Property(e => e.city)
                  .HasColumnName("city")
                  .HasMaxLength(50);

                entity.Property(e => e.state)
                  .HasColumnName("state")
                  .HasMaxLength(50);

                entity.Property(e => e.country)
                  .HasColumnName("country")
                  .HasMaxLength(50);

                entity.Property(e => e.zipCode)
                  .HasColumnName("zipCode")
                  .HasMaxLength(50);

                entity.Property(e => e.mainImageUrl)
                    .HasColumnName("mainImageUrl")
                    .HasMaxLength(120);

                entity.Property(e => e.isEnable)
                   .HasColumnName("isEnable")
                   .HasColumnType("boolean");

                entity.Property(e => e.modifiedDate)
                  .HasColumnName("modifiedDate")
                  .HasColumnType("datetime");

                entity.Property(e => e.Description)
                  .HasColumnName("Description")
                  .HasMaxLength(800);

                entity.Property(e => e.intro)
                .HasColumnName("intro")
                .HasMaxLength(300);

                entity.Property(e => e.sortOrder).HasColumnName("sortOrder");

                entity.Property(e => e.userId)
                .HasColumnName("userId")
                .HasMaxLength(450);

                entity.Property(e => e.tagline)
                .HasColumnName("tagline")
                .HasMaxLength(500);
            });


            modelBuilder.Entity<ImageDetail>(entity =>
            {
                entity.HasKey(e => e.id)
                    .HasName("PK_imageDetail");

                entity.ToTable("imageDetail");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.restaurantId).HasColumnName("restaurantId");

                entity.Property(e => e.imageUrl)
                   .HasColumnName("imageUrl")
                   .HasMaxLength(120);

                entity.Property(e => e.isEnable)
                  .HasColumnName("isEnable")
                  .HasColumnType("boolean");

                entity.Property(e => e.createdDate)
                  .HasColumnName("createdDate")
                  .HasColumnType("datetime");

                entity.Property(e => e.modifiedDate)
                .HasColumnName("modifiedDate")
                .HasColumnType("datetime");

                entity.Property(e => e.sortOrder).HasColumnName("sortOrder");

                entity.Property(e => e.userId).HasColumnName("userId").HasMaxLength(450); ;
            });


            modelBuilder.Entity<PeopleDetail>(entity =>
            {
                entity.HasKey(e => e.id)
                    .HasName("PK_peopleDetail");

                entity.ToTable("peopleDetail");
                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.restaurantId).HasColumnName("restaurantId");

                entity.Property(e => e.firstName)
                   .HasColumnName("firstName")
                   .HasMaxLength(50);

                entity.Property(e => e.lastName)
                  .HasColumnName("lastName")
                  .HasMaxLength(50);

                entity.Property(e => e.isEnable)
                 .HasColumnName("isEnable")
                 .HasColumnType("boolean");

                entity.Property(e => e.createdDate)
                 .HasColumnName("createdDate")
                 .HasColumnType("datetime");

                entity.Property(e => e.modifiedDate)
                .HasColumnName("modifiedDate")
                .HasColumnType("datetime");

                entity.Property(e => e.address1)
                  .HasColumnName("address1")
                  .HasMaxLength(100);

                entity.Property(e => e.address2)
                   .HasColumnName("address2")
                   .HasMaxLength(100);

                entity.Property(e => e.city)
                 .HasColumnName("city")
                 .HasMaxLength(50);

                entity.Property(e => e.state)
                  .HasColumnName("state")
                  .HasMaxLength(50);

                entity.Property(e => e.country)
                  .HasColumnName("country")
                  .HasMaxLength(50);

                entity.Property(e => e.zip)
                  .HasColumnName("zip")
                  .HasMaxLength(50);

                entity.Property(e => e.designation)
                  .HasColumnName("designation")
                  .HasMaxLength(50);

                entity.Property(e => e.imageUrl)
                .HasColumnName("imageUrl")
                .HasMaxLength(120);

                entity.Property(e => e.facebookUrl)
              .HasColumnName("facebookUrl")
              .HasMaxLength(120);

                entity.Property(e => e.twitterUrl)
              .HasColumnName("twitterUrl")
              .HasMaxLength(150);


                entity.Property(e => e.userId)
                .HasColumnName("userId")
                .HasMaxLength(450);

                entity.Property(e => e.sortOrder).HasColumnName("sortOrder");
            });

            modelBuilder.Entity<MenuCategoryDetail>(entity =>
            {
                entity.HasKey(e => e.menuCategoryId)
                    .HasName("PK_menuCategoryDetail");

                entity.ToTable("menuCategoryDetail");

                entity.Property(e => e.menuCategoryId).HasColumnName("menuCategoryId");

                entity.Property(e => e.menuCategoryName)
                    .HasColumnName("menuCategoryName")
                    .HasMaxLength(80);

                entity.Property(e => e.restaurantId)
                    .HasColumnName("restaurantId");

                entity.Property(e => e.sortOrder).HasColumnName("sortOrder");

                entity.Property(e => e.isEnabled)
                   .HasColumnName("isEnabled")
                   .HasColumnType("boolean");

                entity.Property(e => e.createdDate)
                   .HasColumnName("createdDate")
                   .HasColumnType("datetime");

                entity.Property(e => e.modifiedDate)
                  .HasColumnName("modifiedDate")
                  .HasColumnType("datetime");

                entity.Property(e => e.menuCategoryDesc)
                  .HasColumnName("menuCategoryDesc")
                  .HasMaxLength(150);

                entity.Property(e => e.parentMenuCategoryId)
                  .HasColumnName("parentMenuCategoryId");
            });


            modelBuilder.Entity<MenuItemDetail>(entity =>
            {
                entity.HasKey(e => e.menuItemId)
                    .HasName("PK_MenuItemDetail");

                entity.ToTable("MenuItemDetail");

                entity.Property(e => e.menuItemId).HasColumnName("menuItemId");

                entity.Property(e => e.menuCategoryId).HasColumnName("menuCategoryId");

                entity.Property(e => e.restaurantId).HasColumnName("restaurantId");

                entity.Property(e => e.menuItemName)
                    .HasColumnName("menuItemName")
                    .HasMaxLength(80);

               

                entity.Property(e => e.sortOrder).HasColumnName("sortOrder");

                //entity.Property(e => e.isEnabled)
                //   .HasColumnName("isEnabled")
                //   .HasColumnType("boolean");

                entity.Property(e => e.createdDate)
                   .HasColumnName("createdDate")
                   .HasColumnType("datetime");

                //entity.Property(e => e.modifiedDate)
                //  .HasColumnName("modifiedDate")
                //  .HasColumnType("datetime");

                entity.Property(e => e.menuItemDesc)
                  .HasColumnName("menuItemDesc")
                  .HasMaxLength(300);


                entity.Property(e => e.menuItemRate)
                  .HasColumnName("menuItemRate")
                  .HasMaxLength(50);

                entity.Property(e => e.menuItemImageUrl)
                  .HasColumnName("menuItemImageUrl")
                   .HasMaxLength(150); 
            });

            modelBuilder.Entity<IdentityUser>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_IdentityUser");

                entity.ToTable("IdentityUser");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.AccessFailedCount).HasColumnName("AccessFailedCount");

                entity.Property(e => e.ConcurrencyStamp)
               .HasColumnName("ConcurrencyStamp");

                entity.Property(e => e.Email)
                 .HasColumnName("Email")
                 .HasMaxLength(256);

                entity.Property(e => e.EmailConfirmed)
                 .HasColumnName("EmailConfirmed")
                 .HasColumnType("boolean");

                entity.Property(e => e.LockoutEnabled)
                 .HasColumnName("LockoutEnabled")
                 .HasColumnType("boolean");

                entity.Property(e => e.LockoutEnd)
                 .HasColumnName("LockoutEnd")
                 .HasColumnType("datetime");

                entity.Property(e => e.NormalizedEmail)
                 .HasColumnName("NormalizedEmail")
                 .HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName)
                 .HasColumnName("NormalizedUserName")
                 .HasMaxLength(256);

                entity.Property(e => e.PasswordHash)
                  .HasColumnName("PasswordHash");

                entity.Property(e => e.PhoneNumber)
                .HasColumnName("PhoneNumber");

                entity.Property(e => e.PhoneNumberConfirmed)
                 .HasColumnName("PhoneNumberConfirmed")
                 .HasColumnType("boolean");


                entity.Property(e => e.SecurityStamp)
                .HasColumnName("SecurityStamp");

                entity.Property(e => e.TwoFactorEnabled)
                 .HasColumnName("TwoFactorEnabled")
                 .HasColumnType("boolean");

                entity.Property(e => e.UserName)
                   .HasColumnName("UserName")
                   .HasMaxLength(256);
            });

            modelBuilder.Entity<ReviewDetail>(entity =>
            {
                entity.HasKey(e => e.reviewId)
                    .HasName("PK_ReviewDetail");

                entity.ToTable("ReviewDetail");

                entity.Property(e => e.reviewId).HasColumnName("reviewId");

                entity.Property(e => e.review)
                    .HasColumnName("review")
                    .HasMaxLength(500);

                entity.Property(e => e.createdDate)
                  .HasColumnName("createdDate")
                  .HasColumnType("datetime");


                entity.Property(e => e.restaurantId).HasColumnName("restaurantId");


                entity.Property(e => e.reviewUserId).HasColumnName("reviewUserId");
            });


            modelBuilder.Entity<ContactDetails>(entity =>
            {
                entity.HasKey(e => e.contactId)
                    .HasName("PK_ContactDetails");

                entity.ToTable("ContactDetails");

                entity.Property(e => e.contactId).HasColumnName("contactId");

                entity.Property(e => e.contactMessage)
                    .HasColumnName("contactMessage")
                    .HasMaxLength(500);

                entity.Property(e => e.createdDate)
                  .HasColumnName("createdDate")
                  .HasColumnType("datetime");

                entity.Property(e => e.contactEmail)
                    .HasColumnName("contactEmail")
                    .HasMaxLength(50);

                entity.Property(e => e.contactName)
                  .HasColumnName("contactName")
                  .HasMaxLength(50);

                entity.Property(e => e.cSubject)
                .HasColumnName("cSubject")
                .HasMaxLength(300);

                entity.Property(e => e.restaurantId).HasColumnName("restaurantId");

            });

            modelBuilder.Entity<ReservationDetail>(entity =>
            {
                entity.HasKey(e => e.reservationId)
                    .HasName("PK_ReservationDetail");

                entity.ToTable("ReservationDetail");

                entity.Property(e => e.reservationId).HasColumnName("reservationId");

                entity.Property(e => e.firstName)
                    .HasColumnName("firstName")
                    .HasMaxLength(50);

                entity.Property(e => e.lastName)
                   .HasColumnName("lastName")
                   .HasMaxLength(50);

                entity.Property(e => e.reservationEmail)
                   .HasColumnName("reservationEmail")
                   .HasMaxLength(50);

                entity.Property(e => e.guestNum).HasColumnName("guestNum");

                entity.Property(e => e.reservationDate)
                    .HasColumnName("reservationDate")
                    .HasMaxLength(50);


                entity.Property(e => e.reservationTime)
                    .HasColumnName("reservationTime")
                    .HasMaxLength(50);

                entity.Property(e => e.reservationType)
                   .HasColumnName("reservationType")
                   .HasMaxLength(50);

                entity.Property(e => e.createdDate)
                  .HasColumnName("createdDate")
                  .HasColumnType("datetime");                

                entity.Property(e => e.restaurantId).HasColumnName("restaurantId");

            });

            modelBuilder.Entity<privacyPolicies>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_privacyPolicies");

                entity.ToTable("privacyPolicies");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(700);

                entity.Property(e => e.createdDate)
                  .HasColumnName("createdDate")
                  .HasColumnType("datetime");


                entity.Property(e => e.modifiedDate)
                  .HasColumnName("modifiedDate")
                  .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasColumnName("Title")
                    .HasMaxLength(200);

                entity.Property(e => e.restaurantId).HasColumnName("restaurantId");

            });
        }
    }
}