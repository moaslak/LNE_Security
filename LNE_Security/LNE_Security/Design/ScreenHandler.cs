using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHCOOL.UI;

namespace LNE_Security
{
    
    public class ScreenHandler : Screen
    {
        static Company company { get; set; }
        public ScreenHandler(Company Company)
        {
            company = Company;
        }
        public Person Person { get; set; }

        
        public override string Title { get; set; }

        protected override void Draw()
        {
            
        }

        public Company Company
        {
            get => default;
            set
            {
            }
        }
    }
}