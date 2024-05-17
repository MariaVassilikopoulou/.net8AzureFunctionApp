using Dokimh8.DataAccess;
using Dokimh8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dokimh8.Functions
{
    public class CreateProduct
    {
        private readonly ILogger<CreateProduct> _logger;
        private readonly AppDbContext _dbContext;

        public CreateProduct(ILogger<CreateProduct> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Function("CreateAProduct")]
        public async Task< IActionResult > CreateAProduct([HttpTrigger(AuthorizationLevel.Function,  "post", Route= "product")] HttpRequest req)
        {
            try
            {
               
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonConvert.DeserializeObject<Products>(requestBody);
                _dbContext.Products.Add(product);
               await _dbContext.SaveChangesAsync();

               _logger.LogInformation("Creat a new Product");
                return new OkObjectResult($"You just added : {product.Name} a new product");
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is a problem {ex.Message}");
                throw;
            }
        }
    }
}
