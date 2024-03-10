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
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
                comment = stock.Comments.Select(c => c.ToCommentDto()).ToList()
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
                Industry = newstock.Industry,   
                MarketCap = newstock.MarketCap
            };
        }

        public static Stock ToStockFromFMPStock(this FMPStock fmpStock)
        {
            return new Stock
            {
                Symbol = fmpStock.symbol,
                CompanyName = fmpStock.companyName,
                Purchase =(decimal) fmpStock.price,
                LastDiv =(decimal) fmpStock.lastDiv,
                Industry = fmpStock.industry,
                MarketCap = fmpStock.mktCap
            };
        }
    }
}
