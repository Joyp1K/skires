using Bogus.DataSets;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace skires
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(/*"select*from services"*/
                    "select suename as \"Клиент\",the_capt as \"Инвентарь\",tip_number \"Уровень комнаты\", surname as \"Сотрудник\",data_beginn \"Дата приезда\",data_end \"Дата отъезда\", price \"Стоимость\"\r\nfrom services,staff,client,booking,inventory \r\nwhere services.id_staff=staff.id_staff \r\nand services.id_client = client.id_client \r\nand services.id_booking = booking.id_booking \r\n and inventory.id_inventory = services.id_inventory", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                }
                using (var command = new NpgsqlCommand("select name \"Имя\",suename \"Фамилия\",otchestvo \"Отчество\",name_p \"Уровень тренера\",clitizenship \"Гражданство\",\r\nseries_passport \"Серия паспорта\",number_passport \"Номер паспорт\",telefon \"Номер телефона\", adrress \"Адрес\"\r\nfrom client,level_0f_preparation\r\nwhere  client.id_preparation = level_0f_preparation.id_preparation", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView2.DataSource = table;
                    }
                }
                using (var command = new NpgsqlCommand("select the_capt \"Снаряжение\", description_of_the_collec \"Коллекция\", size \"Размер\", kol_vo \"Количество\", cost \"Стоимость\"\r\nfrom inventory", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView3.DataSource = table;
                    }
                }
                using (var command = new NpgsqlCommand("select name \"Имя\",surname \"Фамилия\",otchesto \"Отчество\",dolgnost \"Должность\",\r\nqualification \"Квалификация\",telefon \"Номер телефона\",adrress \"Адрес\",zp \"Зарплата\",role \"Логин\",password \"Пароль\"\r\nfrom staff", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView4.DataSource = table;
                    }
                }
                using (var command = new NpgsqlCommand("select tip_number \"Вид номера\", cost \"стоимость ночи\"\r\nfrom booking\r\n", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView5.DataSource = table;
                    }
                }
                using (var command = new NpgsqlCommand("select*from level_0f_preparation", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView6.DataSource = table;
                    }
                }
                using (var command = new NpgsqlCommand("select name_slopes \"Название склона\",name_p \"Уровень допуска\", length \"Стоимость\", height \"Высота\", status \"Статус\"\r\nfrom slopes s\r\njoin level_0f_preparation  l on l.id_preparation = s.id_preparation", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView7.DataSource = table;
                    }
                }
                string q = "select surname from staff where dolgnost = 'Инструктор'";
                NpgsqlCommand com2 = new NpgsqlCommand(q, conn);
                using (NpgsqlDataReader reader = com2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox4.Items.Add(reader.GetString(0));
                    }
                }
                string q1 = "select suename from client";
                NpgsqlCommand com3 = new NpgsqlCommand(q1, conn);
                using (NpgsqlDataReader reader = com3.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader.GetString(0));
                    }
                }
                string q3 = "select tip_number from booking";
                NpgsqlCommand com4 = new NpgsqlCommand(q3, conn);
                using (NpgsqlDataReader reader = com4.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox3.Items.Add(reader.GetString(0));
                    }
                }
                string q4 = "select name_p from level_0f_preparation";
                NpgsqlCommand com5 = new NpgsqlCommand(q4, conn);
                using (NpgsqlDataReader reader = com5.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox5.Items.Add(reader.GetString(0));
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedWork1 = comboBox1.SelectedItem.ToString();
            string selectedWork2 = comboBox3.SelectedItem.ToString();
            string selectedWork3 = comboBox4.SelectedItem.ToString();
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("INSERT INTO services (id_client,id_booking,id_staff,data_beginn,data_end) VALUES ((select id_client from client where suename=@b), (select id_booking from booking where tip_number=@c), (select id_staff from staff where surname=@d),@e, @f)", conn))
                {
                    command.Parameters.AddWithValue("b", selectedWork1);
                    command.Parameters.AddWithValue("c", selectedWork2);
                    command.Parameters.AddWithValue("d", selectedWork3);
                    command.Parameters.AddWithValue("e", maskedTextBox1.Text);
                    command.Parameters.AddWithValue("f", maskedTextBox2.Text);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные все введены.");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(/*"select*from services"*/
                    "select suename as \"Клиент\",the_capt as \"Инвентарь\",tip_number \"Уровень комнаты\", surname as \"Сотрудник\",data_beginn \"Дата приезда\",data_end \"Дата отъезда\", price \"Стоимость\"\r\nfrom services,staff,client,booking,inventory \r\nwhere services.id_staff=staff.id_staff \r\nand services.id_client = client.id_client \r\nand services.id_booking = booking.id_booking \r\n and inventory.id_inventory = services.id_inventory", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string selectedWork1 = comboBox1.SelectedItem.ToString();
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("insert into client(name,suename,otchestvo,id_preparation,citizenship,series_passport,number_passport,telefon,adrress) values (@b,@c,@d,(select id_preparation from client where surname=@a),@e,@f,@g,@h,@i)", conn))
                {
                    command.Parameters.AddWithValue("a", selectedWork1);
                    command.Parameters.AddWithValue("b", textBox1.Text);
                    command.Parameters.AddWithValue("c", textBox2.Text);
                    command.Parameters.AddWithValue("d", textBox3.Text);
                    command.Parameters.AddWithValue("e", textBox4.Text);
                    command.Parameters.AddWithValue("f", textBox5.Text);
                    command.Parameters.AddWithValue("g", textBox6.Text);
                    command.Parameters.AddWithValue("h", Convert.ToDouble(textBox7.Text));
                    command.Parameters.AddWithValue("i", textBox8.Text);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные все введены.");
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("select name \"Имя\",suename \"Фамилия\",otchestvo \"Отчество\",name_p \"Уровень тренера\",clitizenship \"Гражданство\",\r\nseries_passport \"Серия паспорта\",number_passport \"Номер паспорт\",telefon \"Номер телефона\", adrress \"Адрес\"\r\nfrom client,level_0f_preparation\r\nwhere  client.id_preparation = level_0f_preparation.id_preparation", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView2.DataSource = table;
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("insert into inventory(the_capt,description_of_the_collec,size,kol_vo,cost) values (@a,@b,@c,@d,@e)", conn))
                {
                    command.Parameters.AddWithValue("a", textBox9.Text);
                    command.Parameters.AddWithValue("b", textBox10.Text);
                    command.Parameters.AddWithValue("c", textBox11.Text);
                    command.Parameters.AddWithValue("d", Convert.ToInt32(textBox12.Text));
                    command.Parameters.AddWithValue("e", Convert.ToDouble(textBox13.Text));

                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные все введены.");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("select the_capt \"Снаряжение\", description_of_the_collec \"Коллекция\", size \"Размер\", kol_vo \"Количество\", cost \"Стоимость\" from inventory", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView3.DataSource = table;
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("insert into staff(name,surname,otchesto,dolgnost,qualification,telefon,adrress,zp,role,password) values (@a,@b,@c,@d,@e,@f,@g,@h)", conn))
                {
                    command.Parameters.AddWithValue("a", textBox14.Text);
                    command.Parameters.AddWithValue("b", textBox15.Text);
                    command.Parameters.AddWithValue("c", textBox16.Text);
                    command.Parameters.AddWithValue("d", textBox17.Text);
                    command.Parameters.AddWithValue("e", textBox18.Text);
                    command.Parameters.AddWithValue("f", Convert.ToInt32(textBox19.Text));
                    command.Parameters.AddWithValue("g", textBox20.Text);
                    command.Parameters.AddWithValue("h", Convert.ToDouble(textBox21.Text));

                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные все введены.");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("select name \"Имя\",surname \"Фамилия\",otchesto \"Отчество\",dolgnost \"Должность\",\r\nqualification \"Квалификация\",telefon \"Номер телефона\",adrress \"Адрес\",zp \"Зарплата\",role \"Логин\",password \"Пароль\"\r\nfrom staff", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView4.DataSource = table;
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("insert into booking(tip_number,cost) values (@a,@b)", conn))
                {
                    command.Parameters.AddWithValue("a", textBox22.Text);
                    command.Parameters.AddWithValue("b", Convert.ToDouble(textBox23.Text));

                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные все введены.");
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                using (var command = new NpgsqlCommand("select tip_number \"Вид номера\", cost \"стоимость ночи\"\r\nfrom booking\r\n", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView5.DataSource = table;
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("insert into level_0f_preparation(name_p) values(@a)", conn))
                {
                    command.Parameters.AddWithValue("a", textBox24.Text);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные все введены.");
                }
            }            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("select*from level_0f_preparation", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView6.DataSource = table;
                    }
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string selectedWork5 = comboBox5.SelectedItem.ToString();
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("insert into slopes(name_slopes,id_preparation,length,height,status) values(@a,(select id_preparation from level_0f_preparation where name_p=@c),@b,@d,@e)", conn))
                {
                    command.Parameters.AddWithValue("a", textBox25.Text);
                    command.Parameters.AddWithValue("b", Convert.ToDouble(textBox26.Text));
                    command.Parameters.AddWithValue("c", selectedWork5);
                    command.Parameters.AddWithValue("d", Convert.ToDouble(textBox27.Text));
                    command.Parameters.AddWithValue("e", (textBox28.Text));

                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные все введены.");
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("select name_slopes \"Название склона\",name_p \"Уровень допуска\", length \"Стоимость\", height \"Высота\", status \"Статус\"\r\nfrom slopes s\r\njoin level_0f_preparation  l on l.id_preparation = s.id_preparation", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView7.DataSource = table;
                    }
                }
            }
        }
    }
}
