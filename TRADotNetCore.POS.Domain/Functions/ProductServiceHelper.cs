using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRADotNetCore.POS.Database.Models;
using TRADotNetCore.POS.Domain.Models.Product;

namespace TRADotNetCore.POS.Domain.Functions
{
    public class ProductServiceHelper
    {




        private readonly AppDbContext dbcontext = new AppDbContext();




        public ProductViewModel GetProductVModel(ProductDetail product, ProductCategory category, ProductInStock inStock) {


            return new ProductViewModel { 
            productName = product.ProductName,
            productCategoryName = category.ProductCategoryName,
           productCode = product.ProductCode,
           price = product.Price,
           productCount = inStock.Count,
           lastStocked = inStock.StockDate
            
            
            };
        
        
        }

        public async Task<List<ProductViewModel>?> GetProductViewModelListAsync(string? checker = null)
        {
            List<ProductDetail>? productList = null;
            List<ProductCategory>? categoryList = null;
            List<ProductInStock>? instockList = null;

            
            if (string.IsNullOrEmpty(checker))
            {
                productList = await getAllProductsAsync();
                categoryList = await getAllCategoryAsync();
            }
            else
            {
                
                productList =  (await getAllProductsAsync())?.Where(x => x.ProductCode == checker || x.ProductName == checker).ToList();

               
                if (productList is null || productList.Count == 0)
                {
                    productList = await getAllProductsAsync();
                    categoryList = (await getAllCategoryAsync())?.Where(x => x.ProductCategoryName == checker).ToList();
                }
                else
                {
                    
                    categoryList = await getAllCategoryAsync();
                }
            }

           
            instockList = await getAllInStockRecordAsync();

            
            if (productList is null || productList.Count==0 ||
                categoryList is null || categoryList.Count==0 ||
                instockList is null || instockList.Count == 0)
            {
                return null;
            }

            List<ProductViewModel> modelList = new List<ProductViewModel>();

            
            foreach (ProductDetail product in productList)
            {
                
                var category = categoryList.FirstOrDefault(x => x.ProductCategoryCode == product.ProductCategoryCode);
                var inStock = instockList.FirstOrDefault(x => x.ProductCode == product.ProductCode && x.Count>0);

               
                if (category is null || inStock is null)
                {
                    continue;
                }


                modelList.Add(GetProductVModel(product, category, inStock));
            }

            return modelList;
        }


        public async Task<int> addCategory(ProductCategory category)
        {
            bool isOldProduct =  dbcontext.ProductCategories.Any(x => x.ProductCategoryCode == category.ProductCategoryCode);
            if (isOldProduct) { return 0; }

            dbcontext.ProductCategories.Add(category);
          int result = await dbcontext.SaveChangesAsync();
            return result;
        }

        public async Task<int> addInstockRecordAsync(string productCode, int count)
        {
            ProductInStock inStockItem = new ProductInStock()
            {
                ProductCode = productCode,
                Count = count,
                StockDate = DateTime.Now
            };

            dbcontext.InStocks.Add(inStockItem);
            int result = await dbcontext.SaveChangesAsync();
            return result;

        }

        //forget uses
        public bool checkCategory(ProductDetail product)
        {
            var result = dbcontext.ProductCategories.Any(x => x.ProductCategoryCode == product.ProductCategoryCode);//.FirstOrDefault();

            return result;
        }

        
        public async Task<int> createProductDetailAsync(ProductDetail product)
        {
            bool isOldProduct = dbcontext.Products.Any(x => x.ProductCode == product.ProductCode);
            if (isOldProduct) { return 0; }

            dbcontext.Products.Add(product);
            int result = await dbcontext.SaveChangesAsync();
            return result;
        }

        public async Task<int> UpdateProductInstockAsync(string productCode, int count)
        {
            int result = 0;
            var stock = dbcontext.InStocks.AsNoTracking().Where(x => x.ProductCode == productCode).FirstOrDefault();
            if (stock is null) { result = await addInstockRecordAsync(productCode, count); return result; }

            stock.Count = count;


            dbcontext.Entry(stock).State = EntityState.Modified;

            result = await dbcontext.SaveChangesAsync();
            return result;
        }

        public async Task<int> EditProductInstock(string productCode, int count)
        {
            int result = 0;
            var stock = dbcontext.InStocks.AsNoTracking().Where(x => x.ProductCode == productCode).FirstOrDefault();
            if (stock is null) { return result; }

            stock.Count
                += count;


            dbcontext.Entry(stock).State = EntityState.Modified;

            result = await dbcontext.SaveChangesAsync();
            return result;
        }

        public async Task<List<ProductDetail>> getAllProductsAsync()
        {
            var list= await dbcontext.Products.ToListAsync();
            return list;
        }
        public async Task<List<ProductCategory>> getAllCategoryAsync()
        {
            var list = await dbcontext.ProductCategories.ToListAsync();
            return list;
        }

        public async Task<List<ProductInStock>> getAllInStockRecordAsync()
        {
            var list = await dbcontext.InStocks.ToListAsync(); 
            return list;
        }

        public ProductDetail? getProductByCategoryName(string CategoryName)
        {
            var categoryItem = dbcontext.ProductCategories.Where(y => y.ProductCategoryName == CategoryName).FirstOrDefault();
            var ProductItem = dbcontext.Products.Where(
                x => x.ProductCode == categoryItem.ProductCategoryCode
                ).FirstOrDefault();
            return ProductItem;


        }

        public ProductDetail? getProductByCodeOrName(string queryInput)
        {
            var item = dbcontext.Products.Where(x => x.ProductCode == queryInput || x.ProductName == queryInput).FirstOrDefault();
            return item;
        }



        public ProductInStock? getProductInstock(string productCode)
        {
            var item = dbcontext.InStocks.Where(x => x.ProductCode == productCode && x.Count>0).FirstOrDefault();


            //var newDetail = item is null? 
            //        new { 
            //   Name= product.ProductName,
            //   Count = item.Count
            //    }:null;

            return item;
        }

       
    }
}
