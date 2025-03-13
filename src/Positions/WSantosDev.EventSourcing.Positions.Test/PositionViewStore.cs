using Microsoft.EntityFrameworkCore;

namespace WSantosDev.EventSourcing.Positions
{
    public class PositionViewStore(PositionViewDbContext dbContext)
    {
        public async Task StoreAsync(PositionView position)
        {
            var stored = await dbContext.BySymbolAsync(position.AccountId, position.Symbol);
            if (stored)
            {
                dbContext.Entry(stored.Get()).State = EntityState.Detached;
                dbContext.Entry(position).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return;
            }

            dbContext.Add(position);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(PositionView position)
        {
            var sql = $"DELETE FROM Positions WHERE AccountId = '{position.AccountId}' AND Symbol = '{position.Symbol}'";
            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }
    }

    
}
