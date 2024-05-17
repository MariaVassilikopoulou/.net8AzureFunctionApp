using Dokimh8.DataAccess;
using Dokimh8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dokimh8.Functions
{
    public class UpdateProduct
    {
        private readonly ILogger<UpdateProduct> _logger;
        private readonly AppDbContext _dbContext;
        public UpdateProduct(ILogger<UpdateProduct> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext= dbContext;
        }

        [Function("UpdateAProduct")]
        public async Task<IActionResult> UpdateAProduct([HttpTrigger(AuthorizationLevel.Function, "put", Route ="product/{id}")] HttpRequest req, Guid Id)
        {
            try
            {
                var selectedProduct = await _dbContext.Products.FindAsync(Id);
                if (selectedProduct == null)
                {
                    return new NotFoundResult();
                }

                string requestData = await new StreamReader(req.Body).ReadToEndAsync();
                var updatedProduct = JsonConvert.DeserializeObject<Products>(requestData);

                selectedProduct.Name = updatedProduct.Name;
                selectedProduct.Price = updatedProduct.Price;

                _dbContext.Products.Update(selectedProduct);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Update a Product");
                return new OkObjectResult($"You just updated the: {updatedProduct.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is a problem {ex.Message}");
                throw;
            }
        }
    }
}
