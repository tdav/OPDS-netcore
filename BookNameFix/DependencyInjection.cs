using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookNameFix
{
    public static class DependencyInjection
    {
        public static void AddProgramService(this IServiceCollection services, IConfiguration conf)
        {
            //var connStr = conf.GetConnectionString("DefaultConnection");
            //services.AddDbContext<MyDbContext>(o => { o.UseNpgsql(connStr); });
            services.AddScoped<Program>();
        }
    }
}
