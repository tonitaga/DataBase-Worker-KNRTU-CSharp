using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using DBCSharpUsage.DBModel.DBHelper;

namespace DBCSharpUsage
{
    public partial class DBWorkForm : Form
    {
        private DBController _controller = null;
        public DBWorkForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            _controller = new DBController();
        }

        private void DBWorkForm_Load(object sender, EventArgs e)
        {
            var connection_result = _controller.ConnectToDataBase("ConnectionKeyDB");
            if (connection_result == DBInfo.DBConnectionError)
            {
                MessageBoxErrorShow();
                return;
            }
            BindComboBox(DBQueryType.DELETE);
            BindComboBox(DBQueryType.INSERT);
            BindComboBox(DBQueryType.UPDATE);

        }

        private void insertDataBaseButton_Click(object sender, EventArgs e)
        {
            if (_controller.GetCurrentDBConnectionState() == DBInfo.DBConnectionError)
            {
                MessageBoxErrorShow();
                return;
            }
            var database_info = _controller.InsertToDataBase(GetDataForInsert());
            MessageBoxInformationShow(database_info);
            ToStartState(DBQueryType.INSERT);
            UpdateHistory();
        }

        private void removeDataBaseButton_Click(object sender, EventArgs e)
        {
            if (_controller.GetCurrentDBConnectionState() == DBInfo.DBConnectionError)
            {
                MessageBoxErrorShow();
                return;
            }
            var database_info = _controller.RemoveFromDataBase(GetDataForRemove());
            MessageBoxInformationShow(database_info);
            ToStartState(DBQueryType.DELETE);
            UpdateHistory();
        }

        private void updateDataBaseButton_Click(object sender, EventArgs e)
        {
            if (_controller.GetCurrentDBConnectionState() == DBInfo.DBConnectionError)
            {
                MessageBoxErrorShow();
                return;
            }
            var database_info = _controller.UpdateTheDataBase(GetDataForUpdateFrom(), GetDataForUpdateTo());
            MessageBoxInformationShow(database_info);
            ToStartState(DBQueryType.UPDATE);
            UpdateHistory();
        }

        private void BindComboBox(DBQueryType query)
        {
            if (query == DBQueryType.INSERT)
            {
                fucltiesComboBox.SelectedIndex = 0;
                departmentsComboBox.SelectedIndex = 0;
                educationFormComboBox.SelectedIndex = 0;
                educationsСomboBox.SelectedIndex = 0;
                studentStatusesComboBox.SelectedIndex = 0;
            } else if (query == DBQueryType.DELETE)
            {
                fucltiesComboBoxR.SelectedIndex = 0;
                departmentsComboBoxR.SelectedIndex = 0;
                educationFormComboBoxR.SelectedIndex = 0;
                educationsСomboBoxR.SelectedIndex = 0;
                studentStatusesComboBoxR.SelectedIndex = 0;
            } else
            {
                fucltiesComboBoxUF.SelectedIndex = 0;
                departmentsComboBoxUF.SelectedIndex = 0;
                educationFormComboBoxUF.SelectedIndex = 0;
                educationsСomboBoxUF.SelectedIndex = 0;
                studentStatusesComboBoxUF.SelectedIndex = 0;

                fucltiesComboBoxUT.SelectedIndex = 0;
                departmentsComboBoxUT.SelectedIndex = 0;
                educationFormComboBoxUT.SelectedIndex = 0;
                educationsСomboBoxUT.SelectedIndex = 0;
                studentStatusesComboBoxUT.SelectedIndex = 0;
            }
        }

