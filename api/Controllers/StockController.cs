using Microsoft.AspNetCore.Mvc;
using System;
using api.Data;
using api.Mappers;
using api.Dtos.Stock;
using System.Reflection.Metadata.Ecma335;


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
        public IActionResult Get() 
        {
            var stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());
            return Ok(stocks); //return after  successfully operation from HTTP
        }

        // Getting one stock by id 
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) 
        {
            var stock = _context.Stocks.Find(id);

            if(stock == null)
            {
                return NotFound();
            }
            
            return Ok(stock);
        }

        // POSTing Method
        [HttpPost]
        public IActionResult Create([FromBody] CreateStockDto newstock)
        {
            var CreatedStock = newstock.ToStockFromCreateDto();
            _context.Stocks.Add(CreatedStock);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById),new {id = CreatedStock.Id}, CreatedStock.ToStockDto());    
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id,[FromBody] UpdateStockDto UpdatedDto)
        {
            var stock = _context.Stocks.FirstOrDefault(x => x.Id == id);

            if(stock == null)
            {
                return NotFound();
            }

            stock.Symbol = UpdatedDto.Symbol;
            stock.CompanyName = UpdatedDto.CompanyName;
            stock.Purchase = UpdatedDto.Purchase;
            stock.LastDiv = UpdatedDto.LastDiv;
            stock.MarketCap = UpdatedDto.MarketCap;
            
            _context.SaveChanges();
            return Ok(stock.ToStockDto());
        }
    }
}
