using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace LNE_Security;

public class Employee : Person
{
    public UInt16 EID { get; set; }
    
    public string UserName { get; set; }
    public string Password { get; set; }
    

    public string GetPassword()
    {
        string pwd = "";
        while (true)
        {
            ConsoleKeyInfo i = Console.ReadKey(true);
            if (i.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            else if (i.Key == ConsoleKey.Backspace)
            {
                if (pwd.Length > 0)
                {
                    pwd = pwd.Remove(pwd.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else if (i.KeyChar != '\u0000') // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
            {
                pwd = pwd + (i.KeyChar).ToString();
                Console.Write("*");
            }
        }
        return pwd;
    }
}
