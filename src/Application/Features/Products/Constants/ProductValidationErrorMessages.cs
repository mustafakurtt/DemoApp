namespace Application.Features.Products.Constants;

public class ProductValidationErrorMessages
{
    public const string ProductNameCanNotBeEmpty = "Product name can not be empty";
    public const string ProductNameMustBeAtLeast2Characters = "Product name must be at least 2 characters";
    public const string ProductPriceCanNotBeEmpty = "Product price can not be empty";
    public const string ProductPriceMustBeGreaterThan0 = "Product price must be greater than 0";
    public const string ProductCategoryIdCanNotBeEmpty = "Product category id can not be empty";
    public const string ProductCategoryIdMustBeGreaterThan0 = "Product category id must be greater than 0";
    public const string ProductSupplierIdCanNotBeEmpty = "Product supplier id can not be empty";
    public const string ProductSupplierIdMustBeGreaterThan0 = "Product supplier id must be greater than 0";
    public const string ProductIdCanNotBeEmpty = "Product id can not be empty";
    public const string ProductIdMustBeGreaterThan0 = "Product id must be greater than 0";
}