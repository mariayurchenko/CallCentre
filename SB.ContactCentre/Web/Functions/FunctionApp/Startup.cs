using FunctionApp;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SB.WebShared.DynamicsAuthentication;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.Services.BuildServiceProvider().GetService<IConfiguration>();

            var crmOptions = new CrmClientOptions().BindStringMembersFromConnectionString(config["ClientOptions"]) as CrmClientOptions;

            builder.Services
                .AddSingleton(crmOptions)
                .AddTransient<IAuthenticationService, AuthenticationService>();
        }
    }
}
