using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// <see cref="IServiceTypeIdentifierConverter{TServiceTypeEnum}"/> implementation that provided a normalized version of the service type string.
	/// </summary>
	/// <typeparam name="TServiceTypeEnum">The service type enumeration.</typeparam>
	public sealed class NormalizedServiceTypeIdentifierConverter<TServiceTypeEnum> : IServiceTypeIdentifierConverter<TServiceTypeEnum> 
		where TServiceTypeEnum : Enum
	{
		/// <summary>
		/// Indicates if the normalization should be to upper-case.
		/// Otherwise if false it will use lower-case.
		/// </summary>
		public bool UpperCase { get; init; } = true;

		public NormalizedServiceTypeIdentifierConverter()
		{
			
		}

		public string Create(TServiceTypeEnum context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			if (UpperCase)
				return DefaultServiceTypeIdentifierConverter<TServiceTypeEnum>.Instance.Create(context).ToUpperInvariant();
			else
				return DefaultServiceTypeIdentifierConverter<TServiceTypeEnum>.Instance.Create(context).ToLowerInvariant();
		}
	}
}
