using Microsoft.Data.SqlClient;
using System.Data;

namespace dem_ex
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = @"Server=Vadim-sn\SQLEXPRESS01;Database=dem_ex;Integrated Security=True;TrustServerCertificate=True;";

                connection.Open();
                SqlCommand con = new SqlCommand("SELECT * FROM ������������", connection);

                con.CommandText = "SELECT * FROM ������������ WHERE ����� = '" + textBox1.Text + "' AND ������ = '" + textBox2.Text + "'";

                SqlDataReader reader = con.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string a = reader["����"].ToString();
                        int b = Convert.ToInt32(reader["���_������������"]);
                        if (a == "�������")
                        {
                            Form3 student = new Form3();
                            student.orderId = b;
                            student.Show();
                        }
                        if (a == "�������������")
                        {
                            Form2 teacher = new Form2();
                            teacher.orderId = b;
                            teacher.Show();
                        }
                    }
                    textBox1.Text = "";
                    textBox2.Text = "";
                    this.Hide();
                }
                else
                    MessageBox.Show("�������� ����� ��� ������");
                    textBox2.Text = "";
            }
        }
    }
}