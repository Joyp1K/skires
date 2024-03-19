using Bogus;
using GSF.Adapters;
using GSF.Units;
using Microsoft.Office.Interop.Word;
using Npgsql;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NPOI.HSSF.Util.HSSFColor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = Microsoft.Office.Interop.Word.Application;
using DataTable = System.Data.DataTable;
using Document = Microsoft.Office.Interop.Word.Document;
using Paragraph = Microsoft.Office.Interop.Word.Paragraph;
using Table = Microsoft.Office.Interop.Word.Table;


namespace skires
{
    public partial class Form4 : Form
    {
        string ConnectionString = " Host=localhost;Port=5432;Database=skires;Username=postgres;Password=5958";
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }




        private void button1_Click(object sender, EventArgs e)
        {


            string connectionString = "Host=localhost;Port=5432;Database=skires;Username=postgres;Password=5958";
            string searchValue = textBox1.Text;
            string baseFileName;
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                baseFileName = "C:\\Users\\Алина\\Desktop\\проект надаль\\отчёт_за_весь_период_работы";
            }
            else
            {
                baseFileName = "C:\\Users\\Алина\\Desktop\\проект надаль\\отчёт_за_" + searchValue;
            }
            string fileName = baseFileName + ".xlsx";
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT client.suename, client.name, client.otchestvo, inventory.the_capt, booking.tip_number, staff.surname, staff.name, staff.otchesto, data_beginn, data_end, price " +
                               "FROM services JOIN client ON client.id_client = services.id_client JOIN inventory ON inventory.id_inventory = services.id_inventory JOIN booking ON booking.id_booking = services.id_booking JOIN staff ON staff.id_staff = services.id_staff WHERE data_end LIKE '%" + searchValue + "%';";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchValue", searchValue);
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        IWorkbook workbook = new XSSFWorkbook();
                        ISheet sheet = workbook.CreateSheet("Отчёт по оплате услуг");

