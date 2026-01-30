

using MealMake.Domain.Domain_Models;
using MealMake.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MealMake.Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext<MealMakeApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<UserFavoriteMeal> UserFavoriteMeals { get; set; }
        public DbSet<MealCollection> MealCollections { get; set; }
        public DbSet<CollectionMeal> CollectionMeals { get; set; }
        public DbSet<CollectionCategory> CollectionCategories { get; set; }
        public DbSet<MealCollectionCategory> MealCollectionCategories { get; set; }
        public DbSet<UserActiveCollection> UserActiveCollections { get; set; }
        public DbSet<ArchivedCollection> ArchivedCollections { get; set; }
        public DbSet<ArchivedCollectionMeal> ArchivedCollectionMeals { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MealCollection>()
            .HasOne(mc => mc.User)
            .WithMany(u => u.MealCollections)
            .HasForeignKey(mc => mc.UserId)
            .OnDelete(DeleteBehavior.Restrict);


            // Composite key for UserFavoriteMeal
            modelBuilder.Entity<UserFavoriteMeal>()
                .HasKey(uf => new { uf.UserId, uf.MealId });

            // Unique category name
            modelBuilder.Entity<CollectionCategory>()
             .HasOne(c => c.User)
             .WithMany(u => u.Categories)
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Restrict); 


            // UserActiveCollection: only one active per user
            modelBuilder.Entity<UserActiveCollection>(entity =>
            {
                entity.HasIndex(uac => uac.UserId).IsUnique();

                entity.HasOne(uac => uac.User)
                      .WithMany()
                      .HasForeignKey(uac => uac.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(uac => uac.Collection)
                      .WithMany()
                      .HasForeignKey(uac => uac.CollectionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ArchivedCollection -> ArchivedCollectionMeal relationship
            modelBuilder.Entity<ArchivedCollectionMeal>()
                .HasOne(acm => acm.ArchivedCollection)
                .WithMany(ac => ac.Meals)
                .HasForeignKey(acm => acm.ArchivedCollectionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MealCollectionCategory>()
            .HasOne(mcc => mcc.MealCollection)
            .WithMany(mc => mc.Categories)
            .HasForeignKey(mcc => mcc.MealCollectionId)
            .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<MealCollectionCategory>()
                .HasOne(mcc => mcc.CollectionCategory)
                .WithMany(cc => cc.MealCollections)
                .HasForeignKey(mcc => mcc.CollectionCategoryId)
                .OnDelete(DeleteBehavior.Restrict); 


        }

    }
}

