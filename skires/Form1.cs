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
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace skires
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=skires;Username=postgres;Password=5958";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string password = textBox2.Text;
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT dolgnost FROM staff WHERE role = @username AND password = @password";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", userName);
                    command.Parameters.AddWithValue("@password", password);
                                        
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string dolgnost = reader.GetString(0);

                            switch (dolgnost)
                            {
                                case "Администратор":
                                    Form2 f2 = new Form2();
                                    f2.Show();
                                    this.Hide();
                                    break;

                                case "Менеджер":
                                    Form3 f3 = new Form3();
                                    f3.Show();
                                    this.Hide();
                                    break;

                                case "Руководитель":
                                    Form4 f4 = new Form4();
                                    f4.Show();
                                    this.Hide();
                                    break;
                                case "Прокат":
                                    Form5 f5 = new Form5();
                                    f5.Show();
                                    this.Hide();
                                    break;

                                default:
                                    MessageBox.Show("Траблы.");
                                    break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль.");
                        }
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}