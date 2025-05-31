using Microsoft.Data.SqlClient;
using System.Data;

namespace dem_ex
{
    public partial class Form3 : Form
    {
        public int orderId;
        public Form3()
        {
            InitializeComponent();
        }

        private void LoadDisiplineNames()
        {
            string connectionString = @"Server=Vadim-sn\SQLEXPRESS01;Database=dem_ex;Integrated Security=True;TrustServerCertificate=True;";
            string query = @"
            SELECT Название
            FROM Дисциплина";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["Название"].ToString());
                        }
                    }
                }
            }
        }

        private void LoadMarks()
        {
            try
            {
                string connectString = @"Server=Vadim-sn\SQLEXPRESS01;Database=dem_ex;Integrated Security=True;TrustServerCertificate=True;";
                string query = @"
                SELECT Д.Название, О.Оценка
                FROM Оценки О
                INNER JOIN Студент С ON О.Номер_студента = С.Номер_студента
                INNER JOIN Дисциплина Д ON О.Вид_дисциплины = Д.Код_дисциплины
                WHERE С.Учетная_запись = @user_id AND Д.Название = @discipline_name
                ";

                using (SqlConnection dbConnection = new SqlConnection(connectString))
                {
                    using (SqlCommand command = new SqlCommand(query, dbConnection))
                    {
                        command.Parameters.AddWithValue("@user_id", orderId);
                        command.Parameters.AddWithValue("@discipline_name", comboBox1.Text);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        if (dataTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Нет данных для отображения.");
                        }
                        else
                        {
                            dataGridView1.DataSource = dataTable;
                        }
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            //LoadMarks();
            LoadDisiplineNames();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMarks();
        }
    }
}
