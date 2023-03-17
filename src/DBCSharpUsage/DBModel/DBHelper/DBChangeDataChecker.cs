using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCSharpUsage
{
    internal class DBChangeDataChecker
    {
        private static DBChangeDataChecker _checker = null;
        private DBMainTableItem _tableItem;

        private DBChangeDataChecker() {}

        public static DBChangeDataChecker GetChecker(DBMainTableItem item)
        {
            if (_checker == null)
            {
                _checker = new DBChangeDataChecker();
            }
            _checker._tableItem = item;
            return _checker;
        }

        public bool IsCorrect()
        {
            if (_tableItem.name == null || _tableItem.name.Length == 0) return false;
            else if (_tableItem.surname == null || _tableItem.surname.Length == 0) return false;
            else if (_tableItem.lastname == null || _tableItem.lastname.Length == 0) return false;
            else if (_tableItem.groupid == null || _tableItem.groupid.Length != 4) { return false; }

            string firstDigitInGroupnum = _tableItem.groupid.Substring(0, 1);
            if (firstDigitInGroupnum == "1" && _tableItem.faccode != "Институт авиации, наземного транспорта и энергетики") return false;
            else if (firstDigitInGroupnum == "2" && _tableItem.faccode != "Факультет физико-математический") { return false; }
            else if (firstDigitInGroupnum == "3" && _tableItem.faccode != "Институт автоматики и электронного приборостроения") return false;
            else if (firstDigitInGroupnum == "4" && _tableItem.faccode != "Институт компьютерных технологий и защиты информации") { return false; }
            else if (firstDigitInGroupnum == "5" && _tableItem.faccode != "Институт радиоэлектроники, фотоники и цифровых технологий") return false;

            if (!int.TryParse(_tableItem.groupid, out _)) return false;
            int group = int.Parse(_tableItem.groupid);
            if (group < 0) return false;
            int faculty = int.Parse(_tableItem.groupid[0].ToString());
            if (faculty < 1 || faculty > 5) return false;
            int course = int.Parse(_tableItem.groupid[1].ToString());
            if (course < 1 || course > 5) return false;
            foreach (var item in _tableItem.surname) { if (char.IsDigit(item)) return false; }
            foreach (var item in _tableItem.name) { if (char.IsDigit(item)) return false; }
            foreach (var item in _tableItem.lastname) { if (char.IsDigit(item)) return false; }

            return true;
        }
    }
}
