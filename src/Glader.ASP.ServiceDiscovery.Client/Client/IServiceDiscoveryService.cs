using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
		/// Attempts to discover a service with the provided <see cref="serviceType"/>
		/// model.
		/// </summary>
		/// <param name="serviceType">The type of the service.</param>
		/// <param name="groupName">Represents the group name that the services are grouped by. (Ex. Default, Local, Game1, Game2)</param>
		/// <param name="token"></param>
		/// <returns>The result of the resolve request.</returns>
		[Get("/api/ServiceDiscovery/{name}/Single/{group}")]
		Task<ResponseModel<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>> DiscoverServiceAsync([AliasAs("name")] string serviceType, [AliasAs("group")] string groupName, CancellationToken token = default);

		/// <summary>
		/// Attempts to discover a service with the provided <see cref="serviceType"/>
		/// model.
		/// </summary>
		/// <param name="serviceType">The type of the service.</param>
		/// <param name="token"></param>
		/// <returns>The result of the resolve request.</returns>
		[Get("/api/ServiceDiscovery/{name}/Single")]
		Task<ResponseModel<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>> DiscoverServiceAsync([AliasAs("name")] string serviceType, CancellationToken token = default);

		/// <summary>
		/// Attempts to discover all services of a specified <see cref="serviceType"/>
		/// model.
		/// </summary>
		/// <param name="serviceType">The type of the service.</param>
		/// <param name="token"></param>
		/// <returns>The result a result of all services resolved.</returns>
		[Get("/api/ServiceDiscovery/{name}/All")]
		Task<ResponseModel<ServiceResolutionResult, ResolvedServiceEndpointResponseCode>> DiscoverServicesAsync([AliasAs("name")] string serviceType, CancellationToken token = default);
	}
}
