using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Glader.Essentials;
using Nito.AsyncEx;
using Refit;

namespace Glader.ASP.ServiceDiscovery
{
	public class DefaultServiceDiscoveryServiceResolver<TServiceType, TServiceEnumType> : IServiceResolver<TServiceType>
		where TServiceType : class
		where TServiceEnumType : Enum
	{
		/// <summary>
		/// Client for service discovery.
		/// </summary>
		private IServiceDiscoveryService DiscoveryClient { get; }

		/// <summary>
		/// Indicates the service type the <typeparamref name="TServiceType"/> is mapped to.
		/// </summary>
		public TServiceEnumType ServiceType { get; }

		/// <summary>
		/// The async lock object.
		/// </summary>
		protected AsyncReaderWriterLock SyncObj { get; } = new AsyncReaderWriterLock();

		/// <summary>
		/// Represents an existing resolved service instance.
		/// (default is unavailable)
		/// </summary>
		protected ServiceResolveResult<TServiceType> ServiceInstance { get; private set; } = new ServiceResolveResult<TServiceType>(ResolvedServiceEndpointResponseCode.GeneralRequestError);

		public IServiceTypeIdentifierConverter<TServiceEnumType> ServiceNameConverter { get; init; } = new DefaultServiceTypeIdentifierConverter<TServiceEnumType>();

		public IServiceBaseUrlFactory UrlFactory { get; init; } = new DefaultServiceBaseUrlFactory();

		/// <summary>
		/// Represents the service group name
		/// (Such as: DEFAULT, LOCAL)
		/// Default value: ServiceDiscoveryConstants.DEFAULT_GROUP_NAME
		/// </summary>
		public string GroupName { get; init; } = ServiceDiscoveryConstants.DEFAULT_GROUP_NAME;

		public DefaultServiceDiscoveryServiceResolver(IServiceDiscoveryService discoveryClient,
			TServiceEnumType serviceType)
		{
			DiscoveryClient = discoveryClient ?? throw new ArgumentNullException(nameof(discoveryClient));
			ServiceType = serviceType;
		}

		public async Task<ServiceResolveResult<TServiceType>> Create(CancellationToken context)
		{
			//If not available we must resolve.
			if(!ServiceInstance.isSuccessful)
				await TryCreateService(context);

			//No matter what at this point we will return whatever instance is available
			using(await SyncObj.ReaderLockAsync(context))
				return ServiceInstance;
		}

		protected async Task<bool> TryCreateService(CancellationToken token)
		{
			using(await SyncObj.WriterLockAsync(token))
			{
				//Double check locking, may have been discovered in the middle of checking.
				if(ServiceInstance.isSuccessful)
					return true;

				//TODO: Add cancel token to service discovery
				string name = ServiceNameConverter.Create(ServiceType);
				try
				{
					var discoveryResponse = await DiscoveryClient.DiscoverServiceAsync(name, GroupName, token);

					//Failed to discover the service endpoint so service
					//is unavailable, return existing unavailable model.
					if(!discoveryResponse.isSuccessful)
						return false;

					//We have a valid endpoint!! Let's build the service
					ServiceInstance = BuildService(discoveryResponse.Result);
					return true;
				}
				catch(Exception e)
				{
					//TODO: We need to add logging

					//Return existing instance.
					return false;
				}
			}
		}

		private ServiceResolveResult<TServiceType> BuildService(ResolvedEndpoint endpoint)
		{
			if(endpoint == null) throw new ArgumentNullException(nameof(endpoint));

			TServiceType service = RestService
				.For<TServiceType>(UrlFactory.Create(endpoint), new RefitSettings()
				{
					ContentSerializer = new NewtonsoftJsonContentSerializer(),
					HttpMessageHandlerFactory = BuildHttpClientHandler
				});

			return new ServiceResolveResult<TServiceType>(service);
		}

		protected virtual HttpMessageHandler BuildHttpClientHandler()
		{
			return new BypassHttpsValidationHandler();
		}
	}
}