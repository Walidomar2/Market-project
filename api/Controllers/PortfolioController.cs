using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository; 
        private readonly IPortfolioRepository _portfolioRepo;

        public PortfolioController(UserManager<AppUser> userManager, 
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepo
            )
        {
            _userManager = userManager; 
            _stockRepository = stockRepository;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if (stock == null) return BadRequest("Stock Not Found");

            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);

            if (userPortfolio.Any(p => p.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Can't add same stock to portfolio");


            var portfolio = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            await _portfolioRepo.CreatePortfolioAsync(portfolio);

            if (portfolio == null)
                return StatusCode(500, "Couldn't Created");

            return Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);

            var portfolios = await _portfolioRepo.GetUserPortfolioAsync(appUser);
            var userPortfolio = portfolios.Where(p => p.Symbol.ToLower() == symbol.ToLower()).ToList();
            
            if(userPortfolio.Count == 1)
            {
                await _portfolioRepo.DeletePortfolioAsync(appUser, symbol);
            }
            else
            {
                return BadRequest("Portfolio Not Exists");
            }

            return Ok();
        }
    }
}