using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApi_AfanasevNS.Controllers;
using TestWebApi_AfanasevNS.Models;
using Xunit;

namespace TestWebApi_AfanasevNS.Tests
{
    public class MovementsControllerTest
    {
        DbContextOptions<ProdContext> options;

        public MovementsControllerTest()
        {
            options = new DbContextOptionsBuilder<ProdContext>()
           .UseInMemoryDatabase(databaseName: "productsdb")
           .Options;

            var context = new ProdContext(options);

            if (!context.Movements.Any())
            {
                using (context)
                {
                    context.Movements.Add(new ProductMovements 
                    { 
                        ProductId = 1, Quantity = 10
                    });

                    context.Movements.Add(new ProductMovements 
                    { 
                        ProductId = 2, Quantity = 100
                    });

                    context.Movements.Add(new ProductMovements 
                    { 
                        ProductId = 3, Quantity = 145 
                    });

                    context.Movements.Add(new ProductMovements 
                    { 
                        ProductId = 4, Quantity = 134 
                    });

                    context.Movements.Add(new ProductMovements 
                    { 
                        ProductId = 1, Quantity = -2 
                    });
                    context.SaveChanges();
                }
            }
        }

        [Fact]
        public async Task MovementsContrTest()
        {
            using (var context = new ProdContext(options))
            {
                var control = new MovementsController(context);
                var result = await control.GetSum(1);
                var actualResult = result.Value;
                Assert.Equal(8, actualResult);
            }
        }

        [Fact]
        public async Task MovementsContrAdd()
        {
            using (var contex = new ProdContext(options))
            {
                var control = new MovementsController(contex);
                ProductMovements newUom = new ProductMovements { ProductId = 2, Quantity = -50 };
                var result = await control.Post(newUom);
                var okResult = result.Result as OkObjectResult;
                Assert.Equal(newUom, okResult.Value);
                Assert.Equal(200, okResult.StatusCode);
            }
        }

        [Fact]
        public async Task MovementsContrAddBadReq()
        {
            using (var contex = new ProdContext(options))
            {
                var control = new MovementsController(contex);
                var result = await control.Post(null);
                var badResult = result.Result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
            }
        }
    }
}
