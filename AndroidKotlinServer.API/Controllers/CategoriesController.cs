using AndroidKotlinServer.API.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndroidKotlinServer.API.Controllers
{
    public class CategoriesController : ODataController
    {
        private readonly AppDbContext _appDbContext;
        public CategoriesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Categories.AsQueryable());
        }
    }
}
