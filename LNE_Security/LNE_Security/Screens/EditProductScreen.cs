using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using static LNE_Security.Screens.EditCompnayScreen;

namespace LNE_Security;

public class EditProductScreen : ScreenHandler
{
    private Product product;

    private void EditProduct(Options selected)
    {
        string newValue = "";


        if (newValue == null)
            newValue = "";
        switch (selected.Option)
        {
            case "Product Number":
                this.product.ProductNumber = newValue;
                break;
            case "Product name":
                this.product.ProductName = newValue;
                break;
            case "Description":
                this.product.Description = newValue;
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
    }
}