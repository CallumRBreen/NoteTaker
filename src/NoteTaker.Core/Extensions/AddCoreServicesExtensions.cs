using Microsoft.Extensions.DependencyInjection;
using NoteTaker.Core.Services.Implementations;
using NoteTaker.Core.Services.Interfaces;

namespace NoteTaker.Core.Extensions
{
    public static class AddCoreServicesExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<IPasswordHashingService, BCryptPasswordHashingService>()
                                    .AddTransient<ITokenService, JwtTokenService>()
                                    .AddTransient<INotesService, NotesService>()
                                    .AddTransient<IUsersService, UsersService>();
        }
    }
}
