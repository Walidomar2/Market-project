using Microsoft.AspNetCore.Mvc;
using System;
using api.Data;
using api.Mappers;
using api.Dtos.Stock;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;


namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStockRepository _stockRepository;

        public StockController(ApplicationDbContext context, IStockRepository StockRepo)
        {
            _stockRepository = StockRepo;   
            _context = context;
        }

        // Getting all Stocks list
        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            var stocks = await _stockRepository.GetAllAsync();
            var stocksDto = stocks.Select(s => s.ToStockDto());
            return Ok(stocksDto); //return after  successfully operation from HTTP
        }

        // Getting one stock by id 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) 
        {
            var stock = await _stockRepository.GetByIdAsync(id);

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
            await _stockRepository.CreateAsync(CreatedStock);

            return CreatedAtAction(nameof(GetById),new {id = CreatedStock.Id}, CreatedStock.ToStockDto());    
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateStockDto UpdatedDto)
        {
            var stock = await _stockRepository.UpdateAsync(id, UpdatedDto);

            if(stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _stockRepository.DeleteAsync(id);

            if (stock == null)
            {
                return NotFound();  
            }

            return NoContent();  
        }


    }
}
