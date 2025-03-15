using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Accounts;
using WSantosDev.EventSourcing.Accounts.Commands;
using WSantosDev.EventSourcing.Accounts.Queries;
using WSantosDev.EventSourcing.WebApi.Accounts.DomainEvents;

namespace WSantosDev.EventSourcing.WebApi.Accounts
{
    public static class AccountsModuleExtensions
    {
        public static IServiceCollection AddAccountsModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:EventStore"]!;

            return services.AddDbContext<AccountViewDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton)
                           .AddSingleton<AccountStore>()
                           .AddTransient<Open>()
                           .AddTransient<Credit>()
                           .AddSingleton<Debit>()
                           .AddScoped<AccountById>()
                           .AddSingleton<OrderPlacedHandler>()
                           .AddSingleton<OrderExecutedHandler>();
        }
    }
}
