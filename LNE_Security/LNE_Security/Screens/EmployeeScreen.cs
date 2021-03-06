using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using LNE_Security;
using System.Security;

namespace LNE_Security.Screens;

public class EmployeeScreen : ScreenHandler
{

    ContactInfo contact { get; set; }
    Address address { get; set; }

    private Employee Employee = new Employee();
    public EmployeeScreen(Employee employee) : base(employee)
    {
        this.Employee = employee;
    }

    public EmployeeScreen()
    {
    }
    private Company company { get; set; }
    public EmployeeScreen(Company Company) : base(Company)
    {
        this.company = Company;
    }

    public void newEmployee()
    {
        Address address = new Address();
        ContactInfo contactInfo = new ContactInfo();
        Database database = new Database();
        Console.WriteLine("New Employee");
        Console.WriteLine();
        Console.Write("Enter first name: ");
        contactInfo.FirstName = Console.ReadLine();
        Console.Write("Enter last name: ");
        contactInfo.LastName = Console.ReadLine();
        Console.Write("Enter email: ");
        contactInfo.Email = Console.ReadLine();
        Console.Write("Enter phonenumber: ");
        contactInfo.PhoneNumber = Console.ReadLine();

        Console.Write("Enter street name: ");
        contactInfo.Address.StreetName = Console.ReadLine();
        Console.Write("Enter house number: ");
        contactInfo.Address.HouseNumber = Console.ReadLine();
        Console.Write("Enter zip code: ");
        contactInfo.Address.ZipCode = Console.ReadLine();
        Console.Write("Enter city: ");
        contactInfo.Address.City = Console.ReadLine();
        Console.Write("Enter country: ");
        contactInfo.Address.Country = Console.ReadLine();
        contactInfo.AddressId = Database.Instance.NewAddress(contactInfo.Address);

        Employee newEmployee = new Employee();
        newEmployee.CompanyID = company.CompanyID;
        Console.Write("Enter user name: ");
        newEmployee.UserName = Console.ReadLine();
        Console.Write("Enter password: ");
        newEmployee.Password = newEmployee.GetPassword().ToString();
        int passwordCheck = 0;
        bool passwordConfirmed = false;
        do
        {
            Console.Write("Confirm password: ");
            if (newEmployee.Password == newEmployee.GetPassword())
                passwordConfirmed = true;
            else
            {
                Console.WriteLine("Incorrect confirmation");
                passwordCheck++;
                if (passwordCheck == 3)
                    passwordConfirmed = true;
            }

        } while (!(passwordConfirmed));
        if (passwordCheck == 3 && passwordConfirmed)
        {
            Console.WriteLine("Password no comfirmed!!!");
            Console.WriteLine("Password set to: Test!234");
            newEmployee.Password = "Test!234";
        }

        newEmployee.ContactInfoID = Database.Instance.NewContactInfo(contactInfo);
        newEmployee.ContactInfo = contactInfo;
        newEmployee.Address = contactInfo.Address;
        Database.Instance.NewEmployee(newEmployee);
    }
    private void viewCustomer(Employee employee)
    {
        Console.Write("Name: " + employee.FullName + "\n");
        Console.Write("Address: " + employee.ContactInfo.Address.AddressLine(employee));
    }

    protected override void Draw()
    {
        ListPage<Employee> EmployeeListPage = new ListPage<Employee>();
        ListPage<ContactInfo> ContactListPage = new ListPage<ContactInfo>();
        List<Employee> employees = Database.Instance.GetEmployees(company.CompanyID);

        int maxFullnameLength = 0;
        int maxEmailLength = 0;
        int maxPhoneNumberLength = 0;
        int maxEIDLength = 0;
        int maxUserNameLength = 0;

        foreach (Employee employee in employees)
        {
            try
            {
                employee.FullName = employee.ContactInfo.FullName;
                employee.Email = employee.ContactInfo.Email;
                employee.PhoneNumber = employee.ContactInfo.PhoneNumber;
                EmployeeListPage.Add(employee);
                if (employee.FullName.Length > maxFullnameLength)
                    maxFullnameLength = employee.FullName.Length;
                if (employee.Email.Length > maxEmailLength)
                    maxEmailLength = employee.Email.Length;
                if (employee.PhoneNumber.Length > maxPhoneNumberLength)
                    maxPhoneNumberLength = employee.PhoneNumber.Length;
                if(employee.EID.ToString().Length > maxEIDLength)
                    maxEIDLength = employee.EID.ToString().Length;
                if(employee.UserName.ToString().Length > maxUserNameLength)
                    maxUserNameLength = employee.UserName.ToString().Length;
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //string name = $"{Customer.ContactInfo.FirstName} {Customer.ContactInfo.LastName}";
        //Title = Customer.ContactInfo.FullName + " Customer name";
        Title = "Employee screen";
        Clear(this);
        Employee selected = new Employee();
        if (employees.Count != 0)
        {
            Console.WriteLine("Choose Employee");
            EmployeeListPage.AddColumn("Employee ID", "EID", ColumnLength("Employee ID", maxEIDLength));
            EmployeeListPage.AddColumn("Employee name", "FullName", ColumnLength("Employee name", maxFullnameLength));
            EmployeeListPage.AddColumn("Phone number", "PhoneNumber", ColumnLength("Phone number", maxPhoneNumberLength));
            EmployeeListPage.AddColumn("Email", "Email", ColumnLength("Email", maxEmailLength));
            EmployeeListPage.AddColumn("User name", "UserName", ColumnLength("User name", maxUserNameLength));
            
            selected = EmployeeListPage.Select();
            
        }
        if(selected != null)
        {
            Console.WriteLine("Selection: " + selected.ContactInfo.FullName);
            Console.WriteLine("F1 - New Employee");
            Console.WriteLine("F2 - View/Edit Employee");
            Console.WriteLine("F8 - Delete Employee");
            Console.WriteLine("F10 - To Main menu");
            Console.WriteLine();

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.F1:
                    newEmployee();
                    Console.WriteLine("Press enter to continue");
                    break;
                case ConsoleKey.F2:
                    ScreenHandler.Display(new EditEmployeeScreen(selected, company));
                    break;
                case ConsoleKey.F10:
                    ScreenHandler.Display(new MainMenuScreen(this.company));
                    break;
                case ConsoleKey.F8:
                    
                    Database.Instance.DeleteEmployee(selected.EID);
                    Database.Instance.DeleteContactInfo(selected.ContactInfoID);
                    break;
            }
        }
        
    }
}
