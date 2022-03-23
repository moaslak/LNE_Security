using System;

namespace LNE_Security;
public class CompanyDetails : ScreenHandler
{
	static string BACK = "Back";
	private Company company { get; set; }
	public CompanyDetails(Company Company) : base(company)
	{
		this.company = Company;
	}

	public override void Draw()
    {
		ListPage<Company> CompanylistPage = new ListPage<Company>();
		listPage<String> SelectedList = new listPage<string>();
		CompanyScreen companyScreen = new CompanyScreen();
		CompanylistPage.Show(Company);
		SelectedList.Add(BACK);
		Company selected;
		do
		{
			Title = "Company Details";
			Clear(companyScreen());
			CompanylistPage.AddColumn("Company name", "CompanyName");
			CompanyListPage.AddColumn("Street name", "StreetName");
			CompanyListPage.AddColumn("House number", "HouseNumber");
			CompanyListPage.AddColumn("Zipcode", "ZipCode");
			CompanyListPage.AddColumn("City", "City");
			CompanyListPage.AddColumn("Country", "Country");
			CompanyListPage.AddColumn("Currency", "Currency");
		} while (!(Console.ReadKey().Key == Console.Escape));

        switch (company.CompanyType)
        {
			case "LNE_Security":
				Console.WriteLine("Company Details" + selected.Company);
				break;
			case "BACK":
				Environment.Exit(0);
        }

		
	}
}
