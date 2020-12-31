using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Glader.Essentials;

namespace Glader.ASP.ServiceDiscovery
{
	public interface IServiceEndpointRepository : IGenericRepositoryCrudable<string, ServiceEndpointModel>
	{
		/// <summary>
		/// Retrieves all <see cref="ServiceEndpointModel"/>s based on the provided ServiceType key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<ServiceEndpointModel[]> RetrieveAllAsync(string key, CancellationToken token = default);
	}
}
