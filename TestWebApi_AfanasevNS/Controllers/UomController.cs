using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestWebApi_AfanasevNS.Models;
using Microsoft.EntityFrameworkCore;

namespace TestWebApi_AfanasevNS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UomController : ControllerBase
    {
        ProdContext db;

        public UomController(ProdContext context)
        {
            db = context;
            if (!db.ProductUoms.Any())
            {
                db.ProductUoms.Add(new ProductUom { Title = "шт" });
                db.ProductUoms.Add(new ProductUom { Title = "г" });
                db.ProductUoms.Add(new ProductUom { Title = "кг" });

                db.SaveChanges();
            }
        }

        [HttpGet]
        [HttpGet("{page}")]
        [HttpGet("{page}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<ProductUom>>> Get(int page = 1, int pageSize = 2)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest();
            }

            var items = await db.ProductUoms.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return items;
        }

        [HttpPost]
        public async Task<ActionResult<ProductUom>> Post(ProductUom productUom)
        {
            if(productUom == null)
            {
                return BadRequest();
            }

            db.ProductUoms.Add(productUom);
            await db.SaveChangesAsync();
            return Ok(productUom);
        }
    }
}
