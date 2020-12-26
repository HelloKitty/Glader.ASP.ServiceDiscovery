using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Default EF Core implementation of <see cref="IServiceEndpointRepository"/>.
	/// </summary>
	public sealed class DefaultServiceEndpointRepository : IServiceEndpointRepository
	{
		/// <summary>
		/// The service discovery <see cref="DbContext"/>.
		/// </summary>
		private ServiceDiscoveryDatabaseContext Context { get; }

		public DefaultServiceEndpointRepository(ServiceDiscoveryDatabaseContext context)
		{
			Context = context ?? throw new ArgumentNullException(nameof(context));
		}

		/// <inheritdoc />
		public async Task<bool> ContainsAsync(string key, CancellationToken token = default)
		{
			key = ServiceNameNormalizedBuilder
				.Create(key)
				.ToString();

			//Check if any match the name.
			return await Context.ServiceEndpoints.AnyAsync(e => e.Service.ServiceName == key, token);
		}

		/// <inheritdoc />
		public async Task<bool> TryCreateAsync(ServiceEndpointModel model, CancellationToken token = default)
		{
			//TODO: Implement when not lazy
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public async Task<ServiceEndpointModel> RetrieveAsync(string key, CancellationToken token = default, bool includeNavigationProperties = false)
		{
			return await Context.ServiceEndpoints.FirstAsync(e => e.Service.ServiceName == key, token);
		}

		/// <inheritdoc />
		public async Task<bool> TryDeleteAsync(string key, CancellationToken token = default)
		{
			//TODO: Implement when not lazy
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public async Task UpdateAsync(string key, ServiceEndpointModel model, CancellationToken token = default)
		{
			//TODO: Implement when not lazy
			throw new NotImplementedException();
		}
	}
}
