using Dokimh8.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Dokimh8.Functions
{
    public class DeleteProduct
    {
        private readonly ILogger<DeleteProduct> _logger;
        private readonly AppDbContext _dbContext;
        public DeleteProduct(ILogger<DeleteProduct> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Function("DeleteAProduct")]
        public async Task <IActionResult> DeleteAProduct([HttpTrigger(AuthorizationLevel.Function, "delete", Route ="product/{id}")] HttpRequest req, Guid Id)
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(Id);
                if (product == null)
                {
                    return new NotFoundResult();
                }

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Delete a Product");
                return new OkObjectResult($"You just Deleted this product: {product.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is a problem {ex.Message}");
                throw;
            }
        }
    }
}
