using LNE_Security.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

public class LoginScreen
{
    private LNE_Security.Person.UserLogins userLogins;
    public LoginScreen(Person user) : base(user)
    {
        this.userLogins = user;
    }

    protected override void Draw()
    {
        ListPage<LNE_Security.Person.UserLogins> userListPage = new ListPage<LNE_Security.Person.UserLogins>();
    }
}
