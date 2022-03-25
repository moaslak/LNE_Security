﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens
{
    
    public class MainMenuScreen : ScreenHandler
    {
        class Options
        {
            public string Option { get; set; }
            public string KeyPress { get; set; }
            public Options(string option, string keyPress)
            {
                KeyPress = keyPress;
                Option = option;

            }
        }
        private Company company { get; set; }

        
        public MainMenuScreen(Company Company) : base(Company)
        {
            this.company = Company;
        }

        protected override void Draw()
        {
            Title = "ERP System";
            Clear(this);

            ListPage<Options> MenuOptions = new ListPage<Options>();
            MenuOptions.AddColumn("Option", "Option");
            MenuOptions.Add(new Options("Company screen", "F1"));
            MenuOptions.Add(new Options("Customer screen", "F2"));
            MenuOptions.Add(new Options("Employee screen", "F3"));
            MenuOptions.Add(new Options("Storage screen", "F4"));
            MenuOptions.Add(new Options("Close App", "ESC"));

            Options selected = MenuOptions.Select();

            switch (selected.KeyPress)
            {
                case "F1":
                    CompanyScreen companyScreen = new CompanyScreen(company);
                    ScreenHandler.Display(companyScreen);
                    break;
                case "F2":
                    Console.WriteLine("NOT IMPLEMENTET");
                    break;
                case "F3":
                    Console.WriteLine("NOT IMPLEMENTET");
                    break;
                case "F4":
                    Console.WriteLine("NOT IMPLEMENTET");
                    break;
                case "ESC":
                    Environment.Exit(0);
                    break;
            }
            

        }
    }
}
