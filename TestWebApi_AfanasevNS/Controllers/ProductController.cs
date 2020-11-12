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
    public class ProductController : ControllerBase
    {
        ProdContext db;

        public ProductController(ProdContext context)
        {
            db = context;
            if (!db.Prod.Any())
            {
                db.Prod.Add(new Product { Title = "Бананы", UomId = 3 });
                db.Prod.Add(new Product { Title = "Специи", UomId = 2 });
                db.Prod.Add(new Product { Title = "Мандарины", UomId = 3 });
                db.Prod.Add(new Product { Title = "Авокадо", UomId = 1 });

                db.SaveChanges();
            }
        }

        [HttpGet]
        [HttpGet("{page}")]
        [HttpGet("{page}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<ProductAll>>> Get(int page = 1, int pageSize = 2)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest();
            }

            var productAll = db.Prod.Join(db.ProductUoms, x => x.UomId, p => p.Id, (x, p) => 
            new ProductAll {Id = x.Id, Title = x.Title, Uom = p.Title })
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            return await productAll;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            db.Prod.Add(product);
            await db.SaveChangesAsync();
            return Ok(product);
        }
    }
}
