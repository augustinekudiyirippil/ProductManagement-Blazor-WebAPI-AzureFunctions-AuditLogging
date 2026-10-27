using Microsoft.EntityFrameworkCore;
using ProductManagement.Api.DTOs;
using ProductManagement.Api.Models;
using ProductManagement.Api.Repositories;

namespace ProductManagement.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IAuditFunctionService _auditFunctionService;
    private readonly IEmailServiceBusService     _emailServiceBusService;


    public ProductService(
    IProductRepository repository,
    IAuditFunctionService auditFunctionService,
    IEmailServiceBusService emailServiceBusService)
    {
        _repository = repository;
        _auditFunctionService = auditFunctionService;
        _emailServiceBusService= emailServiceBusService;
    }


    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    //public async Task<Product> CreateProductAsync(Product product)
    //{
    //    product.CreatedDate = DateTime.UtcNow;
    //    product.IsAvailable = true;

    //    return await _repository.AddAsync(product);
    //}

    public async Task<Product> CreateProductAsync(
    Product product,
    int userId)
    {
        product.CreatedDate = DateTime.UtcNow;
        product.IsAvailable = true;

        var createdProduct =
            await _repository.AddAsync(product);

        try
        {
            await _emailServiceBusService.SendAsync(
            new ProductEmailMessage
            {
                ProductId = createdProduct.Id,
                ProductName = createdProduct.Name,
                ActionType = "Create",
                UserId = userId,
                EventDate = DateTime.UtcNow
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Sending  Email failed: {ex.Message}");
        }




        try
        {
            await _auditFunctionService.LogAsync(
                new ProductAuditRequest
                {
                    ProductId = createdProduct.Id,
                    UserId = userId,
                    ActionType = "Add",
                    Description =
                        $"Added {createdProduct.Name}"
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Audit logging failed on create: {ex.Message}");
        }

        return createdProduct;
    }

    //public async Task UpdateProductAsync(Product product)
    //{
    //    product.CreatedDate = DateTime.UtcNow;
    //    product.IsAvailable = true;

    //    await _repository.UpdateAsync(product);
    //}
    public async Task UpdateProductAsync(
    Product product,
    int userId)
    {
        product.CreatedDate = DateTime.UtcNow;
        product.IsAvailable = true;

        await _repository.UpdateAsync(product);


        try
        {
            await _emailServiceBusService.SendAsync(
            new ProductEmailMessage
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ActionType = "Update",
                UserId = userId,
                EventDate = DateTime.UtcNow
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Sending  Email failed: {ex.Message}");
        }


        try
        {
            await _auditFunctionService.LogAsync(
                new ProductAuditRequest
                {
                    ProductId = product.Id,
                    UserId = userId,
                    ActionType = "Edit",
                    Description = $"Modified {product.Name}"
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Audit logging failed on update: {ex.Message}");
        }
    }

    //public async Task DeleteProductAsync(int id)
    //{
    //    var product = await _repository.GetByIdAsync(id);

    //    if (product != null)
    //    {
    //        await _repository.DeleteAsync(product);
    //    }
    //}

    public async Task DeleteProductAsync(
    int id,
    int userId,
    string productName)
    {
        var product =
            await _repository.GetByIdAsync(id);

        if (product != null)
        {
            await _repository.DeleteAsync(product);


            try
            {
                await _emailServiceBusService.SendAsync(
                new ProductEmailMessage
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ActionType = "Delete",
                    UserId = userId,
                    EventDate = DateTime.UtcNow
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Sending  Email failed on Delete : {ex.Message}");
            }



            try
            {
                await _auditFunctionService.LogAsync(
                    new ProductAuditRequest
                    {
                        ProductId = id,
                        UserId = userId,
                        ActionType = "Delete",
                        Description =
                            $"Deleted {productName}"
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Audit logging failed: {ex.Message}");
            }
        }
    }

    //public async Task<bool> MakeUnavailableAsync(int id)
    //{
    //    var product = await _repository.GetByIdAsync(id);

    //    if (product == null)
    //    {
    //        return false;
    //    }

    //    product.IsAvailable = false;

    //    await _repository.UpdateAsync(product);

    //    return true;
    //}

    public async Task<bool> MakeUnavailableAsync(
    int id,
    int userId,
    string productName)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            return false;
        }

        product.IsAvailable = false;

        await _repository.UpdateAsync(product);


        try
        {
            await _emailServiceBusService.SendAsync(
            new ProductEmailMessage
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ActionType = "Disable",
                UserId = userId,
                EventDate = DateTime.UtcNow
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Sending  Email failed on disable : {ex.Message}");
        }

        try
        {
            await _auditFunctionService.LogAsync(
                new ProductAuditRequest
                {
                    ProductId = id,
                    UserId = userId,
                    ActionType = "Disable",
                    Description = $"Disabled {productName}"
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Audit logging failed: {ex.Message}");
        }

        return true;
    }

    //public async Task<bool> MakeAvailableAsync(int id)
    //{
    //    var product = await _repository.GetByIdAsync(id);

    //    if (product == null)
    //    {
    //        return false;
    //    }

    //    product.IsAvailable = true;

    //    await _repository.UpdateAsync(product);

    //    return true;
    //}

    public async Task<bool> MakeAvailableAsync(
    int id,
    int userId,
    string productName)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            return false;
        }

        product.IsAvailable = true;

        await _repository.UpdateAsync(product);


        try
        {
            await _emailServiceBusService.SendAsync(
            new ProductEmailMessage
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ActionType = "Enable",
                UserId = userId,
                EventDate = DateTime.UtcNow
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Sending  Email failed on enable : {ex.Message}");
        }

        try
        {
            await _auditFunctionService.LogAsync(
                new ProductAuditRequest
                {
                    ProductId = id,
                    UserId = userId,
                    ActionType = "Enable",
                    Description = $"Enabled {productName}"
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Audit logging failed: {ex.Message}");
        }

        return true;
    }

}