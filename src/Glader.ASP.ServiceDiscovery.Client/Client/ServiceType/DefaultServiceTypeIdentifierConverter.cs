using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Default <see cref="IServiceTypeIdentifierConverter{TServiceTypeEnum}"/> implementation converts the enum to a string.
	/// </summary>
	/// <typeparam name="TServiceTypeEnum"></typeparam>
	public sealed class DefaultServiceTypeIdentifierConverter<TServiceTypeEnum> : IServiceTypeIdentifierConverter<TServiceTypeEnum> 
		where TServiceTypeEnum : Enum
	{
		public static IServiceTypeIdentifierConverter<TServiceTypeEnum> Instance { get; } = new DefaultServiceTypeIdentifierConverter<TServiceTypeEnum>();

		static DefaultServiceTypeIdentifierConverter()
		{
			
		}

		public string Create(TServiceTypeEnum context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			return context.ToString();
		}
	}
}
