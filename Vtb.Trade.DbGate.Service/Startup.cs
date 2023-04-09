using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using Vtb.Trade.Configuration.Client;
using Vtb.Trade.Configuration.Common;
using Vtb.Trade.DbGate.Common;
using Vtb.Trade.Identity.Client;

namespace Vtb.Trade.DbGate.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddGrpc(opt => Configuration.Bind("grpc", opt));
            services.Configure<RouteEndpoints>(opt => opt.Route(Configuration));
            services.Configure<RepositoryOptions>(opt =>
            {
                opt.ConnectionString = Configuration.GetConnectionString("OLB");
            });

            services.AddSingleton<ChannelProvider>();
            services.AddSingleton<IdentityClient>();
            services.AddTransient<IdentityPushConnector>();

            services.AddSingleton<IdentityGetter>(serviceProvider =>
            {
                IdentityGetter identityGetter = serviceProvider.GetService<IdentityPushConnector>();
                // регистрация для дефолтного получения текста из идентити
                identityGetter.Create<Grpc.Common.E>(out _);
                return identityGetter;
            });

            services.AddScoped(
                services => new RequestService(
                    name => GetRequestOptions(name),
                    services.GetService<IOptions<RepositoryOptions>>(),
                    services.GetService<IdentityGetter>(),
                    services.GetService<ILoggerFactory>())
                );
        }

        private RequestOptions GetRequestOptions(string name)
        {
            var options = new RequestOptions();
            Configuration.Bind(name, options);
            return options;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AccountService>();
                endpoints.MapGrpcService<AgreementService>();
                endpoints.MapGrpcService<SecuritySnapshotAddService>();
                endpoints.MapGrpcService<SecuritySnapshotService>();
                endpoints.MapGrpcService<AgreementAddOrUpdService>();
                endpoints.MapGrpcService<SecuritySnapshotService>();                
                endpoints.MapGrpcService<AccountAddOrUpdService>();
                endpoints.MapGrpcService<RequestService>();
            });
        }
    }
}
