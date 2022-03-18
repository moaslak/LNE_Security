using System;
using TECHCOOL;

namespace LNE_Security
{
    class Program
    {
        public static void Main(string[] args)
        {
            TECHCOOL.UI.Menu menu = new TECHCOOL.UI.Menu();
            TECHCOOL.UI.Screen screen;
            
            menu.Draw();
            Console.ReadKey();
        }
    }
}