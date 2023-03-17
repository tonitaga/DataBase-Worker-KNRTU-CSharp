using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCSharpUsage
{
    internal class DBMainTableItem
    {
        public string name = "";
        public string surname = "";
        public string lastname = "";
        public string birthday = "";
        public string faccode = "";
        public string groupid = "";
        public string otdelcode = "";
        public string obuchcode = "";
        public string obrcode = "";
        public string statuscode = "";

        public DBMainTableItem() { }
        public DBMainTableItem(string name, string surname, string lastname, string birthday, string faccode, string groupid, string otdelcode, string obuchcode, string obrcode, string statuscode)
        {
            this.name = name;
            this.surname = surname;
            this.lastname = lastname;
            this.birthday = birthday;
            this.faccode = faccode;
            this.groupid = groupid;
            this.otdelcode = otdelcode;
            this.obuchcode = obuchcode;
            this.obrcode = obrcode;
            this.statuscode = statuscode;
        }
    }
}
