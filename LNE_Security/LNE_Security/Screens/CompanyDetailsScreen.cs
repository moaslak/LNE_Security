using System;
using TECHCOOL.UI;

namespace LNE_Security;
public class CompanyDetailsScreen : ScreenHandler
{
	private Company company { get; set; }
	
	public CompanyDetailsScreen(Company Company) : base(Company)
	{
		this.company = Company;
	}

	/// <summary>
	/// Displays the screen
	/// </summary>
	protected override void Draw()
    {
		ListPage<Company> CompanylistPage = new ListPage<Company>();
		CompanylistPage.Add(company);

		ListPage<string> SelectedList = new ListPage<string>();

		Title = company.CompanyName + " Company Details";
		Clear(this);

		CompanylistPage.AddColumn("Company name", "CompanyName", company.CompanyName.Length);
		CompanylistPage.AddColumn("Street name", "StreetName", company.StreetName.Length);
		CompanylistPage.AddColumn("House number", "HouseNumber", company.HouseNumber.Length);
		CompanylistPage.AddColumn("Zipcode", "ZipCode", company.ZipCode.Length);
		CompanylistPage.AddColumn("City", "City",company.City.Length);
		CompanylistPage.AddColumn("Country", "Country", company.Country.Length);
		CompanylistPage.AddColumn("Currency", "Currency",company.Currency.ToString().Length);
		Company selected = CompanylistPage.Select();
		
		Console.WriteLine("Selection: " + selected.CompanyName);
		Console.WriteLine("F1 - Back");
		Console.WriteLine("F2 - Edit");
		Console.WriteLine("F5 - Delete");
		CompanyScreen companyScreen = new CompanyScreen(selected);
		
		switch (Console.ReadKey().Key)
        {
			case ConsoleKey.F1:
				ScreenHandler.Display(companyScreen);
				break;
			case ConsoleKey.F2:
				Console.WriteLine("NOT IMPLEMENTET");
				break;
			case ConsoleKey.F5:
				Database.Instance.DeleteCompany(selected.CompanyID);
				break;
			default:
				break;
        }
	}
}
