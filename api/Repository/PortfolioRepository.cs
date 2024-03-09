using api.Data;
using api.Interfaces;
using api.Models;
using System.Data.Entity;
using System.Linq;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;
        public PortfolioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreatePortfolioAsync(Portfolio portfolioModel)
        {
            await _context.Portfolios.AddAsync(portfolioModel);
            await _context.SaveChangesAsync();                  
            return portfolioModel;
        }

        public async Task<Portfolio> DeletePortfolioAsync(AppUser appUser, string symbol)
        {
            var portfolio = await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());
            if (portfolio == null)
                return null;

            _context.Portfolios.Remove(portfolio);  
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<List<Stock>> GetUserPortfolioAsync(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
                .Select(stock => new Stock
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap
                }).ToListAsync();   
        }
    }
}
