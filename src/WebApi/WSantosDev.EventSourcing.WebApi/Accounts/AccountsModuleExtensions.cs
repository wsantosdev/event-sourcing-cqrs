using WSantosDev.EventSourcing.Accounts;
using WSantosDev.EventSourcing.Accounts.Actions;
using WSantosDev.EventSourcing.Accounts.DomainEvents;
using WSantosDev.EventSourcing.Accounts.Queries;
using WSantosDev.EventSourcing.WebApi.Accounts.ExternalEvents;

namespace WSantosDev.EventSourcing.WebApi.Accounts
{
    public static class AccountsModuleExtensions
    {
        public static IServiceCollection AddAccountsModule(this IServiceCollection services)
        {
            return services.AddSingleton<IAccountStore, AccountStore>()
                           .AddSingleton<IAccountReadModelStore, AccountReadModelStore>()
                           .AddTransient<OpenAction>()
                           .AddTransient<CreditAction>()
                           .AddSingleton<DebitAction>()
                           .AddSingleton<AccountQuery>()
                           .AddSingleton<AccountOpenedHandler>()
                           .AddSingleton<AccountUpdatedHandler>()
                           .AddSingleton<OrderPlacedHandler>()
                           .AddSingleton<ExchangeExecutedHandler>();
        }
    }
}
