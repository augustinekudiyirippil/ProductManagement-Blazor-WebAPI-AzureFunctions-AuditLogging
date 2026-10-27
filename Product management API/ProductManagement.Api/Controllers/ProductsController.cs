using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Api.DTOs;
using ProductManagement.Api.Models;
using ProductManagement.Api.Repositories;
using ProductManagement.Api.Services;

namespace ProductManagement.Api.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IAuditFunctionService _auditFunctionService;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IProductImageRepository _productImageRepository;

    public ProductsController(IProductService productService,
        IAuditFunctionService auditFunctionService,
        IBlobStorageService blobStorageService,
        IProductImageRepository productImageRepository)
    {
        _productService = productService;
        _auditFunctionService = auditFunctionService;
        _blobStorageService = blobStorageService;
        _productImageRepository= productImageRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();

        return Ok(products);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }




    //[HttpPost]
    //public async Task<ActionResult<Product>> CreateProduct(ProductRequestDto request)
    //{
    //    try
    //    {
    //        var product = new Product
    //        {
    //            Name = request.Name,
    //            Description = request.Description,
    //            Price = request.Price,
    //            CreatedDate = request.CreatedDate

    //        };

    //        await _productService.CreateProductAsync(product);

    //        try
    //        {
    //            await _auditFunctionService.LogAsync(
    //                new ProductAuditRequest
    //                {
    //                    ProductId = product.Id,
    //                    UserId = request.UserId,
    //                    ActionType = "Add",
    //                    Description = $"Added {product.Name} "
    //                });
    //        }
    //        catch (Exception auditEx)
    //        {
    //            // Log audit failure, but don't fail the product creation
    //            Console.WriteLine($"Audit logging failed: {auditEx.Message}");
    //        }

    //        return Ok(product);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Product creation failed: {ex.Message}");

    //        return StatusCode(
    //            StatusCodes.Status500InternalServerError,
    //            new
    //            {
    //                message = "An error occurred while creating the product."
    //            });
    //    }
    //}


    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(
    ProductRequestDto request)
    {
        try
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CreatedDate = request.CreatedDate
            };

            var createdProduct =
                await _productService.CreateProductAsync(
                    product,
                    request.UserId);

            return Ok(createdProduct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Product creation failed: {ex.Message}");

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "An error occurred while creating the product."
                });
        }
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(
        int id,
        ProductRequestDto request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

       
            var product = new Product
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CreatedDate = request.CreatedDate
            };

            await _productService.UpdateProductAsync(
                product,
                request.UserId);

            

            return NoContent();
        
    }





    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(ProductRequestDto request)
    {
        try
        {
            await _productService.DeleteProductAsync(
                request.Id,
                request.UserId,
                 request.Name);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Failed to delete ProductId {request.Id}: {ex}");

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "An error occurred while deleting the product."
                });
        }
    }




    [HttpPatch("disable")]
    public async Task<IActionResult> DisableProduct(ProductRequestDto request)
    {
        try
        {

            await _productService.MakeUnavailableAsync(
                request.Id,
                request.UserId,
                request.Name);

           

            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Failed to disable  ProductId {request.Id}: {ex}");

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "An error occurred while disble the product."
                });
        }
    }




    [HttpPatch("enable")]
    public async Task<IActionResult> EnableProduct(ProductRequestDto request)
    {
        try
        {

            await _productService.MakeAvailableAsync(
                  request.Id,
                  request.UserId,
                  request.Name);
        
         

            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Failed to enable  ProductId {request.Id}: {ex}");

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "An error occurred while disble the product."
                });
        }
    }





    [HttpPost("{productId}/images")]
    public async Task<IActionResult> UploadImage(
    int productId,
    [FromForm] ProductImageUploadDto request)
    {
        var blobUrl =
            await _blobStorageService
                .UploadAsync(
                    productId,
                    request.File);

        if (request.IsPrimary)
        {
            var existingImages =
                await _productImageRepository
                    .GetByProductIdAsync(productId);

            foreach (var image in existingImages)
            {
                image.IsPrimary = false;
            }

            await _productImageRepository
                .SaveChangesAsync();
        }

        var imageToSave =
            new ProductImage
            {
                ProductId = productId,
                FileName = request.File.FileName,
                BlobUrl = blobUrl,
                UploadedDate = DateTime.UtcNow,
                IsPrimary = request.IsPrimary,
                IsDeleted = false
            };

        await _productImageRepository
            .AddAsync(imageToSave);

        return Ok();
    }


    //[HttpGet("{productId}/images")]
    //public async Task<IActionResult> GetImages(
    //int productId)
    //{
    //    var images =
    //        await _productImageRepository
    //            .GetByProductIdAsync(productId);

    //    return Ok(images);
    //}



    [HttpGet("{productId}/images")]
    public async Task<IActionResult> GetImages(
    int productId)
    {
        var images =
            await _productImageRepository
                .GetByProductIdAsync(productId);

        var result =
            images.Select(x =>
                new ProductImageDto
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    BlobUrl = x.BlobUrl,

                    SasUrl =
                        _blobStorageService
                            .GenerateReadSasUrl(
                                x.BlobUrl),

                    IsPrimary = x.IsPrimary
                });

        return Ok(result);
    }



    [HttpDelete("images/{imageId}")]
    public async Task<IActionResult> DeleteImage(
    int imageId)
    {
        var image =
            await _productImageRepository
                .GetByIdAsync(imageId);

        if (image == null)
        {
            return NotFound();
        }

        image.IsDeleted = true;

        await _productImageRepository
            .SaveChangesAsync();

        return Ok(new
        {
            Message = "Image deleted successfully."
        });
    }

}