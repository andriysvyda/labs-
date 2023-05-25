using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2.Models;
using Lab2.Data;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarContext _context;

        public CarController(CarContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Car>>> Get()
        {
            return Ok(await _context.Car.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> Get(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return BadRequest(404);
            }
            else
            {
                return Ok(car);
            }
        }


        [HttpPost]
        public async Task<ActionResult<List<Car>>> Add(Car car)
        {
            _context.Add(car);
            await _context.SaveChangesAsync();
            return Ok(await _context.Car.ToListAsync());
        }


        [HttpPut]
        public async Task<ActionResult<List<Car>>> Update( Car request)
        {
            var car = await _context.Car.FindAsync(request.Id);
            if (car == null)
            {
                return BadRequest(404);
            }
            else
            {
                car.Owner = request.Owner;
                car.Brend = request.Brend;
                car.NumberCar = request.NumberCar;
                car.Color = request.Color;

                await _context.SaveChangesAsync();

                return Ok(car);
            }
        }


        [HttpDelete]
        public async Task<ActionResult<List<Car>>> Delete(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return BadRequest(404);
            }
            else
            {
                _context.Car.Remove(car);

                await _context.SaveChangesAsync();

                return Ok(car);
            }
        }

        [HttpGet("orderBy")]
        public async Task<ActionResult<List<Car>>> Get(string order, string column)
        {
            if (order == "asc")
            {

                var cars = await _context.Car.OrderBy(
                    car => car.GetType().GetProperty(column).GetValue(car)
                ).ToListAsync();
                return Ok(cars);
            }
            else
            {
                var cars = await _context.Car.OrderByDescending(
                    car => car.Owner
                ).ToListAsync();
                return Ok(cars);
            }
        }

        [HttpGet("filterBy")]
        public async Task<ActionResult<List<Car>>> Get(string color)
        {
            var cars = await _context.Car.Where(
                car => car.Color == color
            ).ToListAsync();
            return Ok(cars);
        }


        [HttpGet("pages")]
        public async Task<ActionResult<List<Car>>> Get(float pageItems, int page)
        {
            //var pageItems = 2f;
            var pageCount = Math.Ceiling(_context.Car.Count() / pageItems);

            var cars = await _context.Car
                .Skip((page - 1) * (int)pageItems)
                .Take((int)pageItems)
                .ToListAsync();

            return Ok(cars);
        }
    }
}
