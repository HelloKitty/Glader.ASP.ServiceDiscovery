using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Glader.Essentials;
using Refit;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Contract for REST service that provides
	/// services discovery endpoints.
	/// </summary>
	[Headers("User-Agent: Glader")]
	public interface IServiceDiscoveryService
	{
		/// <summary>
		/// Attempts to discover a service with the provided <see cref="request"/>
		/// model.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The result of the resolve request.</returns>
		[Post("/api/ServiceDiscovery/Discover")]
		Task<ResponseModel<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>> DiscoverServiceAsync([Body(BodySerializationMethod.Serialized)] ResolvedServiceEndpointRequest request);
	}
}
