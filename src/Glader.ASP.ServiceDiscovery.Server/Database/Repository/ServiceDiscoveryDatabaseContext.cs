using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// <see cref="DbContext"/> for <see cref="ServiceEntryModel"/> and <see cref="ServiceEndpointModel"/>
	/// </summary>
	public sealed class ServiceDiscoveryDatabaseContext : DbContext
	{
		/// <summary>
		/// The service entries.
		/// </summary>
		public DbSet<ServiceEntryModel> Services { get; set; }

		/// <summary>
		/// Endpoints for <see cref="ServiceEntryModel"/>s.
		/// </summary>
		public DbSet<ServiceEndpointModel> ServiceEndpoints { get; set; }

		public ServiceDiscoveryDatabaseContext(DbContextOptions<ServiceDiscoveryDatabaseContext> options)
			: base(options)
		{

		}

		public ServiceDiscoveryDatabaseContext()
		{

		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			SetupServiceEntryModel(modelBuilder);
			SetupServiceEndpointEntryModel(modelBuilder);

			base.OnModelCreating(modelBuilder);
		}

		private static void SetupServiceEntryModel(ModelBuilder modelBuilder)
		{
			if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

			EntityTypeBuilder<ServiceEntryModel> serviceEntity = modelBuilder.Entity<ServiceEntryModel>();

			//Makes the name a unique entry.
			serviceEntity
				.HasAlternateKey(s => s.ServiceType);
		}

		private static void SetupServiceEndpointEntryModel(ModelBuilder modelBuilder)
		{
			if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

			EntityTypeBuilder<ServiceEndpointModel> serviceEndpointEntity = modelBuilder.Entity<ServiceEndpointModel>();

			//Owns an endpoint.
			serviceEndpointEntity
				.OwnsOne(e => e.Endpoint);

			//Service key is now based on name and id, can have many linked services.
			serviceEndpointEntity
				.HasKey(model => new {model.ServiceId, model.Name});
		}
	}
}
