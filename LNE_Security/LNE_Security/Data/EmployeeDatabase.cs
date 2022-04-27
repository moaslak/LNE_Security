using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LNE_Security;
using LNE_Security.Data;

namespace LNE_Security;

partial class Database
{
    private Employee employee { get; set; }
    public List<Employee> GetEmployees()
    {
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection("LNE_Security");
        List<Employee> employees = new List<Employee>();

        string query = @"SELECT * FROM [dbo].[Employee]";
        sqlConnection.Open();

        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Employee employee = new Employee();
            employee.EID = Convert.ToUInt16(reader.GetValue(0));
            employee.ContactInfoID = Convert.ToUInt16(reader.GetValue(1));
            employees.Add(employee);
        }
        reader.Close();
        sqlConnection.Close();
        foreach (Employee employee in employees)
        {
            query = "SELECT * FROM [dbo].[ContactInfo] WHERE ContactInfoID = '" + employee.ContactInfoID + "'";
            sqlConnection.Open();
            cmd = new SqlCommand(query, sqlConnection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                employee.ContactInfo.FirstName = reader.GetValue(1).ToString();
                employee.ContactInfo.LastName = reader.GetValue(2).ToString();
                employee.ContactInfo.Email = reader.GetValue(3).ToString();
                employee.ContactInfo.PhoneNumber = reader.GetValue(4).ToString();
                employee.ContactInfo.AddressId = Convert.ToUInt16(reader.GetValue(5));
            }
            reader.Close();
            sqlConnection.Close();

            query = "SELECT * FROM [dbo].[Address] WHERE AddressID = '" + employee.ContactInfo.AddressId.ToString() + "'";
            sqlConnection.Open();
            cmd = new SqlCommand(query, sqlConnection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                employee.ContactInfo.Address.StreetName = reader.GetValue(1).ToString();
                employee.ContactInfo.Address.HouseNumber = reader.GetValue(2).ToString();
                employee.ContactInfo.Address.ZipCode = reader.GetValue(3).ToString();
                employee.ContactInfo.Address.City = reader.GetValue(4).ToString();
                employee.ContactInfo.Address.Country = reader.GetValue(5).ToString();
            }
            reader.Close();
            sqlConnection.Close();
        }

        return employees;
    }

    public Employee SelectEmployee(UInt16 EID)
    {
        List<Employee> employees = GetEmployees();

        foreach (Employee employee in employees)
        {
            if (employee.EID == EID) return employee;

        }
        return null;
    }

    public void EditEmployee(Employee editedEmployee, string option)
    {
        string query = "UPDATE [dbo].[ContactInfo] SET[FirstName] = '" + editedEmployee.ContactInfo.FirstName + "' " +
            ",[LastName] = '" + editedEmployee.ContactInfo.LastName + "', [Email] = '" + editedEmployee.ContactInfo.Email +
            "', [PhoneNumber] = '" + editedEmployee.ContactInfo.PhoneNumber + "' WHERE ContactInfoID = '" + editedEmployee.ContactInfoID + "'";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        query = "UPDATE [dbo].[Address] SET[StreetName] = '" + editedEmployee.ContactInfo.Address.StreetName +
            "', [HouseNumber] = '" + editedEmployee.ContactInfo.Address.HouseNumber +
            "', [City] = '" + editedEmployee.ContactInfo.Address.City +
            "', [ZipCode] = '" + editedEmployee.ContactInfo.Address.ZipCode +
            "', [Country] = '" + editedEmployee.ContactInfo.Address.Country + "' WHERE AddressID = '" + editedEmployee.ContactInfo.AddressId + "'";

        cmd = new SqlCommand(query, sqlConnection);

        //execute the SQLCommand
        reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
    }

    public void DeleteEmployee(UInt16 EID)
    {
        string query = "DELETE FROM [dbo].[Employee] WHERE CID = " + EID.ToString();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();

        if (SelectEmployee(EID) == null)
            Console.WriteLine("Employee with CID = " + EID + " was succesfully deleted");
        else
            Console.WriteLine("Could not find employee to delete");
    }

    public void NewEmployee(Employee employee)
    {

        string query = @"INSERT INTO [dbo].[Employee] 
            (
            [ContactInfoID])
            VALUES('" + employee.ContactInfoID.ToString() + "')";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
    }


    public List<Person> UpdateEmployee(SqlConnection sqlConnection)
    {
        List<Person> people = new List<Person>();
        string query = "UPDATE Emlpoyee(id, firstname, lastname," +
            " phonenumber, email, streetname, housenumber, city, zipcode, country) " +
            "VALUES(@id, @firstname, @lastname, @phonenumber, @email, @streetname," +
            " @housenumber, @city, @zipcode, @country";
        return people;
    }

}
