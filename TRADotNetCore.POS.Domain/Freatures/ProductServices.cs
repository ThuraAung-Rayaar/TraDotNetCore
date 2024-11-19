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
    
    public ProductResponseModel GetAllProduct()
    {

        var modelList = _serviceHelper.GetProductViewModelList();
       



        return new ProductResponseModel { 


            response =(modelList is null)? BaseResponseModel.InternalServerError("503","Product Not Found"): BaseResponseModel.Success("200", "Product Query Successful"),
            productModels = modelList

        };

       
    }

    
    public ProductResponseModel GetProductInStock(string productName)
    {
        string code = "200"; string description = "Item Retrived Successfully";
        #region Validation
        var item = _serviceHelper.getProductByCodeOrName(productName);
       
        if (item is null) {
            description = "Item Not Found";
            code = "400";
            goto ResultA; }




        #endregion

        var stockRecord = _serviceHelper.getProductInstock(item.ProductCode);
        if (stockRecord is null)
        {
            code = "204";
            description = "no Product in stock";
            goto ResultB;
        }

        return new ProductResponseModel { 
            response = BaseResponseModel.Success(code, description),
            productModels = _serviceHelper.GetProductViewModelList()!.Where(x=>x.productCode == stockRecord.ProductCode).ToList()
        
        };
        

    ResultA:
        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = null
        };

    ResultB:
        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = null
        };
    }
    
    public ProductResponseModel GetProductByName(string productName)
    {

        string code = "200"; string description = "Item Retrived Successfully";
        var item = _serviceHelper.getProductByCodeOrName(productName);
        if (item is null)
        {
            description = "Item Not Found";
            code = "400";
            goto ResultA;
        }

        var productList = _serviceHelper.GetProductViewModelList(productName);

        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = productList
        };





    ResultA:
        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = null
        };
    }
    
    public ProductResponseModel GetProductByCategory(string CategoryName)
    {
        string code = "200"; string description = "Item Retrived Successfully";

        var productList = _serviceHelper.GetProductViewModelList(CategoryName);
         if (productList is null)
        {
            description = "Item Not Found";
            code = "400";
            goto ResultA;
        }


        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = productList
        };


    ResultA:
        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = null
        };

       
    }

    public ProductResponseModel GetAllProductsInStock() {

        string code = "200";
        string description = "All in-stock items retrieved successfully";


        var productList = _serviceHelper.GetProductViewModelList();
        if (productList is null)
        {
            description = "Item Not Found";
            code = "400";
            goto ResultA;
        }

        List<ProductViewModel> InStockproducts = new List<ProductViewModel>();

        foreach (var item in productList) {

            if(_serviceHelper.getProductInstock(item.productCode) is not null)  InStockproducts.Add(item);
        
        }
        return new ProductResponseModel {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = InStockproducts

        };


    ResultA:
        return new ProductResponseModel
        {
            response = BaseResponseModel.ValidationError(code, description),
            productModels = null
        };

    }


}
