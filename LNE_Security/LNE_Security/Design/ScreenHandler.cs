using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHCOOL.UI;

namespace LNE_Security;

public class ScreenHandler : Screen
{
    static Company company { get; set; }
    static Product product { get; set; }
    public ScreenHandler(Company Company)
    {
        company = Company;
    }
    public ScreenHandler(Product Product)
    {
        product = Product;
    }
    public Person Person { get; set; }
    
    
    public override string Title { get; set; }

    protected override void Draw()
    {
        
    }

    protected static void Display(Company company)
    {
        throw new NotImplementedException();
    }
}
