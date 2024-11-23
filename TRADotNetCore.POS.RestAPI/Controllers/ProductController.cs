using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TRADotNetCore.POS.Database.Models;
using TRADotNetCore.POS.Domain.Freatures;
using TRADotNetCore.POS.Domain.Models.Product;

namespace TRADotNetCore.POS.RestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : BaseResponseController
{
   private readonly ProductServices _services = new ProductServices();


    [HttpGet("product")]
    public async Task<Result<ResultProductResponseModel>> GetAllProducts()
    {
        var response = await _services.GetAllProductAsync2();
       
        

        return response;
    }

    [HttpGet("product2")]
    public async Task<IActionResult> GetAllProduct2()
    {
        var response = await _services.GetAllProductAsync();

        

      return  Excute(response);
    }
    [HttpGet("product/instock")]
    public IActionResult GetAllProductInStock()
    {
        var response = _services.GetAllProductsInStock();
        return Ok(response);
    }
    [HttpGet("product/instock/{name}")]
    public IActionResult GetProductInStock(string productName)
    {
        var response = _services.GetProductInStockAsync(productName);
        return Ok(response);
    }

    [HttpGet("product/{Name}")]
    public IActionResult GetProductByName(string productName)
    {
        var response = _services.GetProductByName(productName);
        return Ok(response);
    }

    [HttpGet("Product/category/{categoryName}")]
    public IActionResult GetProductByCategory(string categoryName)
    {
        var response = _services.GetProductByCategory(categoryName);
        return Ok(response);
    }

    //instock detail

    /* [HttpPost("Product/add")]
     public IActionResult addProductInfo(ProductDetail product,int Count) {

         bool categoryExist = _service.checkCategory(product);
         if (!categoryExist) return NotFound("Category Don't Exist");

         int productRe = _service.createProductDetail(product);
         //int InStockRe = _service.addInstockRecord(product.ProductCode, Count);
         if ( productRe == 0) { return BadRequest("Error Creating Product"); }

         return Ok("Product Added");

     }
     [HttpPost("Product/instock")]
     public IActionResult AddInstockInventory(ProductDetail product, int Count)
     {




         //int productRe = _service.createProductDetail(product);
         int InStockRe = _service.addInstockRecord(product.ProductCode, Count);
         if (InStockRe == 0) { return BadRequest("Error stocking Product"); }

         return Ok("Product stocked");

     }
     [HttpPost("Product/category")]
     public IActionResult AddNewCategory(ProductCategory category)
     {




         //int productRe = _service.createProductDetail(product);
         int categoryRe = _service.addCategory(category);
         if (categoryRe == 0) { return BadRequest("Error creating Product category"); }

         return Ok("Product category Created");

     }


     [HttpPatch("Product/{name}/InStock/{count}")]
     public IActionResult AddProductStock(string ProductCode, int count) {

         var item = _service.GetProductByCodeOrName(ProductCode);
         if (item is null) return NotFound("No Product Error");

         int result = _service.UpdateProductInstock(ProductCode, count);
         if (result == 0) return BadRequest();

         return Ok("Product Re stocked");


     }

     [HttpPatch("Product/category/edit/{code}")]

     public IActionResult EditProductInfo(string productCode, string? productName = null, string? CAtegoryName=null, decimal Price=0.0M) {
         _service.GetProductByCodeOrName(productCode); // Check NotFound
         if (String.IsNullOrEmpty(productName)) ;//null check
         if (String.IsNullOrEmpty(CAtegoryName)) ;//NULL CHECK
         if(Price is 0.0M) Price = 0;//No Value Check
         //_service.EditProductInfo(string productCode, string? productName = null, string? CAtegoryName=null, decimal Price=0.0M)


         return Ok();
     }


     [HttpDelete("Product/delete/{code}")]
     public IActionResult DeleteProduct(string code) {
         _service.GetProductByCodeOrName(code);
          //_service.DeleteInstockRecord(code) 
          //_serive.DeleteProduct(code)


         return Ok();
     }

     [HttpDelete("Product/instock/delete/{code}")]
     public IActionResult ClearInstock(string code)
     {
         _service.GetProductByCodeOrName(code);

         //_service.DeleteInstockRecord(code)

         return Ok();
     }

     [HttpDelete("Product/category/delete/{code}")]
     public IActionResult DeleteCategory(string categoryCode) {

         //_service.ProductsetToDefaultCategory(categoryCode)
         //_service.DeleteCAtegory(categoryCode)


         return Ok();
     }*/

}
