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
using University1;

namespace University
{
    public partial class Form1 : Form
    {
        private readonly object dataGridView;
        int _affectedRow = 0;
        public Form1()
        {
            InitializeComponent();

        }
        private async void Form1_Load(object sender, EventArgs e)
        {

            fillCombos();

        }

        public async void fillCombos()
        {
            string query;
            query = @"select ID , Title from university.[dbo].[tblLesson] order by ID ";
            DataTable dtUniversity = new DataTable();
            dtUniversity = await showData(query);

            comboBox1.DataSource = dtUniversity;
            comboBox1.DisplayMember = dtUniversity.Columns[1].ToString();
            comboBox1.ValueMember = dtUniversity.Columns[0].ToString();


            query = @" select ID,FirstName +  LastName  from university.[dbo].[tblMaster] order by ID";
            DataTable dtUniversity1 = new DataTable();
            dtUniversity1 = await showData(query);
            comboBox3.DataSource = dtUniversity1;
            comboBox3.DisplayMember = dtUniversity1.Columns[1].ToString();
            comboBox3.ValueMember = dtUniversity1.Columns[0].ToString();

        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async Task<DataTable> showData(string query)
        {
            try
            {
                // تعریف کانکشن جهت اتصال به  دیتابیس
                var ConnectionString = "Server=localhost,1433; Initial Catalog = university ; Persist Security Info = False; User ID = Ali2; Password = 123456q@;";
                SqlConnection _sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand _sqlCommand = new SqlCommand();
                _sqlCommand.Connection = _sqlConnection;
                SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);
                var res = _sqlConnection.State;
                if (res == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }

                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.CommandText = query;

                DataTable dt = new DataTable();
                //DataSet ds = new DataSet();
                _sqlDataAdapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private async Task<DataTable> showLimitedTeacher(int indexId)
        {
            try
            {
                // تعریف کانکشن جهت اتصال به  دیتابیس
                var ConnectionString = "Server=localhost,1433; Initial Catalog = university ; Persist Security Info = False; User ID = Ali2; Password = 123456q@;";
                SqlConnection _sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand _sqlCommand = new SqlCommand();
                _sqlCommand.Connection = _sqlConnection;
                SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);
                var res = _sqlConnection.State;
                if (res == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }
                string query = $@"SELECT FirstName +' '+ MT.LastName N'نام و نام خانوادگی',LS.Title N'نام درس قابل ارائه',LS.Code N'کد درس', LS.UnitNum N'تعداد واحد'
                                FROM dbo.tblMaster MT
                                INNER JOIN  [dbo].[Relation] RT  ON MT.ID = RT.MasterID
                                INNER JOIN dbo.tblLesson LS ON LS.ID = RT.LESSONID
                                where MT.ID  = {indexId} ";

                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.CommandText = query;

                DataTable dt = new DataTable();
                _sqlDataAdapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private async void btnNumber2_Click(object sender, EventArgs e)
        {
            string query;
            query = $@"SELECT M.FirstName, M.LastName, M.FatherName,M.BirthDay, L.Title  FROM [university]..Relation R INNER JOIN 
                           university.[dbo].[tblLesson] L ON R.LessonID = L.ID
						   INNER JOIN university.[dbo].[tblMaster] M ON R.MasterID = M.ID WHERE MasterID = {comboBox3.SelectedValue} ";
            DataTable dtUniversity = new DataTable();
            dtUniversity = await showData(query);
            dtGridview.DataSource = dtUniversity;
        }

        private async Task<DataTable> showData(int indexId)
        {
            try
            {
                // تعریف کانکشن جهت اتصال به  دیتابیس
                var ConectionString2 = "Server=localhost,1433; Initial Catalog = university ; Persist Security Info = False; User ID = Ali2; Password = 123456q@;";
                // آدرس کانکشن را گرفته و  به آن وصل می شود
                SqlConnection _sqlConnection = new SqlConnection(ConectionString2);
                SqlCommand _sqlCommand = new SqlCommand();
                _sqlCommand.Connection = _sqlConnection;
                SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);

                var res = _sqlConnection.State;
                if (res == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }
                // کوئری موجود در اسکیوئل که می خواهیم در پروزه خود فراخوانی و اجرا کنیم 
                string query = $@"SELECT FirstName +' '+ MT.LastName N'نام و نام خانوادگی',LS.Title N'نام درس قابل ارائه',LS.Code N'کد درس', LS.UnitNum N'تعداد واحد'
                                FROM dbo.tblMaster MT
                                INNER JOIN  [dbo].[Relation] RT  ON MT.ID = RT.MasterID
                                INNER JOIN dbo.tblLesson LS ON LS.ID = RT.LESSONID
                                where MT.ID  = {indexId} ";

                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.CommandText = query;

                DataTable dt = new DataTable();
                _sqlDataAdapter.Fill(dt);
                return dt;
                //var res2 = _sqlCommand.ExecuteReader();

                //TestTmEntities testTm = new TestTmEntities();
                //var salaries = testTm.CustomerInfoes.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;

            }
        }

        private async void btnNumber1_Click(object sender, EventArgs e)
        {
            string firstName = txtBox2.Text;
            string lastName = txtBox3.Text;
            string Title = txtBox1.Text;
            int cntInsert;
            cntInsert = await SaveData(firstName, lastName , Title);
            DataTable dtRes = new DataTable();
            dtGridview.DataSource = dtRes;

        }

        private static async Task<int> SaveData(string FirstName, string LastName , string Title)
        {
            try
            {
                var ConectionString2 = "Server=localhost,1433; Initial Catalog = university ; Persist Security Info = False; User ID = Ali2; Password = 123456q@;";
                SqlConnection _sqlConnection = new SqlConnection(ConectionString2);
                SqlCommand _sqlCommand = new SqlCommand();
                _sqlCommand.Connection = _sqlConnection;
                SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);

                var res = _sqlConnection.State;
                if (res == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }

                string query = $@"insert into [university].[dbo].[tblMaster](FirstName,LastName) 
                    Values('{FirstName}','{LastName}') 
                    insert into [university].[dbo].[tblLesson](Title) 
                    Values('{Title}')";

                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.CommandText = query;

                return await _sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }
        private async Task<DataTable> showData()
        {
            try
            {
                // تعریف کانکشن جهت اتصال به  دیتابیس
                var ConectionString2 = "Server=localhost,1433; Initial Catalog = university ; Persist Security Info = False; User ID = Ali2; Password = 123456q@;";
                // آدرس کانکشن را گرفته و  به آن وصل می شود
                SqlConnection _sqlConnection = new SqlConnection(ConectionString2);
                SqlCommand _sqlCommand = new SqlCommand();
                _sqlCommand.Connection = _sqlConnection;
                SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);

                var res = _sqlConnection.State;
                if (res == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }
                // کوئری موجود در اسکیوئل که می خواهیم در پروزه خود فراخوانی و اجرا کنیم 
                string query = $@" Select [university].[dbo].[tblMaster](FirstName,LastName) 
                    insert into [university].[dbo].[tblLesson](Title)";


                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.CommandText = query;

                DataTable dt = new DataTable();
                _sqlDataAdapter.Fill(dt);
                return dt;
                //var res2 = _sqlCommand.ExecuteReader();

                //TestTmEntities testTm = new TestTmEntities();
                //var salaries = testTm.CustomerInfoes.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;

            }
        }

        private async void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await Task.Delay(100);

                if (comboBox3.Items.Count >= 0)
                {
                    string query;
                    query = $@"SELECT R.LessonID, L.Title  FROM [university]..Relation R INNER JOIN 
                           university.[dbo].[tblLesson] L ON R.LessonID = L.ID WHERE MasterID = {comboBox3.SelectedValue} order by R.ID";
                    DataTable dtUniversity = new DataTable();
                    dtUniversity = await showData(query);

                    comboBox1.DataSource = dtUniversity;
                    comboBox1.DisplayMember = dtUniversity.Columns[1].ToString();
                    comboBox1.ValueMember = dtUniversity.Columns[0].ToString();
                }
            }
            catch { }
            
        }
    }
}