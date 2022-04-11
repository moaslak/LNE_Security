using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LNE_Security;

partial class Database
{
    public void newPerson(Person.Types type)
    {
        string query = "INSERT INTO [dbo].[Person] ([Type])";
        switch (type)
        {
            case Person.Types.Customer:
                query = query + " VALUES(Customer)";
                break;
            case Person.Types.Employee:
                query = query + " VALUES(Employee)";
                break;
            default:
                break;
        }
        
    }
}
