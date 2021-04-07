using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Serializable model representing a resolved endpoint and port.
	/// </summary>
	[JsonObject]
	public sealed class ResolvedEndpoint
	{
		/// <summary>
		/// Represents the address for the endpoint.
		/// </summary>
		[JsonProperty(Required = Required.Always, PropertyName = "EndpointAddress")] //JSON prop names here for backwards compat
		public string Address { get; internal set; }

		/// <summary>
		/// Represents the port for the endpoint.
		/// </summary>
		[JsonProperty(Required = Required.Always, PropertyName = "EndpointPort")] //JSON prop names here for backwards compat
		public int Port { get; internal set; }

		/// <summary>
		/// Creates a new valid resolved endpoint at <see cref="endpointAddress"/> and <see cref="endpointPort"/>.
		/// </summary>
		/// <param name="endpointAddress">The endpoint address.</param>
		/// <param name="endpointPort">The endpoint port.</param>
		public ResolvedEndpoint(string endpointAddress, int endpointPort)
		{
			if(string.IsNullOrWhiteSpace(endpointAddress)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(endpointAddress));
			if(endpointPort <= 0 || endpointPort >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(endpointPort));

			Address = endpointAddress;
			Port = endpointPort;
		}

		/// <summary>
		/// Protected serializer ctor
		/// </summary>
		[JsonConstructor]
		private ResolvedEndpoint()
		{

		}

		public static implicit operator Uri(ResolvedEndpoint endpoint) => new Uri($"{endpoint.Address}:{endpoint.Port}");

		public override string ToString()
		{
			return $"Address: {Address} Port: {Port}";
		}
	}
}