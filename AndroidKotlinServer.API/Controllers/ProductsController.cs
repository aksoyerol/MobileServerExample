using AndroidKotlinServer.API.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndroidKotlinServer.API.Controllers
{
    [Authorize]
    //Odata konfigurasyonu için Controller Base'den miras alınmaz.
    public class ProductsController : ODataController
    {
        private readonly AppDbContext _appDbContext;
        public ProductsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        //Mutlaka geriye IQuaryble dönmeli !
        //odata/products endpointi üzerinden istek yapabileceğiz.
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Products.AsQueryable());
        }

        //odata/products(id) ile işlem yapabilmek için odata'nın tanıyacağı
        //biçimde get func'ı overload yapıyoruz.

        [EnableQuery(PageSize = 5)]
        public IActionResult Get([FromODataUri] int key)
        {
            return Ok(_appDbContext.Products.Where(x => x.Id == key));
        }

        //Odata ile POST işleminin yapılması
        //isimlendirme çok önemli. İlgili endpointi isimleri ile tanır.
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            await _appDbContext.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            return Ok(product);
        }

        //Güncelleme işlemi odata/product(id)
        [HttpPut]
        public async Task<IActionResult> PutProduct([FromODataUri] int key, [FromBody] Product product)
        {
            product.Id = key;
            _appDbContext.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _appDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int key)
        {
            var product = await _appDbContext.Products.FindAsync(key);
            if (product == null) return NotFound();
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return NoContent();
        }


    }
}
