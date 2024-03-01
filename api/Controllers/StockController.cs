using Microsoft.AspNetCore.Mvc;
using System;
using api.Data;
using api.Mappers;
using api.Dtos.Stock;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;


namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Getting all Stocks list
        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stocksDto = stocks.Select(s => s.ToStockDto());
            return Ok(stocks); //return after  successfully operation from HTTP
        }

        // Getting one stock by id 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) 
        {
            var stock = await _context.Stocks.FindAsync(id);

            if(stock == null)
            {
                return NotFound();
            }
            
            return Ok(stock.ToStockDto());
        }

        // POSTing Method
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDto newstock)
        {
            var CreatedStock = newstock.ToStockFromCreateDto();
            await _context.Stocks.AddAsync(CreatedStock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),new {id = CreatedStock.Id}, CreatedStock.ToStockDto());    
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateStockDto UpdatedDto)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(stock == null)
            {
                return NotFound();
            }

            stock.Symbol = UpdatedDto.Symbol;
            stock.CompanyName = UpdatedDto.CompanyName;
            stock.Purchase = UpdatedDto.Purchase;
            stock.LastDiv = UpdatedDto.LastDiv;
            stock.MarketCap = UpdatedDto.MarketCap;
            
            await _context.SaveChangesAsync();
            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (stock == null)
            {
                return NotFound();  
            }

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return NoContent();  
        }


    }
}
