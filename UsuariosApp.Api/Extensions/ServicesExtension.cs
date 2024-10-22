using UsuariosApp.API.Services;

namespace UsuariosApp.API.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServicesConfig(this IServiceCollection services)
        {
            services.AddTransient<UsuarioService>();

            return services;
        }
    }
}

