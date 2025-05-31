using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dem_ex
{
    public partial class Form2 : Form
    {
        public int orderId;
        public int mark;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadDisiplineNames();
            LoadGroupNames();
        }

        private void LoadDisiplineNames()
        {
            string connectionString = @"Server=Vadim-sn\SQLEXPRESS01;Database=dem_ex;Integrated Security=True;TrustServerCertificate=True";
            string query = @"SELECT Д.Название
                FROM Дисциплина Д
                INNER JOIN Нагрузка_преподавателя НП ON Д.Код_дисциплины = НП.Код_дисциплины
                INNER JOIN Преподаватель П ON НП.Номер_преподавателя = П.Номер_преподавателя
                INNER JOIN Пользователи Пз ON П.Учетная_запись = Пз.Код_пользователя
                WHERE Пз.Код_пользователя = @orderId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["Название"]);
                        }
                    }
                }
            }
        }

        private void LoadGroupNames()
        {
            string connectionString = @"Server=Vadim-sn\SQLEXPRESS01;Database=dem_ex;Integrated Security=True;TrustServerCertificate=True";
            string query = @"SELECT Г.Название
                    FROM Группа Г
                    INNER JOIN Нагрузка_преподавателя НП ON Г.Номер_группы = НП.Номер_группы
                    INNER JOIN Преподаватель П ON НП.Номер_преподавателя = П.Номер_преподавателя
                    INNER JOIN Пользователи Пз ON П.Учетная_запись = Пз.Код_пользователя
                    WHERE Пз.Код_пользователя = @orderId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader["Название"]);
                        }
                    }
                }
            }
        }

        private void LoadMarksAndStudents()
        {
            if (comboBox1.Text != "" && comboBox2.Text != "")
            {
                try
                {
                    string connectionString = @"Server=Vadim-sn\SQLEXPRESS01;Database=dem_ex;Integrated Security=True;TrustServerCertificate=True";
                    string query = @"SELECT С.Фамилия, С.Имя, С.Отчество, О.Оценка
                FROM Оценки О
                INNER JOIN Студент С ON О.Номер_студента = С.Номер_студента
                INNER JOIN Группа Г ON С.Номер_группы = Г.Номер_группы
                INNER JOIN Дисциплина Д ON О.Вид_дисциплины = Д.Код_дисциплины
                WHERE Д.Название = @disciplineName AND Г.Название = @group";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand command = new SqlCommand(query, conn))
                        {
                            command.Parameters.AddWithValue("@disciplineName", comboBox1.Text);
                            command.Parameters.AddWithValue("@group", comboBox2.Text);

                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMarksAndStudents();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMarksAndStudents();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                if (int.TryParse(dataGridView1.CurrentCell.Value.ToString(), out int value))
                {
                    comboBox3.SelectedIndex = 5 - value;
                    mark = value;
                }
                else
                {
                    comboBox3.Text = "";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int newMark = Convert.ToInt32(comboBox3.Text);

            if (newMark == mark)
            {
                MessageBox.Show("Вы не ввели нового значения", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else
            {

            }
        }
    }
}
