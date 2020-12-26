using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Glader.ASP.ServiceDiscovery
{
	[Table("service_endpoints")]
	public class ServiceEndpointModel
	{
		/// <summary>
		/// Identifier of the service endpoint entry.
		/// </summary>
		[Key]
		public int ServiceId { get; internal set; }

		/// <summary>
		/// The foreign relation service reference.
		/// See: <see cref="ServiceEntryModel"/>
		/// </summary>
		[ForeignKey(nameof(ServiceId))]
		public virtual ServiceEntryModel Service { get; internal set; }

		/// <summary>
		/// The endpoint address for the service.
		/// </summary>
		public ResolvedEndpoint Endpoint { get; internal set; }

		public ServiceEndpointModel(int serviceId, ResolvedEndpoint endpoint)
		{
			if(serviceId <= 0) throw new ArgumentOutOfRangeException(nameof(serviceId));

			ServiceId = serviceId;
			Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		internal ServiceEndpointModel()
		{

		}
	}
}
