using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class Location
    {
        public char Section { get; set; }
        
        public byte Shelve { get; set; }

        public UInt16 Row { get; set; }

        public Location(char section, byte shelve, UInt16 Row)
        {
            this.Section = section;
            this.Shelve = shelve;
            this.Row = Row;
        }

        public Location()
        {
        }

        public string Location2String(Location location)
        {
            return (location.Row.ToString() + location.Section.ToString() + location.Shelve.ToString());
        }
    }
}