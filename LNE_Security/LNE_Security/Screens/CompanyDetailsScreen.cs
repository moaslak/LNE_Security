using System;
using TECHCOOL.UI;

namespace LNE_Security;
public class CompanyDetailsScreen : ScreenHandler
{
	private Person company { get; set; }
	
	public CompanyDetailsScreen(Person Company) : base(Company)
	{
		this.company = Company;
	}

	protected override void Draw()
    {
		ListPage<Person> CompanylistPage = new ListPage<Person>();
		CompanylistPage.Add(company);

		ListPage<string> SelectedList = new ListPage<string>();

		Title = company.CompanyName + " Company Details";
		Clear(this);

		CompanylistPage.AddColumn("Company name", "CompanyName");
		CompanylistPage.AddColumn("Street name", "StreetName");
		CompanylistPage.AddColumn("House number", "HouseNumber");
		CompanylistPage.AddColumn("Zipcode", "ZipCode");
		CompanylistPage.AddColumn("City", "City");
		CompanylistPage.AddColumn("Country", "Country");
		CompanylistPage.AddColumn("Currency", "Currency");
		Person selected = CompanylistPage.Select();
		
		Console.WriteLine("Selection: " + selected.CompanyName);
		Console.WriteLine("F1 - Back");
		Console.WriteLine("F2 - Edit");
		CompanyScreen companyScreen = new CompanyScreen(selected);
		
		switch (Console.ReadKey().Key)
        {
			case ConsoleKey.F1:
				ScreenHandler.Display(companyScreen);
				break;
			case ConsoleKey.F2:
				Console.WriteLine("NOT IMPLEMENTET");
				break;
			default:
				break;
        }
	}
}
