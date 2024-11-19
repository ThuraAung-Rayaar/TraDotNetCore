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

        public List<ProductViewModel>? GetProductViewModelList(string? checker=null) {

            var productList = getAllProduct()?.Where(x => x.ProductCode == checker || x.ProductName == checker).ToList();
            var categoryList = getAllCategory()?.Where(x => x.ProductCategoryName == checker).ToList();
            var instockList = getAllInStockRecord();

            if (productList is null || categoryList is null || instockList is null)
            {
                return null;
            }

           

            List<ProductViewModel> modelList = new List<ProductViewModel>();
            foreach (ProductDetail product in productList)
            {
                
                ProductCategory category = categoryList.FirstOrDefault(x => x.ProductCategoryCode == product.ProductCategoryCode)!;
                ProductInStock inStock = instockList.FirstOrDefault(x => x.ProductCode == product.ProductCode)!;

                
                modelList.Add(GetProductVModel(product, category, inStock));
            }
            return modelList;

        }


        public int addCategory(ProductCategory category)
        {
            bool isOldProduct = dbcontext.ProductCategories.Any(x => x.ProductCategoryCode == category.ProductCategoryCode);
            if (isOldProduct) { return 0; }

            dbcontext.ProductCategories.Add(category);
            int result = dbcontext.SaveChanges();
            return result;
        }

        public int addInstockRecord(string productCode, int count)
        {
            ProductInStock inStockItem = new ProductInStock()
            {
                ProductCode = productCode,
                Count = count,
                StockDate = DateTime.Now
            };

            dbcontext.InStocks.Add(inStockItem);
            int resut = dbcontext.SaveChanges();
            return resut;

        }

        public bool checkCategory(ProductDetail product)
        {
            var result = dbcontext.ProductCategories.Any(x => x.ProductCategoryCode == product.ProductCategoryCode);//.FirstOrDefault();

            return result;
        }

        public int createProductDetail(ProductDetail product)
        {
            bool isOldProduct = dbcontext.Products.Any(x => x.ProductCode == product.ProductCode);
            if (isOldProduct) { return 0; }

            dbcontext.Products.Add(product);
            int result = dbcontext.SaveChanges();
            return result;
        }

        public int UpdateProductInstock(string productCode, int count)
        {
            int result = 0;
            var stock = dbcontext.InStocks.AsNoTracking().Where(x => x.ProductCode == productCode).FirstOrDefault();
            if (stock is null) { result = addInstockRecord(productCode, count); return result; }

            stock.Count = count;


            dbcontext.Entry(stock).State = EntityState.Modified;

            result = dbcontext.SaveChanges();
            return result;
        }

        public int EditProductInstock(string productCode, int count)
        {
            int result = 0;
            var stock = dbcontext.InStocks.AsNoTracking().Where(x => x.ProductCode == productCode).FirstOrDefault();
            if (stock is null) { return result; }

            stock.Count
                += count;


            dbcontext.Entry(stock).State = EntityState.Modified;

            result = dbcontext.SaveChanges();
            return result;
        }

        public List<ProductDetail>? getAllProduct()
        {
            var Productlist = dbcontext.Products.ToList();
            //.Where(
            //     x => dbcontext.InStocks.Any(y => y.ProductCode == x.ProductCode)
            //     );

            return Productlist;
        }
        public List<ProductCategory>? getAllCategory()
        {
            var categoryList = dbcontext.ProductCategories.ToList();
            //.Where(
            //     x => dbcontext.InStocks.Any(y => y.ProductCode == x.ProductCode)
            //     );

            return categoryList;
        }

        public List<ProductInStock>? getAllInStockRecord()
        {
            var instockList = dbcontext.InStocks.ToList();
            //.Where(
            //     x => dbcontext.InStocks.Any(y => y.ProductCode == x.ProductCode)
            //     );

            return instockList;
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
