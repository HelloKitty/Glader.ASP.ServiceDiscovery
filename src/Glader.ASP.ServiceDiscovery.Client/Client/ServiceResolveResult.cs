using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Model for representing the result of a service resolve attempt.
	/// </summary>
	/// <typeparam name="TServiceType">The service type.</typeparam>
	public sealed class ServiceResolveResult<TServiceType> : ResponseModel<TServiceType, ResolvedServiceEndpointResponseCode>
		where TServiceType : class
	{
		/// <summary>
		/// Indicates if the service is available.
		/// </summary>
		public bool isAvailable => base.isSuccessful;

		/// <summary>
		/// Resolved instance.
		/// </summary>
		public TServiceType Instance => base.Result;

		/// <summary>
		/// Creates a successful resolve attempt.
		/// Instance of the service must be non-null.
		/// </summary>
		/// <param name="instance">The service instance.</param>
		public ServiceResolveResult(TServiceType instance)
			: base(instance)
		{

		}

		/// <summary>
		/// Creates a failed resolve result.
		/// </summary>
		public ServiceResolveResult()
			: base(ResolvedServiceEndpointResponseCode.GeneralRequestError)
		{

		}

		/// <summary>
		/// Creates a failed resolve result.
		/// </summary>
		public ServiceResolveResult(ResolvedServiceEndpointResponseCode failureCode)
			: base(failureCode)
		{
			if (failureCode == ResolvedServiceEndpointResponseCode.Success)
				throw new ArgumentException($"Cannot use Success code for failed response mode.", nameof(failureCode));
		}
	}
}
