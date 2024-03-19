using System;
using Npgsql;
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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("select suename as \"Клиент\",the_capt as \"Инвентарь\",tip_number \"Уровень комнаты\",surname as \"Сотрудник\",data_beginn \"Дата приезда\",data_end \"Дата отъезда\",price \"Стоимость\"\r\nfrom services,staff,client,booking,inventory \r\nwhere services.id_staff=staff.id_staff \r\nand services.id_client=client.id_client \r\nand services.id_booking=booking.id_booking \r\nand inventory.id_inventory=services.id_inventory", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                }
                using (var command = new NpgsqlCommand("select name,suename,otchestvo,name_p,clitizenship,telefon from client,level_0f_preparation where  client.id_preparation = level_0f_preparation.id_preparation", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView2.DataSource = table;
                    }
                }
                using (var command = new NpgsqlCommand("select name,surname,otchesto,dolgnost,qualification,telefon from staff where dolgnost = 'Инструктор'", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView4.DataSource = table;
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
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form a1 = new Form1();
            a1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=5958");
            connStr.TrustServerCertificate = true;
            using (var conn = new NpgsqlConnection(connStr.ToString()))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("select suename,the_capt,tip_number,surname,data_beginn,data_end,price from services,staff,client,booking,inventory where services.id_staff=staff.id_staff and services.id_client=client.id_client and services.id_booking=booking.id_booking and inventory.id_inventory=services.id_inventory", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                }
                using (var command = new NpgsqlCommand("select name,suename,otchestvo,name_p,clitizenship,telefon from client,level_0f_preparation where  client.id_preparation = level_0f_preparation.id_preparation", conn))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView2.DataSource = table;
                    }
                }
                using (var command = new NpgsqlCommand("select name,surname,otchesto,dolgnost,qualification,telefon from staff where dolgnost = 'Инструктор'", conn))
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

        private void button1_Click(object sender, EventArgs e)
        {

                string selectedWork1 = comboBox1.SelectedItem.ToString();
                string selectedWork2 = comboBox3.SelectedItem.ToString();
                string selectedWork3 = comboBox4.SelectedItem.ToString();
                var connStr = new NpgsqlConnectionStringBuilder("Server=localhost; Database=skires; Port=5432; User id=postgres; Password=1234");
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
    }
}
