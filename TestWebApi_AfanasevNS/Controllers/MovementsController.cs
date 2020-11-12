using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebApi_AfanasevNS.Models;

namespace TestWebApi_AfanasevNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovementsController : ControllerBase
    {

        ProdContext db;

        public MovementsController(ProdContext context)
        {
            db = context;
            if (!db.Movements.Any())
            {
                db.Movements.Add(new ProductMovements { ProductId = 1, Quantity = 10 });
                db.Movements.Add(new ProductMovements { ProductId = 2, Quantity = 100 });
                db.Movements.Add(new ProductMovements { ProductId = 3, Quantity = 145 });
                db.Movements.Add(new ProductMovements { ProductId = 4, Quantity = 134 });
                db.Movements.Add(new ProductMovements { ProductId = 1, Quantity = -2 });
                db.SaveChanges();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<double>> GetSum(int id)
        {
            var sum = db.Movements.Where(p => p.ProductId == id);
            return await sum.SumAsync(p=>p.Quantity);
        }

        [HttpPost]
        public async Task<ActionResult<ProductMovements>> Post(ProductMovements movements)
        {
            if (movements == null)
            {
                return BadRequest();
            }

            db.Movements.Add(movements);
            await db.SaveChangesAsync();
            return Ok(movements);
        }
    }
}
