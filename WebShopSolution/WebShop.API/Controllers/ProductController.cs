using Microsoft.AspNetCore.Mvc;
using WebShop.API.Interfaces;
using WebShop.DataAccess.Entities;

namespace WebShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var products = await productService.GetAllAsync();
            return Ok(products);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        try
        {
            var product = await productService.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] Product? product)
    {
        if (product is null)
        {
            return BadRequest("Product is null.");
        }

        try
        {
            product.Id = 0;
            await productService.AddAsync(product);
            return Created(string.Empty, product);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] Product? product)
    {
        if (product is null)
        {
            return BadRequest("Product is null.");
        }

        if (string.IsNullOrWhiteSpace(product.Name) ||
            string.IsNullOrWhiteSpace(product.Description) ||
            product.Price < 0)
        {
            return BadRequest("All product fields must be provided.");
        }

        try
        {
            var existingProduct = await productService.GetByIdAsync(product.Id);
            if (existingProduct is null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;

            await productService.UpdateAsync(existingProduct);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var existingProduct = await productService.GetByIdAsync(id);
            if (existingProduct is null)
            {
                return NotFound();
            }

            await productService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
