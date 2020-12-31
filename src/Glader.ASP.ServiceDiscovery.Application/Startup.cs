using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glader.ASP.ServiceDiscovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GladMMO
{
	public class Startup
	{
		//Changed in ASP Core 2.0
		public Startup(IConfiguration config)
		{
			if(config == null) throw new ArgumentNullException(nameof(config));

			Configuration = config;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddMvc(options =>
				{
					options.EnableEndpointRouting = false;
				})
				.RegisterServiceDiscoveryController()
				.AddNewtonsoftJson();

			services.AddLogging();

			//DefaultServiceEndpointRepository : IServiceEndpointRepository
			services.RegisterServiceDiscoveryDatabase(builder =>
			{
				builder.UseMySql("server=127.0.0.1;port=3306;Database=glader.test;Uid=root;Pwd=test;", optionsBuilder =>
				{
					optionsBuilder.MigrationsAssembly(GetType().Assembly.FullName);
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
#warning Do not deploy exceptions page into production
			app.UseDeveloperExceptionPage();

			app.UseMvcWithDefaultRoute();
		}
	}
}
