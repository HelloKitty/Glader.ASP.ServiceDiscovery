using System;
using System.ComponentModel;
using Autofac;
using Glader.Essentials;

namespace Glader.ASP.ServiceDiscovery
{
	/// <summary>
	/// Service module that registers a <see cref="IServiceResolver{TServiceInterfaceType}"/> for the specified
	/// <typeparamref name="TServiceType"/>. Using <see cref="IServiceDiscoveryService"/> to resolve them.
	/// </summary>
	/// <typeparam name="TServiceType">The service type.</typeparam>
	/// <typeparam name="TServiceEnumType">The service type enumeration.</typeparam>
	public sealed class ServiceDiscoverableServiceModule<TServiceType, TServiceEnumType> : Module
		where TServiceType : class
		where TServiceEnumType : Enum
	{
		/// <summary>
		/// The service type mapping.
		/// </summary>
		private TServiceEnumType ServiceType { get; }

		/// <summary>
		/// The default service mode.
		/// </summary>
		public ServiceDiscoveryModuleMode Mode { get; init; } = ServiceDiscoveryModuleMode.Default;

		/// <summary>
		/// The service URL endpoint factory.
		/// Default: <see cref="DefaultServiceBaseUrlFactory"/>.
		/// </summary>
		public IServiceBaseUrlFactory UrlFactory { get; init; } = new DefaultServiceBaseUrlFactory();

		/// <summary>
		/// Indicates if the service should be globally (singleton) registered.
		/// </summary>
		public bool GlobalScope { get; init; } = false;

		/// <summary>
		/// Optional key.
		/// </summary>
		public object Key { get; init; } = null;

		/// <summary>
		/// Service that converts the service-type enum to the service name string.
		/// Default: <see cref="DefaultServiceTypeIdentifierConverter{TServiceTypeEnum}"/>
		/// </summary>
		public IServiceTypeIdentifierConverter<TServiceEnumType> ServiceNameConverter { get; init; } = new DefaultServiceTypeIdentifierConverter<TServiceEnumType>();

		public ServiceDiscoverableServiceModule(TServiceEnumType serviceType)
		{
			ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
		}

		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			if(Mode == ServiceDiscoveryModuleMode.Authorized)
			{
				//I know this seems strange, but having the service provider be unique to the
				//lifetimescope means we can inject Auth tokens into it and not worry about sharing this resource.
				//It also means if a service dies or gets removed, reconnecting will yield another service on connection.
				var reg = builder.Register<AuthorizedServiceDiscoveryServiceResolver<TServiceType, TServiceEnumType>>(context =>
				{
					return new AuthorizedServiceDiscoveryServiceResolver<TServiceType, TServiceEnumType>(context.Resolve<IServiceDiscoveryService>(), ServiceType, context.Resolve<IReadonlyAuthTokenRepository>())
					{
						ServiceNameConverter = ServiceNameConverter,
						UrlFactory = UrlFactory
					};
				})
					.As<IServiceResolver<TServiceType>>();

				reg = GlobalScope ? reg.SingleInstance() : reg.InstancePerLifetimeScope();

				if(Key != null)
					reg.Keyed<IServiceResolver<TServiceType>>(Key);
			}
			else
			{
				var reg = builder.Register<DefaultServiceDiscoveryServiceResolver<TServiceType, TServiceEnumType>>(context =>
				{
					return new DefaultServiceDiscoveryServiceResolver<TServiceType, TServiceEnumType>(context.Resolve<IServiceDiscoveryService>(), ServiceType)
					{
						ServiceNameConverter = ServiceNameConverter,
						UrlFactory = UrlFactory
					};
				})
					.As<IServiceResolver<TServiceType>>();

				reg = GlobalScope ? reg.SingleInstance() : reg.InstancePerLifetimeScope();

				if(Key != null)
					reg.Keyed<IServiceResolver<TServiceType>>(Key);
			}
		}
	}
}
