using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Glader.ASP.ServiceDiscovery
{
	[Table("services")]
	public class ServiceEntryModel
	{
		/// <summary>
		/// Unique identifier for the server.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("Id")]
		public int ServiceId { get; internal set; }

		/// <summary>
		/// Represents the Type of the service.
		/// </summary>
		[Required]
		public string ServiceType { get; internal set; }

		public ServiceEntryModel(int serviceId, string serviceType)
		{
			if(string.IsNullOrEmpty(serviceType)) throw new ArgumentException("Value cannot be null or empty.", nameof(serviceType));

			ServiceId = serviceId;
			ServiceType = serviceType.ToUpper();
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		internal ServiceEntryModel()
		{

		}
	}
}