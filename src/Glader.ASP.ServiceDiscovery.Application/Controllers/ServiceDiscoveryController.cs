using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glader.ASP.ServiceDiscovery;
using Glader.Essentials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GladMMO
{
	/// <summary>
	/// The service discovery controller.
	/// Endpoints/actions are documented in the documentation repo.
	/// </summary>
	[Route("api/[controller]")]
	public sealed class ServiceDiscoveryController : Controller
	{
		/// <summary>
		/// The endpoint repository.
		/// </summary>
		private IServiceEndpointRepository EndpointRepository { get; }

		public ILogger<ServiceDiscoveryController> LoggingService { get; }

		public ServiceDiscoveryController([FromServices] IServiceEndpointRepository endpointRepository, ILogger<ServiceDiscoveryController> loggingService)
		{
			if (endpointRepository == null) throw new ArgumentNullException(nameof(endpointRepository));
			if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

			EndpointRepository = endpointRepository;
			LoggingService = loggingService;
		}

		[HttpGet(nameof(Discover))]
		public async Task<JsonResult> Discover([FromQuery] string serviceName)
		{
			if(LoggingService.IsEnabled(LogLevel.Debug))
				LoggingService.LogDebug($"Service Discover request for: {serviceName}");

			if(!ModelState.IsValid)
			{
				if(LoggingService.IsEnabled(LogLevel.Debug))
					LoggingService.LogDebug($"Resolution request was sent with an invalid model ModelState.");

				Json(new ResponseModel<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>(ResolvedServiceEndpointResponseCode.GeneralRequestError));
			}

			//We need to check if we know about the locale
			//If we don't we should indicate it is unlisted
			//We also need to check if the keypair region and servicetype exist
			//TODO: Deployment mode handling, right now it's set to INTERNAL
			if(!await EndpointRepository.ContainsAsync(serviceName))
			{
				if(LoggingService.IsEnabled(LogLevel.Debug))
					LoggingService.LogDebug($"Client requested unlisted service Service: {serviceName}.");

				return Json(new ResponseModel<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>(ResolvedServiceEndpointResponseCode.ServiceUnlisted));
			}

			//Assume no failure
			ServiceEndpointModel endpoint = await EndpointRepository.RetrieveAsync(serviceName);

			return Json(new ResponseModel<ResolvedEndpoint, ResolvedServiceEndpointResponseCode>(endpoint.Endpoint));
		}

		[HttpGet(nameof(Discover))]
		public async Task<JsonResult> Discover([FromBody] ResolvedServiceEndpointRequest request)
		{
			if(LoggingService.IsEnabled(LogLevel.Debug))
				LoggingService.LogDebug($"Service Discover request for: {request.ServiceType}");

			return await Discover(request.ServiceType);
		}
	}
}
