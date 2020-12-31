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
		/// Attempts to discover a service with the provided <see cref="serviceName"/>
		/// model.
		/// </summary>
		/// <param name="serviceName">The name of the service.</param>
		/// <returns>The result of the resolve request.</returns>
		[Get("/api/ServiceDiscovery/{name}/Discover")]
		Task<ResponseModel<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>> DiscoverServiceAsync([AliasAs("name")] string serviceName);
	}
}
