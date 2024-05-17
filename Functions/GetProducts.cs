using Dokimh8.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dokimh8.Functions
{
    public class GetProducts
    {
        private readonly ILogger<GetProducts> _logger;
        private readonly AppDbContext _dbContext;
        public GetProducts(ILogger<GetProducts> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Function("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req)
        {

            try
            {


                _logger.LogInformation("Get ALL Products");
                var products = await _dbContext.Products.ToListAsync();
                return new OkObjectResult(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is a problem {ex.Message}");
                throw;
            }
        }
    }
}


//using System;
//using System.Threading.Tasks;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Azure.Functions.Worker.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Dokimh8.DataAccess;
//using System.Linq;

//namespace Dokimh8.Functions
//{
//    public class GetProducts
//    {
//        private readonly ILogger<GetProducts> _logger;
//        private readonly AppDbContext _dbContext;

//        public GetProducts(ILogger<GetProducts> logger, AppDbContext dbContext)
//        {
//            _logger = logger;
//            _dbContext = dbContext;
//        }

//        [Function("GetAllProducts")]
//        public async Task<HttpResponseData> GetAllProducts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequestData req)
//        {
//            try
//            {
//                _logger.LogInformation("Get ALL Products");
//                var products = await _dbContext.Products.ToListAsync();

//                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
//                await response.WriteAsJsonAsync(products);

//                return response;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"There is a problem: {ex.Message}");

//                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
//                await errorResponse.WriteStringAsync("Internal Server Error");

//                return errorResponse;
//            }
//        }
//    }
//}
