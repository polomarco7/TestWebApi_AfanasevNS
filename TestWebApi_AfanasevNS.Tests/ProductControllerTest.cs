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
    public class ProductControllerTest
    {
        DbContextOptions<ProdContext> options;

        public ProductControllerTest()
        {
            options = new DbContextOptionsBuilder<ProdContext>()
           .UseInMemoryDatabase(databaseName: "productsdb")
           .Options;

            var context = new ProdContext(options);

            if (!context.Prod.Any())
            {
                using (context)
                {
                    context.Prod.Add(new Product 
                    { 
                        Title = "Бананы", UomId = 2 
                    });

                    context.Prod.Add(new Product 
                    {
                        Title = "Специи", UomId = 1 
                    });

                    context.Prod.Add(new Product 
                    { 
                        Title = "Мандарины", UomId = 2 
                    });

                    context.Prod.Add(new Product 
                    { 
                        Title = "Авокадо", UomId = 3 
                    });

                    context.Prod.Add(new Product
                    {
                        Title = "Апельсины", UomId = 2
                    });

                    context.Prod.Add(new Product
                    {
                        Title = "Груши", UomId = 2
                    });
                    context.SaveChanges();
                }                
            }
        }

        [Fact]
        public async Task ProductContrTest()
        {
            using (var context = new ProdContext(options))
            {
                ProductController control = new ProductController(context);
                var result = await control.Get();
                var actualResult = result.Result;
                var products = new List<ProductAll>
                {
                    new ProductAll { Id = 1, Title="Бананы", Uom = "кг"},
                    new ProductAll { Id = 2, Title="Специи", Uom = "г"}
                };
                int i = 0;

                foreach (var x in result.Value)
                {
                    Assert.Equal(products[i].Id, x.Id);
                    Assert.Equal(products[i].Title, x.Title);
                    Assert.Equal(products[i].Uom, x.Uom);
                    i++;
                }
            }
        }

        [Fact]
        public async Task ProductContrPageTest()
        {
            using (var context = new ProdContext(options))
            {
                ProductController control = new ProductController(context);
                var result = await control.Get(2);
                var actualResult = result.Result;
                var products = new List<ProductAll>
                {
                    new ProductAll { Id = 3, Title="Мандарины", Uom = "кг"},
                    new ProductAll { Id = 4, Title="Авокадо", Uom = "шт"}
                };
                int i = 0;

                foreach (var x in result.Value)
                {
                    Assert.Equal(products[i].Id, x.Id);
                    Assert.Equal(products[i].Title, x.Title);
                    Assert.Equal(products[i].Uom, x.Uom);
                    i++;
                }
            }
        }

        [Fact]
        public async Task ProductContrPageSizeTest()
        {
            using (var context = new ProdContext(options))
            {
                ProductController control = new ProductController(context);
                var result = await control.Get(2, 3);
                var actualResult = result.Result;
                var products = new List<ProductAll>
                {
                    new ProductAll { Id = 4, Title = "Авокадо", Uom = "шт"},
                    new ProductAll { Id = 5, Title = "Апельсины", Uom = "кг"},
                    new ProductAll { Id = 6, Title = "Груши", Uom = "кг"}
                };
                int i = 0;
                foreach (var x in result.Value)
                {
                    Assert.Equal(products[i].Id, x.Id);
                    Assert.Equal(products[i].Title, x.Title);
                    Assert.Equal(products[i].Uom, x.Uom);
                    i++;
                }
            }
        }

        [Fact]
        public async Task ProductContrPageBadReq()
        {
            using (var contex = new ProdContext(options))
            {
                var control = new ProductController(contex);

                var result = await control.Get(-1);
                var badResult = result.Result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
            }
        }

        [Fact]
        public async Task ProductContrPageSizeBadReq()
        {
            using (var contex = new ProdContext(options))
            {
                var control = new ProductController(contex);

                var result = await control.Get(-1, -1);
                var badResult = result.Result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
            }
        }

        [Fact]
        public async Task ProductContrAdd()
        {
            using (var contex = new ProdContext(options))
            {
                var control = new ProductController(contex);
                Product newUom = new Product { Title = "Яблоки", UomId = 2 };
                var result = await control.Post(newUom);
                var okResult = result.Result as OkObjectResult;
                Assert.Equal(newUom, okResult.Value);
                Assert.Equal(200, okResult.StatusCode);
            }
        }

        [Fact]
        public async Task ProductContrAddBadReq()
        {
            using (var contex = new ProdContext(options))
            {
                var control = new ProductController(contex);
                var result = await control.Post(null);
                var badResult = result.Result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
            }
        }
    }
}
