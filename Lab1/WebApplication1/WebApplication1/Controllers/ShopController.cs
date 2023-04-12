using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ShopContext _context;

        public ShopController(ShopContext context)
        {
            this._context = context;
        }

        // Read
        [HttpGet]
        public async Task<ActionResult<List<Shop>>> Get()
        {
            return Ok(await _context.Shop.ToListAsync());
        }

        // Read one
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> Get(int id)
        {
            var shop = await _context.Shop.FindAsync(id);
            if (shop == null) { 
                return BadRequest(404);
            }
            else
            {
                return Ok(shop);
            }
        }

        // Add
        [HttpPost]
        public async Task<ActionResult<List<Shop>>> Add(Shop shop)
        {   
            _context.Add(shop);
            await _context.SaveChangesAsync();
            return Ok(await _context.Shop.ToListAsync());
        }

        // Update
        [HttpPut]
        public async Task<ActionResult<List<Shop>>> Update(Shop request)
        {
            var shop = await _context.Shop.FindAsync(request.Id);
            if (shop == null)
            {
                return BadRequest(404);
            }
            else
            {
                shop.Name = request.Name;
                shop.CountProduct = request.CountProduct;
                shop.PriceOfOne = request.PriceOfOne;
                shop.DateOfSaving = request.DateOfSaving;

                await _context.SaveChangesAsync();

                return Ok(shop);
            }
        }

        //Таблиця “Міні-ABC” (назва товару; кількість товару в одиницях
        //; ціна за одиницю товару; термін зберігання (в днях)

        [HttpDelete]
        public async Task<ActionResult<List<Shop>>> Delete(int id)
        {
            var shop = await _context.Shop.FindAsync(id);
            if (shop == null)
            {
                return BadRequest(404);
            }
            else
            {
                _context.Shop.Remove(shop);

                await _context.SaveChangesAsync();

                return Ok(shop);
            }
        }
    }
}
