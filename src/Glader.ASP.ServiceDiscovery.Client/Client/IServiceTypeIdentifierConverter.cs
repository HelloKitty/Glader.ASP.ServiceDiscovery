using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Contract for types that can convert the service type enum value to an identifier name.
	/// </summary>
	/// <typeparam name="TServiceTypeEnum">The service enum type.</typeparam>
	public interface IServiceTypeIdentifierConverter<in TServiceTypeEnum> : IFactoryCreatable<string, TServiceTypeEnum>
		where TServiceTypeEnum : Enum
	{

	}
}
