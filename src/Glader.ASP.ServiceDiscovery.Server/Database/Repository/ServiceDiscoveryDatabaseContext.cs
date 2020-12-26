﻿using System;
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

		//We do the below for local database creation stuff
#if DATABASE_MIGRATION
		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//TODO: Should I have local or also AWS setup here?
			optionsBuilder.UseMySql("Server=127.0.0.1;Database=guardians.global;Uid=root;Pwd=test;");

			base.OnConfiguring(optionsBuilder);
		}
#endif

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
				.HasAlternateKey(s => s.ServiceName);
		}

		private static void SetupServiceEndpointEntryModel(ModelBuilder modelBuilder)
		{
			if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

			EntityTypeBuilder<ServiceEndpointModel> serviceEndpointEntity = modelBuilder.Entity<ServiceEndpointModel>();

			//Owns an endpoint.
			serviceEndpointEntity
				.OwnsOne(e => e.Endpoint);
		}
	}
}