        private void ToStartState(DBQueryType query)
        {
            if (query == DBQueryType.INSERT)
            {
                nameTextBox.Text = string.Empty;
                surnameTextBox.Text = string.Empty;
                lastnameTextBox.Text = string.Empty;
                groupNumtextBox.Text = string.Empty;
                BindComboBox(DBQueryType.INSERT);
            } else if (query == DBQueryType.DELETE)
            {
                nameTextBoxR.Text = string.Empty;
                surnameTextBoxR.Text = string.Empty;
                lastnameTextBoxR.Text = string.Empty;
                groupNumTextBoxR.Text = string.Empty;
                BindComboBox(DBQueryType.DELETE);
            } else
            {
                nameTextBoxUF.Text = string.Empty;
                surnameTextBoxUF.Text = string.Empty;
                lastnameTextBoxUF.Text = string.Empty;
                groupNumTextBoxUF.Text = string.Empty;
                nameTextBoxUT.Text = string.Empty;
                surnameTextBoxUT.Text = string.Empty;
                lastnameTextBoxUT.Text = string.Empty;
                groupNumTextBoxUT.Text = string.Empty;
                BindComboBox(DBQueryType.UPDATE);
            }

        }

        private DBMainTableItem GetDataForInsert()
        {
            return new DBMainTableItem(
                nameTextBox.Text,
                surnameTextBox.Text,
                lastnameTextBox.Text,
                birthdayDateTimePicker.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                fucltiesComboBox.SelectedItem.ToString(),
                groupNumtextBox.Text,
                departmentsComboBox.SelectedItem.ToString(),
                educationFormComboBox.SelectedItem.ToString(),
                educationsСomboBox.SelectedItem.ToString(),
                studentStatusesComboBox.SelectedItem.ToString()
            );
        }

        private DBMainTableItem GetDataForRemove()
        {
            return new DBMainTableItem(
                nameTextBoxR.Text,
                surnameTextBoxR.Text,
                lastnameTextBoxR.Text,
                birthdayDateTimePickerR.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                fucltiesComboBoxR.SelectedItem.ToString(),
                groupNumTextBoxR.Text,
                departmentsComboBoxR.SelectedItem.ToString(),
                educationFormComboBoxR.SelectedItem.ToString(),
                educationsСomboBoxR.SelectedItem.ToString(),
                studentStatusesComboBoxR.SelectedItem.ToString()
            );
        }

        private DBMainTableItem GetDataForUpdateFrom()
        {
            return new DBMainTableItem(
                nameTextBoxUF.Text,
                surnameTextBoxUF.Text,
                lastnameTextBoxUF.Text,
                birthdayDateTimePickerUF.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                fucltiesComboBoxUF.SelectedItem.ToString(),
                groupNumTextBoxUF.Text,
                departmentsComboBoxUF.SelectedItem.ToString(),
                educationFormComboBoxUF.SelectedItem.ToString(),
                educationsСomboBoxUF.SelectedItem.ToString(),
                studentStatusesComboBoxUF.SelectedItem.ToString()
            );
        }

        private DBMainTableItem GetDataForUpdateTo()
        {
            return new DBMainTableItem(
                nameTextBoxUT.Text,
                surnameTextBoxUT.Text,
                lastnameTextBoxUT.Text,
                birthdayDateTimePickerUT.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                fucltiesComboBoxUT.SelectedItem.ToString(),
                groupNumTextBoxUT.Text,
                departmentsComboBoxUT.SelectedItem.ToString(),
                educationFormComboBoxUT.SelectedItem.ToString(),
                educationsСomboBoxUT.SelectedItem.ToString(),
                studentStatusesComboBoxUT.SelectedItem.ToString()
            );
        }

        private void UpdateHistory()
        {
            var query = _controller.GetLastAddedHistory();
            if (query.Key != null && query.Value != null)
            {
                historyDataGridView.Rows.Add(query.Key, query.Value);
            }
        }

        private void MessageBoxErrorShow()
        {
            MessageBox.Show(_controller.GetDBInfoString(DBInfo.DBConnectionError), "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MessageBoxInformationShow(DBInfo info)
        {
            MessageBox.Show(_controller.GetDBInfoString(info), "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
