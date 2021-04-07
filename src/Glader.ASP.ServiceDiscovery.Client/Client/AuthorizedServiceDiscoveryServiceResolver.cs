using System;
using System.ComponentModel;
using System.Net.Http;
using Glader.Essentials;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// <see cref="IServiceDiscoveryService"/>-based implementation for service resolution.
	/// </summary>
	/// <typeparam name="TServiceType">The service type.</typeparam>
	/// <typeparam name="TServiceEnumType">The service type enumeration.</typeparam>
	public sealed class AuthorizedServiceDiscoveryServiceResolver<TServiceType, TServiceEnumType> : DefaultServiceDiscoveryServiceResolver<TServiceType, TServiceEnumType>
		where TServiceType : class
		where TServiceEnumType : Enum
	{
		private IReadonlyAuthTokenRepository TokenRepository { get; }

		public AuthorizedServiceDiscoveryServiceResolver(IServiceDiscoveryService discoveryClient,
			TServiceEnumType serviceType,
			IReadonlyAuthTokenRepository tokenRepository)
			: base(discoveryClient, serviceType)
		{
			TokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
		}

		protected override HttpMessageHandler BuildHttpClientHandler()
		{
			return new AuthenticatedHttpClientHandler(TokenRepository, new BypassHttpsValidationHandler());
		}
	}
}