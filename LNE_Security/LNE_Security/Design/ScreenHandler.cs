using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHCOOL.UI;

namespace LNE_Security
{
    public class ScreenHandler : Screen
    {
        public Company Company { get; set; }
        

        public Person Person { get; set; }


        public override string Title { get; set; }

        protected override void Draw()
        {
            Title = Company.CompanyName;
            Clear(this);
            Console.WriteLine("Screen");
        }
    }
}