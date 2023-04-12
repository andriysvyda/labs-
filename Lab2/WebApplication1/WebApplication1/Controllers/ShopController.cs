using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

using System.Linq.Expressions;
using System;

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
            if (shop == null)
            {
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        [HttpGet("orderBy")]
        public async Task<ActionResult<List<Shop>>> Get(string order, string column)
        {
            if (order == "asc")
            {

                var shops = await _context.Shop.OrderBy(
                    shop => shop.GetType().GetProperty(column).GetValue(shop)
                    //employee => employee.Name
                ).ToListAsync();
                return Ok(shops);
            }
            else
            {
                var shops = await _context.Shop.OrderByDescending(
                    //employee => typeof(Employee).GetProperty(column).GetValue(employee)
                    shop => shop.Name
                ).ToListAsync();
                return Ok(shops);
            }
        }

        [HttpGet("filterBy")]
        public async Task<ActionResult<List<Shop>>> Get(string name)
        {
            var shops = await _context.Shop.Where(
                shop => shop.Name == name
            ).ToListAsync();
            return Ok(shops);
        }

        [HttpGet("pages")]
        public async Task<ActionResult<List<Shop>>> Get(float pageItems, int page)
        {
            //var pageItems = 2f;
            var pageCount = Math.Ceiling(_context.Shop.Count() / pageItems);

            var employees = await _context.Shop
                .Skip((page - 1) * (int)pageItems)
                .Take((int)pageItems)
                .ToListAsync();

            return Ok(employees);
        }
    }
}    