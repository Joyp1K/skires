using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace skires
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=***;Username=postgres;Password=***";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string password = textBox2.Text;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT *** FROM *** WHERE *** = @username AND *** = @password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", userName);
                    command.Parameters.AddWithValue("@password", password);

                    string userType = command.ExecuteScalar().ToString();
            switch (userType)
                    {
                        case "*":
                            Form2 a1 = new Form2();
                            a1.Show();
                            break;
                        case "**":
                            Form3 a2 = new Form3();
                            a2.Show();
                            break;
                        case "***":
                            Form4 a3 = new Form4();
                            a3.Show();
                            break;
                        case "****":
                            Form5 a4 = new Form5();
                            a4.Show();
                            break;
                        default:
                            MessageBox.Show("Неверный логин или пароль");
                            break;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
