using Kata_DAL.InMemoryRepositories;
using Kata_DAL.IRepositories;
using Kata_Services.Commands.AddUser;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SolidToken.SpecFlow.DependencyInjection;

namespace BechavioralTestsSpecFlow.Dependencies;

public static class TestDependencies
{
    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IUserRepository, UserInMemoryRepository>();
        services.AddSingleton<IMessageRepository, MessageInMemoryRepository>();
        services.AddMediatR(typeof(AddUserCommand).Assembly);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}