

using Microsoft.EntityFrameworkCore;
using UsuariosApp.Api.Contexts;

namespace UsuariosApp.API.Extensions
{
    public static class EntityFrameworkExtension
    {
        public static IServiceCollection AddEntityFrameworkConfig
            (this IServiceCollection services, IConfiguration configuration)
        {
            //capturando a string de conexão do /appsettings
            var connectionString = configuration.GetConnectionString("UsuariosApp");

            //injeção de dependência do contexto do EntityFramework
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}



