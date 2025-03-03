﻿using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Accounts;
using WSantosDev.EventSourcing.Exchange;
using WSantosDev.EventSourcing.Orders;
using WSantosDev.EventSourcing.Positions;

namespace WSantosDev.EventSourcing.WebApi
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<EventDbContext>(options => options.UseSqlite(configuration["ConnectionStrings:EventStore"]), 
                                                                    ServiceLifetime.Singleton)
                           .AddSingleton<EventStore>()
                           .AddDbContext<AccountReadModelDbContext>(options => 
                                        options.UseSqlite(configuration["ConnectionStrings:ReadModelStore"]),
                                        ServiceLifetime.Singleton)
                           .AddDbContext<PositionReadModelDbContext>(options => 
                                        options.UseSqlite(configuration["ConnectionStrings:ReadModelStore"]), 
                                        ServiceLifetime.Singleton)
                           .AddDbContext<ExchangeOrderReadModelDbContext>(options => 
                                        options.UseSqlite(configuration["ConnectionStrings:ReadModelStore"]),
                                        ServiceLifetime.Singleton)
                           .AddDbContext<OrderReadModelDbContext>(options =>
                                        options.UseSqlite(configuration["ConnectionStrings:ReadModelStore"]),
                                        ServiceLifetime.Singleton);
        }
    }
}
