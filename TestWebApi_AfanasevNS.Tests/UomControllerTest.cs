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
    public class UomControllerTest
    {    
        DbContextOptions<ProdContext> options;

        public UomControllerTest()
        {     
             options = new DbContextOptionsBuilder<ProdContext>()
            .UseInMemoryDatabase(databaseName: "productsdb")
            .Options;            
            var context = new ProdContext(options);
            if (!context.ProductUoms.Any())
            {
                using (context)
                {
                    context.ProductUoms.Add(new ProductUom
                    {
                        Title = "г"
                    });

                    context.ProductUoms.Add(new ProductUom
                    {
                        Title = "кг"
                    });

                    context.ProductUoms.Add(new ProductUom
                    {
                        Title = "шт"
                    });

                    context.ProductUoms.Add(new ProductUom
                    {
                        Title = "т"
                    });
                    context.SaveChanges();
                }
            }
        }

        [Fact]
        public async Task UomContrTest()
        {
            using (var context = new ProdContext(options))
            {
                UomController control = new UomController(context);
                var result = await control.Get();
                var actualResult = result.Result;
                var productUoms = new List<ProductUom>
                {
                    new ProductUom { Id = 1, Title="г"},
                    new ProductUom { Id = 2, Title="кг"}
                };
                int i = 0;

                foreach (var x in result.Value)
                {
                    Assert.Equal(productUoms[i].Id, x.Id);
                    Assert.Equal(productUoms[i].Title, x.Title);
                    i++;
                }
            }            
        }

        [Fact]
        public async Task UomContrPageTest()
        { 
            using (var context = new ProdContext(options))
            {
                UomController control = new UomController(context);
                var result = await control.Get(2);
                var actualResult = result.Result;
                var productUoms = new List<ProductUom>
                {
                    new ProductUom { Id = 3, Title="шт"},
                    new ProductUom { Id = 4, Title="т"}
                };
                int i = 0;

                foreach (var x in result.Value)
                {
                    Assert.Equal(productUoms[i].Id, x.Id);
                    Assert.Equal(productUoms[i].Title, x.Title);
                    i++;
                }
            }
        }

        [Fact]
        public async Task UomContrPageSizeTest()
        {
            using (var context = new ProdContext(options))
            {
                UomController control = new UomController(context);
                var result = await control.Get(2, 3);
                var actualResult = result.Result;
                var productUoms = new ProductUom { Id = 4, Title = "т" };

                foreach (var x in result.Value)
                {
                    Assert.Equal(productUoms.Id, x.Id);
                    Assert.Equal(productUoms.Title, x.Title);
                }
            }
        }

        [Fact]
        public async Task UomContrPageBadReq()
        {
            using (var contex = new ProdContext(options))
            {
                var control = new UomController(contex);

                var result = await control.Get(-1);
                var badResult = result.Result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
            }
        }

        [Fact]
        public async Task UomContrPageSizeBadReq()
        {
            using (var contex = new ProdContext(options))
            {
                var control = new UomController(contex);

                var result = await control.Get(-1, -1);
                var badResult = result.Result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
            }
        }

        [Fact]
        public async Task UomContrAdd()
        {
            using (var contex = new ProdContext(options))
            {
                var control = new UomController(contex);
                ProductUom newUom = new ProductUom { Title = "л" };
                var result = await control.Post(newUom);
                var okResult = result.Result as OkObjectResult;
                Assert.Equal(newUom, okResult.Value);
                Assert.Equal(200, okResult.StatusCode);
            }
        }

        [Fact]
        public async Task UomContrAddBadReq()
        {
            using (var contex = new ProdContext(options))
            {
                var control = new UomController(contex);
                var result = await control.Post(null);
                var badResult = result.Result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
            }
        }
    }
}
