using BlueWater.OrderMangement.DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlueWater.OrderMangement.DataAccess
{
    public class OrderDataContext : DbContext
    {

        /// <summary>
        /// ReadModel Schema Name
        /// </summary>
        internal const string Schema = "dbo";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDataContext"/> class.
        /// </summary>
        /// <param name="options">DbContext Options</param>
        public OrderDataContext(DbContextOptions<OrderDataContext> options)
            : base(options)
        {
            // Do nothing
        }

        /// <summary>
        /// Configuring DateTime to UTC DateTime
        /// </summary>
        /// <param name="optionsBuilder">options Builder</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }
            optionsBuilder.ConfigureWarnings(w => w.Log(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.ManyServiceProvidersCreatedWarning));
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Gets or sets Products
        /// </summary>

        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Create models
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}