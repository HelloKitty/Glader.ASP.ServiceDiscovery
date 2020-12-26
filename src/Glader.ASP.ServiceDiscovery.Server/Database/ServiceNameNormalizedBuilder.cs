using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Normalizes a service's name.
	/// </summary>
	public sealed class ServiceNameNormalizedBuilder
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ServiceNameNormalizedBuilder Create(string serviceName)
		{
			return new ServiceNameNormalizedBuilder(serviceName);
		}

		/// <summary>
		/// The name of the service.
		/// </summary>
		public string ServiceName { get; }

		/// <summary>
		/// Creates a new normalized name builder.
		/// </summary>
		/// <param name="serviceName">Service name.</param>
		public ServiceNameNormalizedBuilder(string serviceName)
		{
			ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
		}

		/// <summary>
		/// Call to normalized <see cref="ServiceName"/>
		/// </summary>
		/// <returns>Normalized service name.</returns>
		public override string ToString()
		{
			return ServiceName.ToUpperInvariant();
		}
	}
}