                        IRow titleRow = sheet.CreateRow(0);
                        titleRow.CreateCell(0).SetCellValue("Отчёт по оплате сервиса оказанного предприятием        ");
                        string[] columnNames = new string[] { "Фамилия клиента   ", "Имя клиента   ", "Отчество клиента  ", "Тип инвентаря  ", "Тип номера  ", "Фамилия сотрудника  ", "Имя сотрудника  ", "Отчество сотрудника  ", "Дата въезда  ", "'Дата выезда  ", "Сумма к оплате  ", " ", "Сумма выручки общая:  " };
                        IRow headerRow = sheet.CreateRow(1);
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(columnNames[i]);
                        }
                        double totalSum = 0;

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            IRow newRow = sheet.CreateRow(i + 1);

                            for (int j = 0; j < dataTable.Columns.Count; j++)
                            {
                                if (dataTable.Columns[j].ColumnName == "price")
                                {
                                    decimal priceValue;
                                    if (decimal.TryParse(dataTable.Rows[i][j].ToString(), out priceValue))
                                    {
                                        newRow.CreateCell(j).SetCellValue((double)priceValue);
                                        totalSum += (double)priceValue;
                                    }
                                    else
                                    {
                                        newRow.CreateCell(j).SetCellValue(dataTable.Rows[i][j].ToString());
                                    }
                                }
                                else
                                {
                                    newRow.CreateCell(j).SetCellValue(dataTable.Rows[i][j].ToString());
                                }
                            }
                        }

                        IRow totalSumRow = sheet.CreateRow(dataTable.Rows.Count + 1);
                        ICell totalSumCell = totalSumRow.CreateCell(10);
                        totalSumCell.SetCellValue(totalSum);

                        // Установка "Итого" в крайней левой ячейке "А"
                        ICell totalLabelCell = totalSumRow.CreateCell(0);
                        totalLabelCell.SetCellValue("Итого");

                        ICellStyle boldStyle = workbook.CreateCellStyle();
                        IFont boldFont = workbook.CreateFont();
                        boldFont.Boldweight = (short)FontBoldWeight.Bold;
                        boldStyle.SetFont(boldFont);
                        totalSumCell.CellStyle = boldStyle;
                        totalLabelCell.CellStyle = boldStyle;

                        for (int i = 1; i < columnNames.Length; i++)
                        {
                            sheet.AutoSizeColumn(i);
                        }

                        using (FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            workbook.Write(file);
                        }
                        

                    }
                }
            }
            MessageBox.Show("Поздравляю! Отчёт создан!");





        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
       {
       

            if (button5.Visible)
            {

                button5.Visible = false;
                if(!button5.Visible)
                {
                    label3.Visible = false;
                    label4.Visible = false;
                    textBox3.Clear();
                    textBox3.Visible = false;
                    textBox4.Clear();
                    textBox4.Visible = false;
                    button4.Visible = false;
                }

                    button6.Visible = false;
                if (!button6.Visible)
                {
                    label5.Visible = false;
                    label6.Visible = false;
                    textBox5.Clear();
                    textBox5.Visible = false;
                    textBox6.Clear();
                    textBox6.Visible = false;
                    button7.Visible = false;
                }
            }
            else
            {

                button5.Visible = true;
                button6.Visible = true;
            }
        }




       
     

        private void button5_Click(object sender, EventArgs e)
        {
            if (label3.Visible)
            {
                label3.Visible = false;
                label4.Visible = false;
                textBox3.Clear();
                textBox3.Visible = false;
                textBox4.Clear();
                textBox4.Visible = false;
                button4.Visible = false;
            }
            else
            {
                // Проверяем видимость элементов, связанных с button6
                if (label5.Visible)
                {
                    label5.Visible = false;
                    label6.Visible = false;
                    textBox5.Clear();
                    textBox5.Visible = false;
                    textBox6.Clear();
                    textBox6.Visible = false;
                    button7.Visible = false;
                }

                label3.Visible = true;
                label4.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                button4.Visible = true;
            }
          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (label5.Visible)
            {
                label5.Visible = false;
                label6.Visible = false;
                textBox5.Clear();
                textBox5.Visible = false;
                textBox6.Clear();
                textBox6.Visible = false;
                button7.Visible = false;
            }
            else
            {
                // Проверяем видимость элементов, связанных с button5
                if (label3.Visible)
                {
                    label3.Visible = false;
                    label4.Visible = false;
                    textBox3.Clear();
                    textBox3.Visible = false;
                    textBox4.Clear();
                    textBox4.Visible = false;
                    button4.Visible = false;
                }

                label5.Visible = true;
                label6.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;
                button7.Visible = true;
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string passwords = textBox6.Text;
            string newroles = textBox5.Text;
            try
            {
                // Создание подключения к базе данных PostgreSQL
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    // Проверка наличия такого же пароля в базе данных
                    string queryCheck = "SELECT COUNT(*) FROM staff WHERE role = @newroles";
                    using (var commandCheck = new NpgsqlCommand(queryCheck, connection))
                    {
                        commandCheck.Parameters.AddWithValue("newroles", newroles);
                        int count = Convert.ToInt32(commandCheck.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Логин '" + newroles + "' этот логин не безопасный измените его, пожалуйста.");
                        }
                        else
                        {
                            // Выполнение обновления пароля
                            string query = "UPDATE staff SET role = @newroles WHERE password = @password";
                            using (var command = new NpgsqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("newroles", newroles);
                                command.Parameters.AddWithValue("password", passwords);
                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Логин успешно изменен");
                                }
                                else
                                {
                                    MessageBox.Show("Такого пароля не существует.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string roles = textBox3.Text;
            string newPassword = textBox4.Text;
            try
            {
                // Создание подключения к базе данных PostgreSQL
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    // Проверка наличия такого же пароля в базе данных
                    string queryCheck = "SELECT COUNT(*) FROM staff WHERE password = @newPassword";
                    using (var commandCheck = new NpgsqlCommand(queryCheck, connection))
                    {
                        commandCheck.Parameters.AddWithValue("newPassword", newPassword);
                        int count = Convert.ToInt32(commandCheck.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Пароль '" + newPassword + "' этот пароль не безопасный измените его, пожалуйста.");
                        }
                        else
                        {
                            // Выполнение обновления пароля
                            string query = "UPDATE staff SET password = @newPassword WHERE role = @role";
                            using (var command = new NpgsqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("newPassword", newPassword);
                                command.Parameters.AddWithValue("role", roles);
                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Пароль успешно изменен");
                                }
                                else
                                {
                                    MessageBox.Show("Такого логина не существует.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
            }
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { // Скрываем все элементы
            label1.Visible = false;
            textBox1.Visible = false;
            button1.Visible = false;
            button13.Visible = false;

            label2.Visible = false;
            textBox2.Visible = false;
            button3.Visible = false;
            button14.Visible = false;

            label8.Visible = false;
            textBox7.Visible = false;
            button8.Visible = false;

            // Проверяем выбранный элемент
            if (comboBox1.SelectedIndex == 0) // Первый элемент
            {
                label1.Visible = true;
                textBox1.Visible = true;
                button1.Visible = true;
                button13.Visible = true;
            }
             if (comboBox1.SelectedIndex == 1) // Второй элемент
            {
                label2.Visible = true;
                textBox2.Visible = true;
                button3.Visible = true;
                button14.Visible = true;
            }
            else if (comboBox1.SelectedIndex == 2) // Второй элемент
            {
                label8.Visible = true;
                textBox7.Visible = true;
                button8.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connectionString = "Host=localhost;Port=5432;Database=skires;Username=postgres;Password=5958;";
            string query = "SELECT surname, name, otchesto,\r\n" +
                "  SUM(price),\r\n " +
                "      COUNT(id_client) FROM\r\n  " +
                "  public.services join public.staff on staff.id_staff= services.id_staff\r\n" +
                "WHERE data_end LIKE '%' || @input || '%' GROUP BY surname, name, otchesto ";
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
            Application winword = new Application();
            winword.Visible = false;
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, conn);
            command.Parameters.AddWithValue("@input", textBox2.Text);
            NpgsqlDataReader dr = command.ExecuteReader();
            Document document = winword.Documents.Add();
            document.Application.Caption = "Отчет выручка";
            foreach (Section section in document.Sections)
            {
                section.PageSetup.Orientation = WdOrientation.wdOrientLandscape;
            }
            Paragraph para1 = document.Content.Paragraphs.Add();
            para1.Range.set_Style(WdBuiltinStyle.wdStyleHeading1);
            para1.Range.Font.Size = 25;
            para1.Range.Text = "Отчет о работе сотрудников";
            para1.Range.InsertParagraphAfter();

            Table firstTable = document.Tables.Add(para1.Range, dr.FieldCount, dr.VisibleFieldCount);
            firstTable.Borders.Enable = 1;
            firstTable.Cell(1, 1).Range.Text = "Фамилия сотрудника";
            firstTable.Cell(1, 2).Range.Text = "Имя сотрудника";
            firstTable.Cell(1, 3).Range.Text = "Отчество сотрудника";
            firstTable.Cell(1, 4).Range.Text = "Выручка";
            firstTable.Cell(1, 5).Range.Text = "Количество клиентов";

            firstTable.Columns[1].Width += 1.5f;
            firstTable.Columns[2].Width += 1.5f;
            firstTable.Columns[3].Width -= 1.5f;
            firstTable.Columns[4].Width -= 1.5f;
            firstTable.Columns[5].Width += 1.5f;

            int rowIndex = 2;
            while (dr.Read())
            {
                firstTable.Cell(rowIndex, 1).Range.Text = dr[0].ToString();
                firstTable.Cell(rowIndex, 2).Range.Text = dr[1].ToString();
                firstTable.Cell(rowIndex, 3).Range.Text = dr[2].ToString();
                firstTable.Cell(rowIndex, 4).Range.Text = dr[3].ToString();
                firstTable.Cell(rowIndex, 5).Range.Text = dr[4].ToString();
                rowIndex++;
            }

            MessageBox.Show("Отчет успешно создан!");
            dr.Close();

            document.SaveAs("C:\\Users\\Алина\\Desktop\\проект надаль\\отчёт по выручке(сотрудники)" + textBox2.Text + ".docx");
            document.Save();
            document.Close();
            winword.Quit();

           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible)
            {
                    
                dataGridView1.Visible = false;
             // button6.Visible = false;
            }
            else
            {

                dataGridView1.Visible = true;
              //button6.Visible = true;
            }
            string connectionString = "Host=localhost;Port=5432;Database=skires;Username=postgres;Password=5958;";
            // string connString = "Host=your_host;Port=your_port;Database=your_database;Username=your_username;Password=your_password";
            string query = "select client.suename, client.name, client.otchestvo, inventory.the_capt, services.data_beginn," +
                " services.data_end, services.price, booking.tip_number from services inner join client on" +
                " client.id_client= services.id_client inner join inventory on inventory.id_inventory= services.id_inventory" +
                " inner join booking on booking.id_booking= services.id_booking WHERE suename LIKE '%' || @input || '%'";
            string input = textBox7.Text; // Получаем ввод из текстового поля

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("input", input);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        // Создаем DataTable для хранения результатов запроса
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Назначаем DataTable источником данных для DataGridView
                        //  dataGridView1.DataSource = dataTable.DefaultView;
                        dataGridView1.DataSource = dataTable;

                        dataGridView1.Columns[0].Width = 100;
                        dataGridView1.Columns[1].Width = 115;
                        dataGridView1.Columns[2].Width = 115;
                        dataGridView1.Columns[3].Width = 255;
                        dataGridView1.Columns[4].Width = 80;
                        dataGridView1.Columns[5].Width = 100;
                        dataGridView1.Columns[6].Width = 90;
                        dataGridView1.Columns[7].Width = 100;
                       
                        // Задание имен столбцов
                        dataGridView1.Columns[0].HeaderText = "Фамилия Клиентая";
                        dataGridView1.Columns[1].HeaderText = "Имя Клиента ";
                        dataGridView1.Columns[2].HeaderText = "Отчество Клиента ";
                        dataGridView1.Columns[3].HeaderText = "Инвентарь";
                        dataGridView1.Columns[4].HeaderText = "Дата въезда ";
                        dataGridView1.Columns[5].HeaderText = "Дата выезда ";
                        dataGridView1.Columns[6].HeaderText = "Сумма к оплате";
                        dataGridView1.Columns[7].HeaderText = "Тип номера";
                        
                    }
                }
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
          textBox8.Text = "";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label11.Visible = false;
            textBox8.Visible = false;
            button11.Visible = false;

            label10.Visible = false;
            textBox8.Visible = false;
            button12.Visible = false;

           

            // Проверяем выбранный элемент
            if (comboBox2.SelectedIndex == 0) // Первый элемент
            {
                label11.Visible = true;
                textBox8.Visible = true;
                button11.Visible = true;
            }
            else if (comboBox2.SelectedIndex == 1) // Второй элемент
            {
                label10.Visible = true;
                textBox8.Visible = true;
                button12.Visible = true;
            }
          
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible)
            {

                dataGridView1.Visible = false;
                // button6.Visible = false;
            }
            else
            {

                dataGridView1.Visible = true;
                //button6.Visible = true;
            }
            string connectionString = "Host=localhost;Port=5432;Database=skires;Username=postgres;Password=5958;";
            // string connString = "Host=your_host;Port=your_port;Database=your_database;Username=your_username;Password=your_password";
            string query = "select dolgnost, surname, name, otchesto, qualification,  telefon, adrress, zp FROM staff WHERE dolgnost LIKE '%' || @input || '%'";

            string input = textBox8.Text; // Получаем ввод из текстового поля

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("input", input);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        // Создаем DataTable для хранения результатов запроса
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Назначаем DataTable источником данных для DataGridView
                        //  dataGridView1.DataSource = dataTable.DefaultView;
                        dataGridView1.DataSource = dataTable;

                        dataGridView1.Columns[0].Width = 130;
                        dataGridView1.Columns[1].Width = 95;
                        dataGridView1.Columns[2].Width = 95;
                        dataGridView1.Columns[3].Width = 90;
                        dataGridView1.Columns[4].Width = 80;
                        dataGridView1.Columns[5].Width = 90;
                        dataGridView1.Columns[6].Width = 300;
                        dataGridView1.Columns[7].Width = 100;
                       

                        // Задание имен столбцов
                        dataGridView1.Columns[0].HeaderText = "Должность";
                        dataGridView1.Columns[1].HeaderText = "Фамилия";
                        dataGridView1.Columns[2].HeaderText = "Имя  ";
                        dataGridView1.Columns[3].HeaderText = "Отчество  ";
                        dataGridView1.Columns[4].HeaderText = "Квалификация ";
                        dataGridView1.Columns[5].HeaderText = "Номер телефона";
                        dataGridView1.Columns[6].HeaderText = "Адрес";
                        dataGridView1.Columns[7].HeaderText = "Заработная плата";
                       
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {

            if (dataGridView1.Visible)
            {

                dataGridView1.Visible = false;
                // button6.Visible = false;
            }
            else
            {

                dataGridView1.Visible = true;
                //button6.Visible = true;
            }
            string connectionString = "Host=localhost;Port=5432;Database=skires;Username=postgres;Password=5958;";
            // string connString = "Host=your_host;Port=your_port;Database=your_database;Username=your_username;Password=your_password";
            string query = "select surname, name, otchesto, dolgnost, qualification,  telefon, adrress, zp FROM staff WHERE surname LIKE '%' || @input || '%'";

            string input = textBox8.Text; // Получаем ввод из текстового поля

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("input", input);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        // Создаем DataTable для хранения результатов запроса
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Назначаем DataTable источником данных для DataGridView
                        //  dataGridView1.DataSource = dataTable.DefaultView;
                        dataGridView1.DataSource = dataTable;

                        dataGridView1.Columns[0].Width = 85;
                        dataGridView1.Columns[1].Width = 85;
                        dataGridView1.Columns[2].Width = 85;
                        dataGridView1.Columns[3].Width = 190;
                        dataGridView1.Columns[4].Width = 150;
                        dataGridView1.Columns[5].Width = 90;
                        dataGridView1.Columns[6].Width = 190;
                        dataGridView1.Columns[7].Width = 100;


                        // Задание имен столбцов
                        
                        dataGridView1.Columns[0].HeaderText = "Фамилия";
                        dataGridView1.Columns[1].HeaderText = "Имя  ";
                        dataGridView1.Columns[2].HeaderText = "Отчество  ";
                        dataGridView1.Columns[3].HeaderText = "Должность";
                        dataGridView1.Columns[4].HeaderText = "Квалификация ";
                        dataGridView1.Columns[5].HeaderText = "Номер телефона";
                        dataGridView1.Columns[6].HeaderText = "Адрес";
                        dataGridView1.Columns[7].HeaderText = "Заработная плата";

                    }
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible)
            {

                dataGridView1.Visible = false;
                // button6.Visible = false;
            }
            else
            {

                dataGridView1.Visible = true;
                //button6.Visible = true;
            }
            string connectionString = "Host=localhost;Port=5432;Database=skires;Username=postgres;Password=5958;";
            // string connString = "Host=your_host;Port=your_port;Database=your_database;Username=your_username;Password=your_password";
            string query = "SELECT client.suename, client.name, client.otchestvo, inventory.the_capt, booking.tip_number, staff.surname, staff.name, staff.otchesto, data_beginn, data_end, price " +
                               "FROM services JOIN client ON client.id_client = services.id_client JOIN inventory ON inventory.id_inventory = services.id_inventory JOIN booking ON booking.id_booking = services.id_booking JOIN staff ON staff.id_staff = services.id_staff WHERE data_end  LIKE '%' || @input || '%'";
            string input = textBox1.Text; // Получаем ввод из текстового поля

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("input", input);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        // Создаем DataTable для хранения результатов запроса
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Назначаем DataTable источником данных для DataGridView
                        //  dataGridView1.DataSource = dataTable.DefaultView;
                        dataGridView1.DataSource = dataTable;

                        dataGridView1.Columns[0].Width = 80;
                        dataGridView1.Columns[1].Width = 80;
                        dataGridView1.Columns[2].Width = 80;
                        dataGridView1.Columns[3].Width = 150;
                        dataGridView1.Columns[4].Width = 80;
                        dataGridView1.Columns[5].Width = 80;
                        dataGridView1.Columns[6].Width = 90;
                        dataGridView1.Columns[7].Width = 80;
                        dataGridView1.Columns[8].Width = 80;
                        dataGridView1.Columns[9].Width = 80;
                        dataGridView1.Columns[10].Width = 90;
                     //   dataGridView1.Columns[7].Width = 100;

                        // Задание имен столбцов
                        dataGridView1.Columns[0].HeaderText = "Фамилия Клиентая";
                        dataGridView1.Columns[1].HeaderText = "Имя Клиента ";
                        dataGridView1.Columns[2].HeaderText = "Отчество Клиента ";
                        dataGridView1.Columns[3].HeaderText = "Инвентарь";
                        dataGridView1.Columns[4].HeaderText = "Тип номера";
                        dataGridView1.Columns[5].HeaderText = "Фамилия Сотрудника";
                        dataGridView1.Columns[6].HeaderText = "Имя Сотрудника ";
                        dataGridView1.Columns[7].HeaderText = "Отчество Сотрудника ";
                        dataGridView1.Columns[8].HeaderText = "Дата въезда ";
                        dataGridView1.Columns[9].HeaderText = "Дата выезда ";
                        dataGridView1.Columns[10].HeaderText = "Сумма к оплате";
                       // dataGridView1.Columns[7].HeaderText = "Тип номера";

                    }
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible)
            {

                dataGridView1.Visible = false;
                // button6.Visible = false;
            }
            else
            {

                dataGridView1.Visible = true;
                //button6.Visible = true;
            }
            string connectionString = "Host=localhost;Port=5432;Database=skires;Username=postgres;Password=5958;";
            // string connString = "Host=your_host;Port=your_port;Database=your_database;Username=your_username;Password=your_password";
            string query = "SELECT surname, name, otchesto,\r\n" +
                "  SUM(price),\r\n " +
                "      COUNT(id_client) FROM\r\n  " +
                "  public.services join public.staff on staff.id_staff= services.id_staff\r\n" +
                "WHERE data_end LIKE '%' || @input || '%' GROUP BY surname, name, otchesto";
            string input = textBox2.Text; // Получаем ввод из текстового поля

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("input", input);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        // Создаем DataTable для хранения результатов запроса
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Назначаем DataTable источником данных для DataGridView
                       
                        dataGridView1.DataSource = dataTable;

                        dataGridView1.Columns[0].Width = 115;
                        dataGridView1.Columns[1].Width = 115;
                        dataGridView1.Columns[2].Width = 115;
                        dataGridView1.Columns[3].Width = 150;
                        dataGridView1.Columns[4].Width = 115;
 

                        // Задание имен столбцов
                        dataGridView1.Columns[0].HeaderText = "Фамилия Сотрудника";
                        dataGridView1.Columns[1].HeaderText = "Имя Сотрудника";
                        dataGridView1.Columns[2].HeaderText = "Отчество Сотрудника";
                        dataGridView1.Columns[3].HeaderText = "Сумма выручки";
                        dataGridView1.Columns[4].HeaderText = "Кол-во клиентов";

                        // Автоматическая подгонка ширины столбцов к содержимому
                        dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        // Установка ширины DataGridView в соответствии с размером столбцов
                        int totalWidth = 0;
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            totalWidth += dataGridView1.Columns[i].Width;
                        }
                        dataGridView1.Width = totalWidth + dataGridView1.RowHeadersWidth + 2; // Учитываем ширину заголовков столбцов и небольшой отступ

                        // Прокрутка до последней строки
                        if (dataGridView1.Rows.Count > 0)
                        {
                            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
                        }
                    }
                }
            }
        }
    }
}
   
