using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WorkWithDataBase
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string ConnetcionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\3 курс\ПП\ПРсSQL\WorkWithDataBase\WorkWithDataBase\Database1.mdf;Integrated Security=True";
            /*Кукушка*/

            sqlConnection = new SqlConnection(ConnetcionString);

            await sqlConnection.OpenAsync();
            SqlDataReader SqlReader = null;
            SqlCommand comand = new SqlCommand("SELECT * FROM [Table]", sqlConnection);

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Table]", sqlConnection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

            try
            {
                SqlReader = await comand.ExecuteReaderAsync();

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (SqlReader != null)
                    SqlReader.Close();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

        private async void button_Add_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                SqlCommand comand = new SqlCommand("INSERT INTO [Table] (FAM, Mathematics, Biology, Programming, Chemistry, Physics) VALUES (@FAM, @Mathematics, @Biology, @Programming, @Chemistry, @Physics)", sqlConnection);

                try
                {
                    Random random = new Random();
                    comand.Parameters.AddWithValue("FAM", textBox1.Text);
                    comand.Parameters.AddWithValue("Mathematics", Convert.ToString(random.Next(2, 6)));
                    comand.Parameters.AddWithValue("Biology", Convert.ToString(random.Next(2, 6)));
                    comand.Parameters.AddWithValue("Programming", Convert.ToString(random.Next(2, 6)));
                    comand.Parameters.AddWithValue("Chemistry", Convert.ToString(random.Next(2, 6)));
                    comand.Parameters.AddWithValue("Physics", Convert.ToString(random.Next(2, 6)));

                    await comand.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите текст", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string ConnetcionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\3 курс\ПП\ПРсSQL\WorkWithDataBase\WorkWithDataBase\Database1.mdf;Integrated Security=True";
            sqlConnection = new SqlConnection(ConnetcionString);
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Table]", sqlConnection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

            textBox1.Text = "";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string ConnetcionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\3 курс\ПП\ПРсSQL\WorkWithDataBase\WorkWithDataBase\Database1.mdf;Integrated Security=True";
            sqlConnection = new SqlConnection(ConnetcionString);

            //Удаление столбца
            if (checkBox1.Checked == false)
            {
                try
                {
                    string alter = "ALTER TABLE [Table] DROP COLUMN AverageScore";
                    sqlConnection.Open();
                    SqlCommand cmd = sqlConnection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = alter;
                    cmd.ExecuteNonQuery();

                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Table]", sqlConnection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //Добавление столбца
            if (checkBox1.Checked == true)
            {
                try
                {
                    //Добавление столбца
                    string alter = "ALTER TABLE [Table] ADD AverageScore INT";
                    sqlConnection.Open();
                    SqlCommand cmd = sqlConnection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = alter;
                    cmd.ExecuteNonQuery();

                    //Заполнение нового столбца
                    AverageScoreUpdate();

                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Table]", sqlConnection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void AverageScoreUpdate()
        {
            try
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\3 курс\ПП\ПРсSQL\WorkWithDataBase\WorkWithDataBase\Database1.mdf;Integrated Security=True";

                string sqlExpression = "SELECT * FROM [Table]";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            object FAM = reader.GetValue(0);
                            object Mathematics = reader.GetValue(1);
                            object Biology = reader.GetValue(2);
                            object Programming = reader.GetValue(3);
                            object Chemistry = reader.GetValue(4);
                            object Physics = reader.GetValue(5);

                            int sum = Convert.ToInt32(Mathematics) + Convert.ToInt32(Biology) + Convert.ToInt32(Programming) + Convert.ToInt32(Chemistry) + Convert.ToInt32(Physics);

                            SqlCommand comandInsert = new SqlCommand("UPDATE [Table] SET [AverageScore] = " + sum + " WHERE [AverageScore] IS NULL AND [FAM] = \'" + FAM + "\'", sqlConnection);

                            comandInsert.ExecuteNonQuery();
                        }
                    }
                    reader.Close();
                }
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Table]", sqlConnection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            try
            {
                Random random = new Random();

                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\3 курс\ПП\ПРсSQL\WorkWithDataBase\WorkWithDataBase\Database1.mdf;Integrated Security=True";

                string sqlExpression = "SELECT * FROM [Table]";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            object FAM = reader.GetValue(0);
                            int Mathematics = random.Next(2, 6);
                            int Biology = random.Next(2, 6);
                            int Programming = random.Next(2, 6);
                            int Chemistry = random.Next(2, 6);
                            int Physics = random.Next(2, 6);

                            SqlCommand comandUpdate = new SqlCommand("UPDATE [Table] SET [Mathematics] = " + Mathematics + ", [Biology] = " + Biology + ", [Programming] = " + Programming + ", [Chemistry] = " + Chemistry + ", [Physics] = " + Physics + " WHERE [FAM] = \'" + FAM + "\'", sqlConnection);

                            int sum = Mathematics + Biology + Programming + Chemistry + Physics;
                            SqlCommand comandInsert = new SqlCommand("UPDATE [Table] SET [AverageScore#2] = " + sum + " WHERE [AverageScore#2] IS NULL AND [FAM] = \'" + FAM + "\'", sqlConnection);

                            comandUpdate.ExecuteNonQuery();
                            comandInsert.ExecuteNonQuery();
                        }
                    }
                    connection.Close();
                    reader.Close();
                }

                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Table]", sqlConnection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string ConnetcionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\3 курс\ПП\ПРсSQL\WorkWithDataBase\WorkWithDataBase\Database1.mdf;Integrated Security=True";
                sqlConnection = new SqlConnection(ConnetcionString);
                sqlConnection.Open();

                if (checkBox2.Checked == true)
                {
                    //Добавление столбца
                    string alter = "ALTER TABLE [Table] ADD [AverageScore#2] INT";
                    SqlCommand cmd = sqlConnection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = alter;
                    cmd.ExecuteNonQuery();
                }
                if (checkBox2.Checked == false)
                {
                    string alter = "ALTER TABLE [Table] DROP COLUMN [AverageScore#2]";
                    SqlCommand cmd = sqlConnection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = alter;
                    cmd.ExecuteNonQuery();
                }

                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Table]", sqlConnection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
