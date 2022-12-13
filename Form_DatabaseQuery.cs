using SANHUA_TCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SANHUA_MAIN
{
    public partial class Form_DatabaseQuery : Form
    {


        //存放数据库数据源(数据库连接对象 已经是由主窗口传值给数据显示窗口)
        //string strMyConnection = "Data Source = DESKTOP-P9FRMTR;Initial Catalog = sanhua ;Integrated Security = True";

        public SqlConnection MyConnection { get; }
        SqlCommand myCommand = new SqlCommand();
        

        //数据库连接对象 需要在load里实例化
        //SqlConnection myConnection;

        //查询到的数据集合
        List<string> Data_List = new List<string>();
   

        public Form_DatabaseQuery(SqlConnection myConnection)  //接收主窗口传过来的数据库连接对象
        {
            InitializeComponent();
            MyConnection = myConnection;
        }

        private void Btn_Query_Click(object sender, EventArgs e)
        {
           
           
            myCommand.Connection = MyConnection;
            myCommand.CommandType = CommandType.Text;

            //数据库查询指令
            myCommand.CommandText = @"select * form [dbo.SHProcessProperty]";
            SqlDataReader Reader = myCommand.ExecuteReader();
            //获取到数据库读取相关数控读取器 把数据先存放集合里面去
            SqlDataAdapter dataAdapter = new SqlDataAdapter(); //数据操作适配器
            DataSet dataSet = new DataSet(); //数据集
            try
            {
                //打开数据库连接
                MyConnection.Open();
                //执行数据库查询指令
                dataAdapter.SelectCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {

                MessageBox.Show("数据库连接失败！请检查");
            }

            //关闭数据库连接
            MyConnection.Close();
            //把数据填充到数据适配器中
            dataAdapter.Fill(dataSet);
           //实现数据的100行显示
           
            
            dataGridView_DatasInfo.DataSource = dataSet;
          
        }
        //自定义数据显示需要使用
        private void AddRows(Command command)
        {
            object[] rows = {
                command.Command_head,
                command.Command_fun_verify,
                command.Command_process,
                command.Command_station,
                command.Command_staff_code,
                command.Command_code1_lenght,
                command.Command_code1,
                command.Command_code2_lenght,
                command.Command_code2,
                command.Command_flag
            };
            dataGridView_DatasInfo.Rows.Add(rows);


        }

        private void Form_DatabaseQuery_Load(object sender, EventArgs e)
        {
            //目前是使用数据适配器 直接填充数据表格并显示出来
           //对数据表格进行初始化显示相关数据  
           /* dataGridView_DatasInfo.ColumnCount = 12;
            dataGridView_DatasInfo.Columns[0].Name = "序号";
            dataGridView_DatasInfo.Columns[1].Name = "工位号";
            dataGridView_DatasInfo.Columns[2].Name = "功能码";
            dataGridView_DatasInfo.Columns[3].Name = "员工号";
            dataGridView_DatasInfo.Columns[3].Name = "条码长度";
            dataGridView_DatasInfo.Columns[3].Name = "条码";*/
        }
        //实现数据的100行显示操作
        private void Btn_1_Hundred_Click(object sender, EventArgs e)
        {
            //数据库100行查询命令
            myCommand.CommandText = @"select top(100) * form [dbo.SHProcessProperty]";
        }

        private void Btn_2_Hundred_Click(object sender, EventArgs e)
        {
            myCommand.CommandText = @"select top(200) * form [dbo.SHProcessProperty]";
        }
    }
}
