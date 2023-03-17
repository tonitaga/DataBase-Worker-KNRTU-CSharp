using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBCSharpUsage.DBView
{
    public partial class DBLogin : Form
    {
        DBController _controller = null;
        public DBLogin()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            _controller = new DBController();
        }

        private void DBLogin_Load(object sender, EventArgs e)
        {
            var connection_result = _controller.ConnectToDataBase("LoginConnectionKeyDB");
            if (connection_result == DBInfo.DBConnectionError)
            {
                MessageBox.Show(_controller.GetDBInfoString(DBInfo.DBConnectionError), "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            passwordTextBox.UseSystemPasswordChar = false;
            passwordTextBox.PasswordChar = '●';
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (_controller.GetCurrentDBConnectionState() == DBInfo.DBConnectionError)
            {
                MessageBox.Show(_controller.GetDBInfoString(DBInfo.DBConnectionError), "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var login = loginTextBox.Text;
            var password = passwordTextBox.Text;

            var login_result = _controller.SelectFromDataBase($"SELECT * FROM Register WHERE Login = '{login}' AND Password = '{password}'");
            MessageBox.Show(_controller.GetDBInfoString(login_result), "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (login_result == DBInfo.DBUserLoginSuccess)
            {
                DBWorkForm workForm = new DBWorkForm();
                this.Hide();
                workForm.ShowDialog();
                this.Close();
            }

            ClearTextBoxes();
        }

        private void ClearTextBoxes()
        {
            loginTextBox.Text = string.Empty;
            passwordTextBox.Text = string.Empty;
        }
    }
}
