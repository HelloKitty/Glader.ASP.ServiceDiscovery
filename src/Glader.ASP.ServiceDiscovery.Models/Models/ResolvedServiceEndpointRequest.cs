using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Request that resolves a specific service's name.
	/// </summary>
	[JsonObject]
	public sealed class ResolvedServiceEndpointRequest
	{
		/// <summary>
		/// Indicates the service requested for resolution.
		/// </summary>
		[JsonProperty]
		public string ServiceType { get; private set; }

		public ResolvedServiceEndpointRequest(string serviceType)
		{
			if(string.IsNullOrWhiteSpace(serviceType)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceType));
			ServiceType = serviceType;
		}

		/// <summary>
		/// Protected serializer ctor
		/// </summary>
		[JsonConstructor]
		private ResolvedServiceEndpointRequest()
		{

		}
	}
}