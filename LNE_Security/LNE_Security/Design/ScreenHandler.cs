using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHCOOL.UI;

namespace LNE_Security
{
    public class ScreenHandler : Screen
    {
        public Company Company
        {
            get => default;
            set
            {
            }
        }

        public Person Person
        {
            get => default;
            set
            {
            }
        }

        public override string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override void Draw()
        {
            throw new NotImplementedException();
        }
    }
}