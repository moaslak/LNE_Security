using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LNE_Security.Screens.EditCompnayScreen;

namespace LNE_Security.Screens;

public class EditProductScreen : ScreenHandler
{
    private Options options;
    private Product product;
    public EditProductScreen(Product Product, Options options) : base(Product)
    {
        this.options = options;
        this.product = Product;
    }

    string newValue = "";

     if (newValue == null)
            newValue = "";
        switch (selected.Option)
        {
            case "Product name":
                this.product.ProductName = newValue;
                break;
            case "Street name":
                this.company.StreetName = newValue;
                break;
            case "House number":
                this.company.HouseNumber = newValue;
                break;
            case "Zip code":
                this.company.ZipCode = newValue;
                break;
            case "City":
                this.company.City = newValue;
                break;
            case "Country": 
                this.company.Country = newValue;
                break;
            default:
                break;
        }
