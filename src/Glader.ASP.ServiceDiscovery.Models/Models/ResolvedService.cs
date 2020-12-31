using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Data-model that represents a named resolved service of a specific type.
	/// (ServiceType not specified in this model)
	/// </summary>
	[JsonObject]
	public sealed class ResolvedService
	{
		/// <summary>
		/// Represents the sub-name of the service.
		/// (Ex. Service: GAMESERVER SubName: DerpServer2)
		/// </summary>
		[JsonProperty]
		public string Name { get; private set; }

		/// <summary>
		/// Represents the service's endpoint.
		/// </summary>
		[JsonProperty]
		public ResolvedEndpoint Endpoint { get; private set; }

		/// <summary>
		/// Creates a new resolved service with the specified <see cref="Name"/>
		/// and <see cref="Endpoint"/>
		/// </summary>
		/// <param name="name">The service's subname.</param>
		/// <param name="endpoint">The service's endpoint.</param>
		public ResolvedService(string name, ResolvedEndpoint endpoint)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		[JsonConstructor]
		public ResolvedService()
		{
			
		}
	}
}
