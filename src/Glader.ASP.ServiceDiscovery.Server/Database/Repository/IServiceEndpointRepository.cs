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

		/// <summary>
		/// Indicates if an <see cref="ServiceEndpointModel"/> exists with the name <see cref="key"/> within the group name <see cref="groupName"/>.
		/// </summary>
		/// <param name="key">The service key/name.</param>
		/// <param name="groupName">The group name.</param>
		/// <param name="token">The cancel token.</param>
		/// <returns>True if an entry exists.</returns>
		Task<bool> ContainsAsync(string key, string groupName, CancellationToken token = default);

		/// <summary>
		/// Retrieves a <see cref="ServiceEndpointModel"/> with the name <see cref="key"/> within the group name <see cref="groupName"/>.
		/// </summary>
		/// <param name="key">The service key/name.</param>
		/// <param name="groupName">The group name.</param>
		/// <param name="token">The cancel token.</param>
		/// <returns>Service endpoint.</returns>
		Task<ServiceEndpointModel> RetrieveAsync(string key, string groupName, CancellationToken token = default);

		/// <summary>
		/// Checks if a group name even exists in the service listings.
		/// </summary>
		/// <param name="groupName">The group name.</param>
		/// <param name="token">Cancel token.</param>
		/// <returns>True if an entry with the provided groupName exists.</returns>
		Task<bool> ContainsGroupAsync(string groupName, CancellationToken token = default);

		/// <summary>
		/// Retrieves all <see cref="ServiceEndpointModel"/>s based on the provided <see cref="groupName"/> key.
		/// </summary>
		/// <param name="groupName">The group name.</param>
		/// <param name="token">Cancel token.</param>
		/// <returns>Retrieves all the entries with the provided <see cref="groupName"/>.</returns>
		Task<ServiceEndpointModel[]> RetrieveAllGroupedAsync(string groupName, CancellationToken token = default);
	}
}
