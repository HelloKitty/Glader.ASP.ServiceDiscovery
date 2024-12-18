﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Glader.ASP.ServiceDiscovery;
using Glader.Essentials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// The service discovery controller.
	/// Endpoints/actions are documented in the documentation repo.
	/// </summary>
	[Route("api/[controller]")]
	public sealed class ServiceDiscoveryController : BaseGladerController, IServiceDiscoveryService
	{
		/// <summary>
		/// The endpoint repository.
		/// </summary>
		private IServiceEndpointRepository EndpointRepository { get; }

		public ILogger<ServiceDiscoveryController> LoggingService { get; }

		public ServiceDiscoveryController([FromServices] IServiceEndpointRepository endpointRepository, ILogger<ServiceDiscoveryController> loggingService)
			: base(loggingService)
		{
			EndpointRepository = endpointRepository ?? throw new ArgumentNullException(nameof(endpointRepository));
			LoggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
		}

		[ProducesJson]
		[HttpGet("{name}/Single/{group}")]
		public async Task<ResponseModel<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>> DiscoverServiceAsync([FromRoute(Name = "name")] string serviceType, 
			[FromRoute(Name = "group")] string groupName, CancellationToken token = default)
		{
			return await RetrieveServiceEndpointResponseAsync(serviceType, groupName, token);
		}

		[ProducesJson]
		[HttpGet("{name}/Single")]
		public async Task<ResponseModel<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>> DiscoverServiceAsync([FromRoute(Name = "name")] string serviceType, CancellationToken token = default)
		{
			return await RetrieveServiceEndpointResponseAsync(serviceType, ServiceDiscoveryConstants.DEFAULT_GROUP_NAME, token);
		}

		private async Task<ResponseModel<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>> 
			RetrieveServiceEndpointResponseAsync(string serviceType, string groupName, CancellationToken token)
		{
			if (LoggingService.IsEnabled(LogLevel.Debug))
				LoggingService.LogDebug($"Service Discovery request for Service: {serviceType} Group: {groupName}");

			if (!ModelState.IsValid)
			{
				if (LoggingService.IsEnabled(LogLevel.Debug))
					LoggingService.LogDebug($"Resolution request was sent with an invalid model ModelState.");

				return Failure<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>(ResolvedServiceEndpointResponseCode.GeneralRequestError);
			}

			//We need to check if we know about the locale
			//If we don't we should indicate it is unlisted
			//We also need to check if the keypair region and servicetype exist
			if (!await EndpointRepository.ContainsAsync(serviceType, groupName, token))
			{
				if (LoggingService.IsEnabled(LogLevel.Debug))
					LoggingService.LogDebug($"Client requested unlisted service Service: {serviceType} Group: {groupName}.");

				return Failure<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>(ResolvedServiceEndpointResponseCode.ServiceUnlisted);
			}

			//Assume no failure
			ServiceEndpointModel endpoint = await EndpointRepository.RetrieveAsync(serviceType, groupName, token);

			return Success<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>(endpoint.Endpoint);
		}

		[ProducesJson]
		[HttpGet("{name}/All")]
		public async Task<ResponseModel<ServiceResolutionResult, ResolvedServiceEndpointResponseCode>> DiscoverServicesAsync([FromRoute(Name = "name")] string serviceType, CancellationToken token = default)
		{
			if(LoggingService.IsEnabled(LogLevel.Debug))
				LoggingService.LogDebug($"Service Discover request for: {serviceType}");

			if(!ModelState.IsValid)
			{
				if(LoggingService.IsEnabled(LogLevel.Debug))
					LoggingService.LogDebug($"Resolution request was sent with an invalid model ModelState.");

				return Failure<ServiceResolutionResult, ResolvedServiceEndpointResponseCode>(ResolvedServiceEndpointResponseCode.GeneralRequestError);
			}

			//We need to check if we know about the locale
			//If we don't we should indicate it is unlisted
			//We also need to check if the keypair region and servicetype exist
			//TODO: Deployment mode handling, right now it's set to INTERNAL
			if(!await EndpointRepository.ContainsAsync(serviceType, token))
			{
				if(LoggingService.IsEnabled(LogLevel.Debug))
					LoggingService.LogDebug($"Client requested unlisted service Service: {serviceType}.");

				return Failure<ServiceResolutionResult, ResolvedServiceEndpointResponseCode>(ResolvedServiceEndpointResponseCode.ServiceUnlisted);
			}

			//Assume no failure
			ResolvedService[] endpoints = (await EndpointRepository
					.RetrieveAllAsync(serviceType, token))
				.Select(m => new ResolvedService(m.Name, m.Endpoint))
				.ToArray();

			return Success<ServiceResolutionResult, ResolvedServiceEndpointResponseCode>(new ServiceResolutionResult(endpoints));
		}

		// [Get("/api/ServiceDiscovery/Group/{groupName}/All")]
		[ProducesJson]
		[HttpGet("Group/{groupName}/All")]
		public async Task<ResponseModel<Dictionary<string, ResolvedEndpoint>, ResolvedServiceEndpointResponseCode>> 
			DiscoverGroupServicesAsync([FromRoute(Name = "groupName")] string groupName, CancellationToken token = default)
		{
			if(LoggingService.IsEnabled(LogLevel.Debug))
				LoggingService.LogDebug($"Full Service Discover request for GroupName: {groupName}");

			if (!ModelState.IsValid)
			{
				if(LoggingService.IsEnabled(LogLevel.Debug))
					LoggingService.LogDebug($"Full GroupName resolution request was sent with an invalid model ModelState.");

				return Failure<Dictionary<string, ResolvedEndpoint>, ResolvedServiceEndpointResponseCode>(ResolvedServiceEndpointResponseCode.GeneralRequestError);
			}

			//We need to check if we know about the locale
			//If we don't we should indicate it is unlisted
			//We also need to check if the keypair region and servicetype exist
			//TODO: Deployment mode handling, right now it's set to INTERNAL
			if (!await EndpointRepository.ContainsGroupAsync(groupName, token))
			{
				if(LoggingService.IsEnabled(LogLevel.Debug))
					LoggingService.LogDebug($"Client requested unlisted GroupName: {groupName}.");

				return Failure<Dictionary<string, ResolvedEndpoint>, ResolvedServiceEndpointResponseCode>(ResolvedServiceEndpointResponseCode.GroupNameServiceUnlisted);
			}

			ServiceEndpointModel[] entries = await EndpointRepository
				.RetrieveAllGroupedAsync(groupName, token);

			//Assume no failure
			var endpoints = entries
				.ToDictionary(model => model.Service.ServiceType, model => model.Endpoint);

			return Success<Dictionary<string, ResolvedEndpoint>, ResolvedServiceEndpointResponseCode>(endpoints);
		}
	}
}
