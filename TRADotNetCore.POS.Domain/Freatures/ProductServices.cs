using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRADotNetCore.POS.Database.Models;
using TRADotNetCore.POS.Domain.Functions;
using TRADotNetCore.POS.Domain.Models.Product;

namespace TRADotNetCore.POS.Domain.Freatures;


public class ProductServices
{
    private readonly ProductServiceHelper _serviceHelper = new ProductServiceHelper();
    
    public async Task<ProductResponseModel> GetAllProductAsync()
    {

        var modelList = await  _serviceHelper.GetProductViewModelListAsync();
       



        return new ProductResponseModel { 


            response =(modelList is null)? BaseResponseModel.InternalServerError("503","Product Not Found"): BaseResponseModel.Success("200", "Product Query Successful"),
            productModels = modelList

        };

       
    }

    public async Task<Result<ResultProductResponseModel>> GetAllProductAsync2()
    {

        var modelList = await _serviceHelper.GetProductViewModelListAsync();


        var item = new ResultProductResponseModel { 
        
        productModels = modelList
        };

        

        return (modelList is null) ? Result<ResultProductResponseModel>.InternalServerError("Product Not Found"): Result<ResultProductResponseModel>.Success(item);
        


    }

    public async Task<ProductResponseModel> GetProductInStockAsync(string productName)
    {
        string code = "200"; string description = "Item Retrived Successfully";

        #region Validation
        var item = _serviceHelper.getProductByCodeOrName(productName);
       
        if (item is null) {
            description = "Item Not Found";
            code = "400";
            goto ValidError; }




        #endregion

      //  var stockRecord = _serviceHelper.getProductInstock(item.ProductCode);
        var viewModelList = await _serviceHelper.GetProductViewModelListAsync(item.ProductCode);
        if (viewModelList is null)
        {
            code = "204";
            description = "no Product in stock";
            goto ValidError;
        }

        return new ProductResponseModel
        {
            response = BaseResponseModel.Success(code, description),
            productModels = viewModelList

        };


    ValidError:
        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = null
        };

    }
    
    public async Task<ProductResponseModel> GetProductByName(string productName)
    {

        string code = "200"; string description = "Item Retrived Successfully";
        var item = _serviceHelper.getProductByCodeOrName(productName);
        if (item is null)
        {
            description = "Item Not Found";
            code = "400";
            goto ValidError;
        }

        var productList = await _serviceHelper.GetProductViewModelListAsync(productName);

        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = productList
        };





    ValidError:
        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = null
        };

    }

    public async Task<ProductResponseModel> GetProductByCategory(string CategoryName)
    {
        string code = "200"; string description = "Item Retrived Successfully";

        var productList =await _serviceHelper.GetProductViewModelListAsync(CategoryName);
         if (productList is null)
        {
            description = "Item Not Found";
            code = "400";
            goto ValidError;
        }


        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = productList
        };

    ValidError:
        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = null
        };


    }

    public async Task<ProductResponseModel> GetAllProductsInStock() {

        string code = "200";
        string description = "All in-stock items retrieved successfully";


        var productList = await _serviceHelper.GetProductViewModelListAsync();
        if (productList is null)
        {
            description = "Item Not Found";
            code = "400";
            goto NotFoundError;
        }

        
        return new ProductResponseModel {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = productList

        };


    NotFoundError:
        return new ProductResponseModel
        {
            response = BaseResponseModel.NotFoundError(code, description),
            productModels = null
        };

    }


}
