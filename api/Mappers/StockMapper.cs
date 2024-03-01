using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    //Extension methods
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock stock)
        {
            return new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                MarketCap = stock.MarketCap
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockDto newstock)
        {
            return new Stock
            {
                Symbol = newstock.Symbol,
                CompanyName = newstock.CompanyName,
                Purchase = newstock.Purchase,
                LastDiv = newstock.LastDiv,
                MarketCap = newstock.MarketCap
            };
        }
    }
}
