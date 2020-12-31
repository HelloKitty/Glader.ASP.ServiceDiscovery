using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Represents a response model to a full service resolution request.
	/// </summary>
	[JsonObject]
	public sealed class ServiceResolutionResult
	{
		/// <summary>
		/// Internal-serialized <see cref="Services"/>
		/// </summary>
		[JsonProperty(PropertyName = "Services")]
		internal ResolvedService[] _Services { get; set; } = Array.Empty<ResolvedService>();

		/// <summary>
		/// Represents the services as part of the service resolution request.
		/// Can be empty.
		/// </summary>
		[JsonIgnore]
		public IEnumerable<ResolvedService> Services => _Services;

		/// <summary>
		/// Creates a resolution result with the specified <see cref="Services"/>
		/// </summary>
		/// <param name="services">The resolved services.</param>
		public ServiceResolutionResult(ResolvedService[] services)
		{
			_Services = services ?? throw new ArgumentNullException(nameof(services));
		}

		/// <summary>
		/// Creates an empty resolution result.
		/// <see cref="Services"/> will be non-null but empty.
		/// </summary>
		[JsonConstructor]
		public ServiceResolutionResult()
		{
			
		}
	}
}
