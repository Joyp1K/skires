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
    public partial class Form5 : Form

    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=skires;Username=postgres;Password=5958";
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadDataFromDatabase();
            //LoadClientsToComboBox();
            LoadInventoryDataFromDatabase();
            LoadStaffDataFromDatabase();

        }
        private void LoadData()
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT the_capt AS \"Название\", description_of_the_collec AS \"Описание\", size AS \"Размер\", kol_vo AS \"Количество\", cost AS \"Стоимость\" FROM Inventory";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dataGridView1.DataSource = dt;
                    }
                }
            }
        }


        private void LoadDataFromDatabase()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // SQL-запрос для выборки данных из таблицы Services
                    string sqlQuery = @"
                SELECT 
                    CONCAT(C.suename, ' ', C.name) AS client_full_name,
                    I.the_capt AS inventory_caption,
                    B.tip_number AS booking_type,
                    CONCAT(ST.surname, ' ', ST.name) AS staff_full_name,
                    S.data_beginn AS data_beginn,
                    S.data_end AS data_end,
                    S.price AS price
                FROM 
                    Services S
                JOIN 
                    Client C ON S.id_client = C.id_client
                JOIN 
                    Inventory I ON S.id_inventory = I.id_inventory
                JOIN 
                    Booking B ON S.id_booking = B.id_booking
                JOIN 
                    Staff ST ON S.id_staff = ST.id_staff;
            ";

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlQuery, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Очистить существующие столбцы и строки, если есть
                    dataGridView2.Columns.Clear();
                    dataGridView2.DataSource = null;

                    // Добавление столбцов в DataGridView
                    dataGridView2.AutoGenerateColumns = false;

                    // Создание столбцов для каждого столбца в DataTable
                    DataGridViewTextBoxColumn clientFullNameColumn = new DataGridViewTextBoxColumn();
                    clientFullNameColumn.DataPropertyName = "client_full_name";
                    clientFullNameColumn.HeaderText = "ФИО клиента";
                    dataGridView2.Columns.Add(clientFullNameColumn);

                    DataGridViewTextBoxColumn inventoryCaptionColumn = new DataGridViewTextBoxColumn();
                    inventoryCaptionColumn.DataPropertyName = "inventory_caption";
                    inventoryCaptionColumn.HeaderText = "Наименование инвентаря";
                    dataGridView2.Columns.Add(inventoryCaptionColumn);

                    DataGridViewTextBoxColumn bookingTypeColumn = new DataGridViewTextBoxColumn();
                    bookingTypeColumn.DataPropertyName = "booking_type";
                    bookingTypeColumn.HeaderText = "Тип бронирования";
                    dataGridView2.Columns.Add(bookingTypeColumn);

                    DataGridViewTextBoxColumn staffFullNameColumn = new DataGridViewTextBoxColumn();
                    staffFullNameColumn.DataPropertyName = "staff_full_name";
                    staffFullNameColumn.HeaderText = "ФИО сотрудника";
                    dataGridView2.Columns.Add(staffFullNameColumn);

                    DataGridViewTextBoxColumn dataBeginColumn = new DataGridViewTextBoxColumn();
                    dataBeginColumn.DataPropertyName = "data_beginn";
                    dataBeginColumn.HeaderText = "Дата начала";
                    dataGridView2.Columns.Add(dataBeginColumn);

                    DataGridViewTextBoxColumn dataEndColumn = new DataGridViewTextBoxColumn();
                    dataEndColumn.DataPropertyName = "data_end";
                    dataEndColumn.HeaderText = "Дата окончания";
                    dataGridView2.Columns.Add(dataEndColumn);

                    DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn();
                    priceColumn.DataPropertyName = "price";
                    priceColumn.HeaderText = "Цена";
                    dataGridView2.Columns.Add(priceColumn);


                    // Устанавливаем источник данных
                    dataGridView2.DataSource = dataTable;
                    // Очищаем комбобокс перед добавлением новых данных
                    comboBox1.Items.Clear();

                    // Добавляем данные из столбца "client_full_name" в комбобокс
                    foreach (DataRow row in dataTable.Rows)
                    {
                        comboBox1.Items.Add(row["client_full_name"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных из базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "INSERT INTO Inventory (the_capt, description_of_the_collec, size, kol_vo, cost) VALUES (@the_capt, @description_of_the_collec, @size, @kol_vo, @cost)";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        // Передаем параметры из текстовых полей
                        cmd.Parameters.AddWithValue("@the_capt", textBox1.Text);
                        cmd.Parameters.AddWithValue("@description_of_the_collec", textBox2.Text);
                        cmd.Parameters.AddWithValue("@size", textBox3.Text);
                        cmd.Parameters.AddWithValue("@kol_vo", int.Parse(textBox4.Text));
                        cmd.Parameters.AddWithValue("@cost", decimal.Parse(textBox5.Text));

                        // Выполняем команду SQL
                        cmd.ExecuteNonQuery();
                    }
                }

                // Обновляем таблицу, чтобы отобразить новые данные
                LoadData();

                // Сообщение о успешном добавлении данных
                MessageBox.Show("Данные успешно добавлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Сообщение об ошибке, если что-то пошло не так
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Получаем значение из выбранной строки в DataGridView
            string the_captToDelete = dataGridView1.CurrentRow.Cells["Название"].Value.ToString();

            // Вызываем метод удаления данных, передавая значение переменной the_captToDelete
            DeleteData(the_captToDelete);
        }
        private void DeleteData(string the_captToDelete)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "DELETE FROM Inventory WHERE the_capt = @the_capt";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@the_capt", the_captToDelete);

                        // Выполняем команду SQL
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Проверяем, были ли удалены строки
                        if (rowsAffected > 0)
                        {
                            // Обновляем таблицу, чтобы отобразить новые данные после удаления
                            LoadData();

                            // Сообщение об успешном удалении данных
                            MessageBox.Show("Данные успешно удалены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Сообщение, если строка с указанным значением не найдена
                            MessageBox.Show("Запись с указанным значением не найдена.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Сообщение об ошибке, если что-то пошло не так
                MessageBox.Show($"Ошибка при удалении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void LoadInventoryDataFromDatabase()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // SQL-запрос для выборки данных из таблицы Inventory
                    string sqlQuery = @"
                SELECT 
                    the_capt
                FROM 
                    Inventory;
            ";

                    NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, connection);
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    // Очищаем комбобокс перед добавлением новых данных
                    comboBox2.Items.Clear();

                    // Добавляем данные из столбца "the_capt" в комбобокс
                    while (reader.Read())
                    {
                        string item = reader["the_capt"].ToString();
                        comboBox2.Items.Add(item);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных из базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadStaffDataFromDatabase()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // SQL-запрос для выборки данных из таблицы Staff
                    string sqlQuery = @"
                SELECT 
                    CONCAT(surname, ' ', name) AS staff_full_name
                FROM 
                    Staff;
            ";

                    NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, connection);
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    // Очищаем комбобокс перед добавлением новых данных
                    comboBox3.Items.Clear();


                    // Добавляем данные из столбца "staff_full_name" в комбобокс
                    while (reader.Read())
                    {
                        string fullName = reader["staff_full_name"].ToString();
                        comboBox3.Items.Add(fullName);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных из базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateInventoryForClient(string surname, string name, string selectedInventory)
        {
            try
            {
                int idInventory = GetInventoryId(selectedInventory);

                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = @"
            UPDATE Services 
            SET id_inventory = @id_inventory
            WHERE id_client = (SELECT id_client FROM Client WHERE suename = @surname AND name = @name)";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_inventory", idInventory);
                        cmd.Parameters.AddWithValue("@surname", surname);
                        cmd.Parameters.AddWithValue("@name", name);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            LoadDataFromDatabase();
                            MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStaffForClient(string surname, string name, string selectedStaff)
        {
            try
            {
                int idStaff = GetStaffId(selectedStaff);

                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = @"
            UPDATE Services 
            SET id_staff = @id_staff
            WHERE id_client = (SELECT id_client FROM Client WHERE suename = @surname AND name = @name)";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_staff", idStaff);
                        cmd.Parameters.AddWithValue("@surname", surname);
                        cmd.Parameters.AddWithValue("@name", name);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            LoadDataFromDatabase();
                            MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem != null)
                {
                    string[] fullName = comboBox1.SelectedItem.ToString().Split(' ');
                    string surname = fullName[0];
                    string name = fullName[1];

                    if (comboBox2.SelectedItem != null)
                    {
                        string selectedInventory = comboBox2.SelectedItem.ToString();
                        UpdateInventoryForClient(surname, name, selectedInventory);
                    }

                    if (comboBox3.SelectedItem != null)
                    {
                        string selectedStaff = comboBox3.SelectedItem.ToString();
                        UpdateStaffForClient(surname, name, selectedStaff);
                    }

                    if (comboBox2.SelectedItem == null && comboBox3.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите, что вы хотите обновить.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите клиента.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Метод для получения id_inventory из таблицы Inventory по наименованию инвентаря
        private int GetInventoryId(string inventoryName)
        {
            int idInventory = 0;
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT id_inventory FROM Inventory WHERE the_capt = @inventoryName";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@inventoryName", inventoryName);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            idInventory = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении id_inventory: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return idInventory;
        }
        // Метод для получения id_staff из таблицы Staff по ФИ сотрудника
        private int GetStaffId(string staffFullName)
        {
            int idStaff = 0;
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT id_staff FROM Staff WHERE CONCAT(surname, ' ', name) = @staffFullName";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@staffFullName", staffFullName);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            idStaff = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении id_staff: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return idStaff;
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form a1 = new Form1();
            a1.Show();
            this.Hide();
        }
    }


        //private void LoadClientsToComboBox()
        //{
        //    try
        //    {
        //        using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
        //        {
        //            connection.Open();

        //            string sqlQuery = "SELECT id_client, CONCAT(suename, ' ', name) AS client_name FROM Client";
        //            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlQuery, connection);
        //            DataTable dataTable = new DataTable();
        //            adapter.Fill(dataTable);

        //            comboBox1.DisplayMember = "client_name";
        //            comboBox1.ValueMember = "id_client";
        //            comboBox1.DataSource = dataTable;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка при загрузке данных клиентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void comboBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int selectedClientId = (int)comboBox1.SelectedValue;
        //    LoadInventoryForClient(selectedClientId);
        //}

        //private void LoadInventoryForClient(int clientId)
        //{
        //    try
        //    {
        //        using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
        //        {
        //            connection.Open();

        //            string sqlQuery = "SELECT id_inventory, the_capt FROM Inventory WHERE id_client = @clientId";
        //            NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection);
        //            command.Parameters.AddWithValue("@clientId", clientId);

        //            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
        //            DataTable dataTable = new DataTable();
        //            adapter.Fill(dataTable);

        //            // Очистка комбобокса перед загрузкой новых данных
        //            comboBox2.Items.Clear();

        //            // Заполнение комбобокса данными инвентаря
        //            foreach (DataRow row in dataTable.Rows)
        //            {
        //                comboBox2.Items.Add(new InventoryItem
        //                {
        //                    Id = Convert.ToInt32(row["id_inventory"]),
        //                    TheCapt = row["the_capt"].ToString()
        //                });
        //            }

        //            // Выбор первого элемента в комбобоксе
        //            if (comboBox2.Items.Count > 0)
        //                comboBox2.SelectedIndex = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка при загрузке инвентаря для клиента: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private class InventoryItem
        //{
        //    public int Id { get; set; }
        //    public string TheCapt { get; set; }

        //    public override string ToString()
        //    {
        //        return TheCapt;
        //    }
        //}
    }





 
