using System;
using TECHCOOL.UI;

namespace LNE_Security;
public class CompanyDetails : ScreenHandler
{
	static string BACK = "Back";
	private Company company;
	private CompanyScreen companyScreen { get; set; }
	public CompanyDetails(Company Company) : base(Company)
	{
		this.company = Company;
	}

	protected override void Draw()
    {
		ListPage<CompanyScreen> CompanylistPage = new ListPage<CompanyScreen>();
		ListPage<string> SelectedList = new ListPage<string>();
		CompanylistPage.Add(companyScreen);

		do
		{
			Title = "Company Details";
			Clear(this);
			CompanylistPage.AddColumn("Company name", "CompanyName");
			CompanylistPage.AddColumn("Street name", "StreetName");
			CompanylistPage.AddColumn("House number", "HouseNumber");
			CompanylistPage.AddColumn("Zipcode", "ZipCode");
			CompanylistPage.AddColumn("City", "City");
			CompanylistPage.AddColumn("Country", "Country");
			CompanylistPage.AddColumn("Currency", "Currency");
		} while (!(Console.ReadKey().Key == ConsoleKey.Escape));
		
		
	}
}
