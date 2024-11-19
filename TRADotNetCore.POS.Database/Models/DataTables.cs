using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRADotNetCore.POS.Database.Models;

[Table("Products")]
public partial class ProductDetail
{
    [Key]
    [Column("ProductId")]
    public int ProductId { get; set; }

    [Column("ProductCode")]
    public string ProductCode { get; set; }

    [Column("ProductCategoryCode")]
    public string ProductCategoryCode { get; set; }

    [Column("ProductName")]
    public string ProductName { get; set; }

    [Column("Price")]
    public decimal Price { get; set; }
}


[Table("Product_Category")]
public partial class ProductCategory
{
    [Key]
    [Column("ProductCategoryId")]
    public int ProductCategoryId { get; set; }

    [Column("ProductCategoryCode")]
    public string ProductCategoryCode { get; set; }

    [Column("ProductCategoryName")]
    public string ProductCategoryName { get; set; }
}

[Table("Product_InStock")]
public partial class ProductInStock
{
    [Key]
    [Column("ProductCode")]
    public string ProductCode { get; set; }

   

    [Column("ProductCount")]
    public int Count { get; set; }

    [Column("InStockDate")]
    public DateTime StockDate { get; set; }
}

[Table("Shop")]
public partial class ShopDetail
{
    [Key]
    [Column("ShopId")]
    public int ShopId { get; set; }

    [Column("ShopCode")]
    public string ShopCode { get; set; }

    [Column("ShopName")]
    public string ShopName { get; set; }

    [Column("MobileNo")]
    public string MobileNum { get; set; }

    [Column("Address")]
    public string Address { get; set; }
}
//[Table("Employed_Staff")]

[Table("Staff")]
public partial class StaffDetail
{
    [Key]
    [Column("StaffId")]
    public int StaffId { get; set; }

    [Column("StaffCode")]
    public string StaffCode { get; set; }

    [Column("StaffName")]
    public string StaffName { get; set; }

    [Column("DateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    [Column("MobileNo")]
    public string MobileNum { get; set; }

    [Column("Address")]
    public string Address { get; set; }

    [Column("Gender")]
    public string Gender { get; set; }

    [Column("Position")]
    public string Position { get; set; }

    [Column("Password")]
    public string Password { get; set; }
}


[Table("SaleInvoice")]
public partial class SaleInvoice
{
    [Key]
    [Column("SaleInvoiceId")]
    public int SaleInvoiceId { get; set; }

    [Column("SaleInvoiceDateTime")]
    public DateTime SaleInvoiceDateTime { get; set; }

    [Column("VoucherNo")]
    public string VoucherNo { get; set; }

    [Column("TotalAmount")]
    public decimal TotalAmount { get; set; }

    [Column("Discount")]
    public decimal DiscountValue { get; set; }

    [Column("StaffCode")]
    public string StaffCode { get; set; }

    [Column("Tax")]
    public decimal TaxAmount { get; set; }

    [Column("PaymentType")]
    public string PaymentType { get; set; }

    [Column("CustomerAccountNo")]
    public string CustomerAccountNum { get; set; }

    [Column("PaymentAmount")]
    public decimal? PaymentAmount { get; set; }

    [Column("ReceiveAmount")]
    public decimal? ReceiveAmount { get; set; }

    [Column("Change")]
    public decimal? ChangeAmount { get; set; }

    [Column("CustomerCode")]
    public string CustomerCode { get; set; }
}


[Table("SaleInvoiceDetail")]
public partial class SaleInvoiceDetail
{
    [Key]
    [Column("SaleInvoiceDetailId")]
    public int SaleInvoiceDetailId { get; set; }

    [Column("VoucherNo")]
    public string VoucherNo { get; set; }

    [Column("ProductCode")]
    public string ProductCode { get; set; }

    [Column("Quantity")]
    public int Quantity { get; set; }

    [Column("Price")]
    public decimal Price { get; set; }

    [Column("Amount")]
    public decimal Amount { get; set; }
}
