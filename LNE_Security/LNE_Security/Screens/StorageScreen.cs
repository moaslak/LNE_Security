using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNE_Security.Screens;

    public class StorageScreen : ScreenHandler
    {

	private Person company;
	public StorageScreen(Person Company) : base(Company)
	{
		this.company = Company;
	}
}
