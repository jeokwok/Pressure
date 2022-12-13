using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mcOMRON;
using Modbus.Device;
using System.Net;
using System.Threading;
using SANHUA_TCP;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace SANHUA_MAIN
{
    public partial class Form_main : Form
    {      
        private bool ModeChange = false; //手自动标识

        //通讯循环标识位
        private CancellationTokenSource cts_1;
        private CancellationTokenSource cts_2;
        private CancellationTokenSource cts_3;

        //创建左右工位显示即时数据的数据结构对象
        Command_str command_str = new Command_str();
        Command tcp_command = new Command();

        //发送左气缸动作延时数据给系统设置
        public delegate void SendLeftDelay(int delay_value);
        public event SendLeftDelay sendLeftDelay;
        
        /// <summary>
        /// 私有串口实例
        /// </summary>
        private SerialPort serialPort = new SerialPort();
        private SerialPort serialPort_right = new SerialPort();

        //仪器串口参数全局变量--LOAD方法下进行连接
        InstrumentParam serial_param = null;

        //仪器串口参数读取文件路径                                  
        private string serial_param_path = Application.StartupPath + @"\serial_port_config.ini";


        //private SerialPort serialPort_right = new SerialPort();
        //SerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits);
   
        // 私有ModbusRTU主站字段
        private IModbusMaster master;    
        private IModbusMaster master_right;

        string g_left_counter;//左工位产量
        string g_right_counter;//右工位产量
        string g_left_ok;
        string g_right_ok;

        string g_Left_Programmer;
        string g_Left_Test_result_Pressure;
        string g_Left_Test_result_Q;

        int right_row_count = 0;
        int left_row_count = 0;
        string g_Right_Programmer = "1";
        string g_Right_Test_result_Pressure;
        string g_Right_Test_result_Q;
        //左工位泄露测试结果 true = OK 
        bool g_Left_Test_Status;  
        //右工位泄露测试结果
        bool g_Right_Test_Status;

        //左右轴测试成功后计数1次，数据上传数据库1次
        int g_i = 0;
        int g_j = 0;
        bool operation_flag = false;//手自动操作标志
        //存放数据库数据源
        string strMyConnection = "Data Source = DESKTOP-P9FRMTR;Initial Catalog = sanhua ;Integrated Security = True";
        //数据库连接对象 需要在load里实例化
        SqlConnection myConnection;
        private int PosX = 0;//滚动显示报警信息

        //左右工位串口读取信息
        private List<ushort> LeftComData = new List<ushort>();
        private List<ushort> RightComData = new List<ushort>();
        //左右条码的集合
        private List<string> Left_CodeList = new List<string>();
        private List<string> Right_CodeList = new List<string>();
        int Code_MaxLength = 100;//条码最大容量
        JX_APP app;//程序主逻辑



        public Form_main()
        {
            InitializeComponent();
            app = new JX_APP();
        }
        Stopwatch softRunWatch = new Stopwatch();
        Stopwatch workRunWatch = new Stopwatch();

        //接受系统设置界面发送过来的字符数据
        private void receiveChildMsg(string msg)
        {
            tb_system_error.Text = msg;   
        }

        //把主窗口的左气缸延时参数传递给系统设置窗口

        // 22.9.16创建通讯类
        OmronPLC client = new OmronPLC(mcOMRON.TransportType.Tcp);
        string path;
        string TxtIP;
        string Port;
  
        public void connectIP()
        {
            path = Application.StartupPath + @"\config.ini";
            TxtIP = OmronPLC.IniFile.ReadIniData("PlcComTCP", "IP", "", path);
            Port = OmronPLC.IniFile.ReadIniData("PlcComTCP", "Port", "", path);
            tcpFINSCommand tcpCommand = ((tcpFINSCommand)client.FinsCommand);
            tcpCommand.SetTCPParams(IPAddress.Parse(TxtIP), Convert.ToInt32(Port));
            bool Result = client.Connect();
            MessageBox.Show(Result?"PLC通讯连接成功":"PLC通讯连接失败","建立连接");
            if (Result == true)
            {
                //这里是启动定时器1
                //timer1.Enabled = true;
               //PLC连接状态
                panel_connect_status.BackColor = Color.Green;
                onNetworkStatetoolStripStatusLabel.Text = "PLC通信正常";
                //点亮与PLC连接状态的同时判断扫码枪和PLC连接状态
                panel_scan.BackColor = Color.Green;
                //ovalShape2.BackColor = Color.Transparent;
                //ovalShape1.BackColor = Color.Lime;
            }
            else
            {
                panel_connect_status.BackColor = Color.Red;
            }
        }

        string[] str_array_left = new string[45];
        string[] str_array_right = new string[45];

        /// <summary>
        /// 界面显示读取结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        private void SetMsg<T>(List<T> result)
        {
            string msg = string.Empty;
            string lefe_test_faild_counter = string.Empty;

            result.ForEach(m => msg += $"{m} ");
            string[] str_array = msg.Trim().Split(' ');
            str_array_left = str_array;
            //把数据传递给全局变量 在传递给系统设置
            // left_data = str_array;
            
            
            //Left_Total_Count.Text = JX_split_string(str,1,2);
            //Left_NG_Count.Text = JX_split_string(str, 6, 8);

            //32寄存器读取测试仪器状态
            g_left_counter = str_array[2];
            g_left_ok = (string)JX_strTOintTOstr(str_array[2], str_array[0]);
            g_Left_Programmer = str_array[29];
            g_Left_Test_result_Pressure = JX_FUNCTION_DecimalToBinary_right_2bit(int.Parse(str_array[38]));
            g_Left_Test_result_Q = JX_FUNCTION_DecimalToBinary_right_2bit(int.Parse(str_array[40]));
            int left_test_status = int.Parse(str_array[31]);
           
        }

        /// <summary>
        /// 右工位串口数据读取显示 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        private void SetMsg_right<T>(List<T> result)
        {
            string msg = string.Empty;
            string right_test_faild_counter = string.Empty;

            result.ForEach(m => msg += $"{m} ");
            string[] str_array = msg.Trim().Split(' ');
            str_array_right = str_array;
            //把数据传递给全局变量 在传递给系统设置
            // right_data = str_array;

            //Pressure_Value.Text = str_array[13];


            g_right_counter = str_array[2];
            g_right_ok = (string)JX_strTOintTOstr(str_array[2], str_array[0]);
            g_Right_Test_result_Pressure  = JX_FUNCTION_DecimalToBinary_right(int.Parse(str_array[38]));
            g_Right_Test_result_Q = JX_FUNCTION_DecimalToBinary_right_2bit(int.Parse(str_array[40]));
            int right_test_status = int.Parse(str_array[31]);
           
        }


        private string JX_FUNCTION_DecimalToBinary(int number_in)
        {
            string str1;
            if (JX_Judge_PositiveOrNegative(number_in)  == true)
            {
                 str1 = Convert.ToDouble(number_in / 10.00) + "pa";
            }
            else {
                int str_y = (int)((int)~number_in + 1);
                double str_x = str_y / 10.00;
                str1 = "-" + Convert.ToDouble(str_x / 10.00) + "pa";
            }
            return str1;
        }
 
        
        private string JX_FUNCTION_DecimalToBinary_2bit(int number_in)
        {
            string str1;
            if (JX_Judge_PositiveOrNegative(number_in) == true)
            {
                str1 = Convert.ToDouble(number_in / 100.00) + "ml/min";
            }
            else
            {
                int str_y = (int)((int)~number_in + 1);
                double str_x = str_y / 100.00;
                str1 = "-" + Convert.ToDouble(str_x) + "ml/min ";
            }
            return str1;
        }

        private string JX_FUNCTION_DecimalToBinary_right(int number_in)
        {
            string str1;

            if (number_in < 32767)
            {
                int str_s = (int)((int)~number_in + 1);
                double str_m = str_s / 10.00;
                str1 = Convert.ToDouble(str_m) + "pa";
            }
            else {

                short str_y = (short)((short)~number_in + 1);
                double str_x = str_y / 10.00;
                str1 = "-" + Convert.ToDouble(str_x) + "pa";
            }     
                return str1;
        }

        private string JX_FUNCTION_DecimalToBinary_right_2bit(int number_in)
        {
            string str1;

            if (number_in < 32767)
            {
                int str_s = (int)((int)~number_in + 1);
                double str_m = str_s / 100.00;
                str1 = Convert.ToDouble(str_m) + "ml/min";
            }
            else
            {

                short str_y = (short)((short)~number_in + 1);
                double str_x = str_y / 100.00;
                str1 = "-" + Convert.ToDouble(str_x) + "ml/min";
            } 
            return str1;
        }

        /// <summary>
        /// 工具方法实现字符转换
        /// </summary>
        /// <param name="str_in1"></param>
        /// <param name="str_in2"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private Object JX_strTOintTOstr(string str_in1, string str_in2)
        {
            string result;
            try
            {
                if (str_in1 == null && str_in2 == null)
                {
                    return null;
                }
                else
                {
                    int str1 = int.Parse(str_in1);
                    int str2 = int.Parse(str_in2);
                    //int r = Convert.ToInt32(str_array[2]);
                    //int b = Convert.ToInt32(str_array[0]);
                    result = (str1 - str2).ToString();
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

        private void Form_main_Load(object sender, EventArgs e)
        {
            toolStripProgressBar1.Maximum = 100;
            Btn_start.Enabled = true;//启用启动开始按钮
            softRunWatch.Start();  //监控软件启动时间
            sTimer.Start();
            timer1.Start();
            //timer2.Start();
            toolStripProgressBar1.Value = 10;
            //BeginInvoke表示异步
            /*this.BeginInvoke(new Action(() => {
                   RightCodeInfo();
              }));*/
            //初始化MODBUS对象
            master = ModbusSerialMaster.CreateRtu(serialPort);
            master_right = ModbusSerialMaster.CreateRtu(serialPort_right);

             OpengLeftComm();
            //左工位端口设置
            /*string port_name_left = "com9";
            int baud_rate_left = 115200;
            serialPort.PortName = port_name_left;
            serialPort.BaudRate = baud_rate_left;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Open();*/



            //左工位定时器刷新
            /* this.Invoke(new Action(() => {
             System.Timers.Timer time = new System.Timers.Timer(1000);
             time.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick_Timers_Left);
             time.AutoReset = true;
             time.Enabled = true;
             time.Interval = 1000;
         }));*/

            OpenRightComm();
            //右工位端口设置
            /* string port_name_right = "com8";
             int baud_rate_right = 115200;
             serialPort_right.PortName = port_name_right;
             serialPort_right.BaudRate = baud_rate_right;
             serialPort_right.Parity = Parity.None;
             serialPort_right.DataBits = 8;
             serialPort_right.StopBits = StopBits.One;
             serialPort_right.Open();


             //右工位定时器刷新
             this.Invoke(new Action(() => {
                 System.Timers.Timer time_right = new System.Timers.Timer(1000);
                 time_right.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick_Timer_Right);
                 time_right.AutoReset = true;
                 time_right.Enabled = true;
                 time_right.Interval = 1000;
             }));*/


            //左、右工位条码信息读取
            //LeftCodeInfo();

            toolStripProgressBar1.Value = 20;
            string user_name = "管理员";
            User_Info.Text = user_name;
            currentUserToolStripStatusLabel.Text = "当前登录：管理员";

            string product_id = "a--01";
            Product_Info.Text = product_id;

            Left_Status.BackColor = Color.Transparent;
            Left_Status.Text = "";
            Right_Status.BackColor = Color.Transparent;
            Right_Status.Text = "";

            //显示员工号
            string user_info = System.IO.File.ReadAllText("C:/Users/pc/source/repos/SANHUA_MAIN/SANHUA_MAIN/bin/Debug/user_info.text");
            string[] info = user_info.Split('\r');
            tb_staff_num.Text = info[1];
            //this.Invoke(new Action(() =>{
            currentUserToolStripStatusLabel.Text = "当前员工：" + "123";
            onProcessNameToolStripStatusLabel.Text = "员工" + "123" + "已登录";
            //}));

            //修改左右信息显示框的字体
            //dataGridView_Left.Font = new Font("宋体", 8);
            //dataGridView_Right.Font = new Font("宋体", 8);
            //扫码枪连接状态初始化
            panel_scan.BackColor = Color.Transparent;
            //系统报警初始化
            tb_system_error.Text = "0";

            //左右工位程序号选择
           
         
            //仪器串口参数的获取
            serial_param = Get_serial_Config(serial_param_path);
            if (serial_param == null)
            {
                onProcessNameToolStripStatusLabel.Text = "仪器串口参数配置文件读取失败";
                //AddLog("仪器串口参数配置文件读取失败");
            }
            connectIP();
            AddLog("PLC通信连接正常");
            onProcessNameToolStripStatusLabel.Text = "PLC通信连接正常";

            /* jx_timer1.Elapsed += new System.Timers.ElapsedEventHandler(JX_Timer1_Execute);
             jx_timer1.AutoReset = true;
             jx_timer1.Enabled = true;
             jx_timer1.Start();*/

            //创建异步线程实现界面刷新方法1  

            toolStripProgressBar1.Value = 90;
            myConnection = new SqlConnection(strMyConnection);


            panel_left_scan_shield.BackColor = Color.Green;
            panel_left_database_write.BackColor = Color.Green;
            
            panel_right_scan_shield.BackColor = Color.Green;
            panel_right_database_write.BackColor = Color.Green;

            panel_left_inspect_status.BackColor = Color.Green;
            panel_right_inspect_status.BackColor = Color.Green;
       
        }

        #region 左串口设置及读取相关
        private void OpengLeftComm()
        {
            //左工位端口设置
            string port_name_left = "com9";
            int baud_rate_left = 115200;
            try
            {
                serialPort.PortName = port_name_left;
                serialPort.BaudRate = baud_rate_left;
                serialPort.Parity = Parity.None;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;
                serialPort.Open();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message,"警告：",MessageBoxButtons.OK);
                serialPort.Close();             
            }         
            LeftComData = this.master.ReadHoldingRegisters(1, 1, 45).ToList();    
            this.Invoke(new EventHandler(Left_CommShowData));
        }
        /// <summary>
        /// 左串口数据读取并显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Left_CommShowData(object sender, EventArgs e)
        {
            // MessageBox.Show("ok");

            //SetMsg(master.ReadHoldingRegisters(1, 1, 45).ToList());
            //左工位测试flag  
            //Thread.Sleep(50);
            //ShowIO_tip();
            if (client.ReadBitCIO(0, 3) == 1)
            {        
                Left_Status.BackColor = Color.Green;
                Left_Status.Text = "OK";
                g_Left_Test_Status = true;
            }
            else if (client.ReadBitCIO(0, 4) == 1)
            {
                Left_Status.BackColor = Color.Red;
                Left_Status.Text = "NG";
                g_Left_Test_Status = false;
                
            }
            else if (client.ReadBitCIO(0, 2) == 1)
            {
                Left_Status.BackColor = Color.Transparent;
                Left_Status.Text = "检测中...";              
            }
            else
            {
                Left_Status.BackColor = Color.Transparent;
                Left_Status.Text = "待检测";
            }        

            //SetMsg(master.ReadHoldingRegisters(1, 1, 45).ToList());
        }
        #endregion

        #region 右工位串口设置和读取

        private void OpenRightComm()
        {
            //右工位端口设置
            string port_name_right = "com8";
            int baud_rate_right = 115200;
            try
            {
                serialPort_right.PortName = port_name_right;
                serialPort_right.BaudRate = baud_rate_right;
                serialPort_right.Parity = Parity.None;
                serialPort_right.DataBits = 8;
                serialPort_right.StopBits = StopBits.One;
                serialPort_right.Open();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message,"警告：",MessageBoxButtons.OK);
                serialPort_right.Close();
            }
            RightComData = this.master_right.ReadHoldingRegisters(1, 1, 45).ToList();
            this.Invoke(new EventHandler(Right_CommShowData));
        }

        private void Right_CommShowData(object sender, EventArgs e)
        {
          
            
             //SetMsg_right(master_right.ReadHoldingRegisters(1, 1, 45).ToList());
            //右工位测试flag
            //Thread.Sleep(50);
            if (client.ReadBitCIO(0, 6) == 1)
            {
                Right_Status.BackColor = Color.Green;
                Right_Status.Text = "OK";
                g_Right_Test_Status = true;
                //label_scroll.Text = "右工位测试OK";
            }
            else if (client.ReadBitCIO(0, 7) == 1)
            {
                Right_Status.BackColor = Color.Red;
                Right_Status.Text = "NG";
                g_Right_Test_Status = false;
                // label_scroll.Text = "右工位测试NG";
            }
            else if (client.ReadBitCIO(0, 5) == 1)
            {
                Right_Status.BackColor = Color.Transparent;
                Right_Status.Text = "检测中...";
                // label_scroll.Text = "右工测试中...";
            }
            else
            {
                Right_Status.BackColor = Color.Transparent;
                Right_Status.Text = "待检测";
            }
            // SetMsg_right(master_right.ReadHoldingRegisters(1, 1, 45).ToList());
        }
        #endregion

        #region 程序关闭部分
        //工具栏退出按钮
        private void ToolStripMenuItem_Btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定退出？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //停止和PLC的通信
                client.Close();
         
                cts_1?.Cancel();
                MessageBox.Show("线程工厂耗时显示线程关闭成功");
                cts_2?.Cancel();
                MessageBox.Show("线程2关闭成功");
                cts_3?.Cancel();
                MessageBox.Show("线程3关闭成功");
                try
                {
                    serialPort.Close();//关闭左仪器串口
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
                finally 
                {
                    serialPort.Close();//关闭左仪器串口

                }


                try
                {
                    serialPort_right.Close();//关闭右仪器串口
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
                finally 
                {
                    serialPort_right.Close();//关闭右仪器串口 
                }
                
                sTimer.Stop();
                timer1.Stop();
                this.Close();
            }
            else
            {
                return;
            }
        }

        //退出系统按钮
        private void Btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定退出？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //停止和PLC的通信
                client.Close();

                cts_1?.Cancel();
                MessageBox.Show("线程工厂耗时显示线程关闭成功");
                cts_2?.Cancel();
                MessageBox.Show("线程2关闭成功");
                cts_3?.Cancel();
                MessageBox.Show("线程3关闭成功");
                try
                {
                    serialPort.Close();//关闭左仪器串口
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
                finally
                {
                    serialPort.Close();//关闭左仪器串口

                }


                try
                {
                    serialPort_right.Close();//关闭右仪器串口
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
                finally
                {
                    serialPort_right.Close();//关闭右仪器串口 
                }

                sTimer.Stop();
                timer1.Stop();
                this.Close();
            }
            else
            {
                return;
            }
        }
        #endregion

        //系统设置按钮
        private void Btn_system_Click(object sender, EventArgs e)
        {
            ScanShield scanShield = new ScanShield();
            scanShield.ShowDialog();

            if (scanShield.DialogResult == DialogResult.OK)
            {
                Form_system system_form = new Form_system();
                system_form.send_msg += new Form_system.SendMsg(receiveChildMsg);
                system_form.Show();
                
            }
            else
            {
                MessageBox.Show("请输入正确的密码，谢谢");

            }    
        }


       


        private void Btn_Left_Front_Click(object sender, EventArgs e)
        {
            if (client.ReadDM_Short(20) == 0)
            {
                client.WriteDM(51, 1);
                AddLog("左工位气缸前进");
            }
            else 
            {
                MessageBox.Show("需要在手动模式下进行手动操作");
                return;   
            }
            
        }
        //创建标识位用于识别目前的状态
        private void Btn_Left_Back_Click(object sender, EventArgs e)
        {
            if (client.ReadDM_Short(20) == 0)
            {
                client.WriteDM(51, 2);
                AddLog("左工位气缸后退");
            }
            else 
            {
                return;            
            }
           
        }

        private void Btn_Right_Front_Click(object sender, EventArgs e)
        {
            client.WriteDM(50, 1);
            AddLog_Right("右工位气缸前进");
        }

        private void Btn_Right_Back_Click(object sender, EventArgs e)
        {
            client.WriteDM(50, 2);
            AddLog_Right("右工位气缸后退");
        }
        /// <summary>
        /// 判断补码数据的正负
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private bool JX_Judge_PositiveOrNegative(int number)
        {
            if (number != 32767)
            {
                if (number < 32767)
                {
                    return true;
                }
                else if (number > 32767)
                {
                    return false;

                }
                else
                {
                    MessageBox.Show("输入的数据有误，请检查！");
                }
            }
            else 
            {
                return true; 
            }
            return true;
        }
        //右工位数据库写入按钮
/*        private void Btn_database_write_Click(object sender, EventArgs e)
        {
            ScanShield scanShield = new ScanShield();
            scanShield.ShowDialog();

            if (scanShield.DialogResult == DialogResult.OK)
            {
                *//*MessageBoxButtons msgButton_sac_shield = MessageBoxButtons.YesNo;
                DialogResult dr = MessageBox.Show("确定修改扫码屏蔽", "提示", msgButton_sac_shield);*//*
                DialogResult result = MessageBox.Show("确定禁止右工位数据库写入", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    Btn_right_database_write.BackColor = Color.Red;
                    Btn_right_database_write.Text = "数据库写入禁止";
                    //禁止写入执行代码
                    client.WriteDM(27,1);
                    label_scroll.Text = "；右工位数据库禁止写入中...";
                }
                else
                {
                    //返回不执行代码
                    //MessageBox.Show("no");
                    Btn_right_database_write.BackColor = Color.Transparent;
                    Btn_right_database_write.Text = "数据库写入";
                    client.WriteDM(27,0);
                    return;
                }
            }
            else
            {


            }
            *//*  myConnection.Open();
              SqlCommand myCommand = new SqlCommand();
              myCommand.Connection = myConnection;
              myCommand.CommandType = CommandType.Text;

              myCommand.CommandText = @"insert into [dbo.SHProcessProperty](id,bar_no,process_no,do_time) values(@id,@bar_no,@process_no,@do_time)";
              myCommand.Parameters.Add(new SqlParameter("@id",27));
              myCommand.Parameters.Add(new SqlParameter("@bar_no", 'g'));
              myCommand.Parameters.Add(new SqlParameter("@process_no", 'k'));
              myCommand.Parameters.Add(new SqlParameter("@do_time","2022"));
              //提交数据
              myCommand.ExecuteNonQuery();
              //MessageBox.Show("数据库打开成功！");


              myConnection.Close();*//*          
        }*/

        //数据写入到本地数据库方法
        private void database_write(string[] data) 
        {
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.Text;

            myCommand.CommandText = @"insert into [dbo.SHProcessProperty](id,bar_no,process_no,do_time,ok_flag,ng_msg,user_id,flag,eqpt_loc_id,major_state,second_state,aux_state,data001,data002) values(@id,@bar_no,@process_no,@do_time,@ok_flag,@ng_msg,@user_id,@flag,@eqpt_loc_id,@major_state,@second_state,@aux_state,@data001,@data002)";
            myCommand.Parameters.Add(new SqlParameter("@id", data[0]));
            myCommand.Parameters.Add(new SqlParameter("@bar_no", data[1]));
            myCommand.Parameters.Add(new SqlParameter("@process_no", data[2]));
            myCommand.Parameters.Add(new SqlParameter("@do_time", data[3]));
            myCommand.Parameters.Add(new SqlParameter("@ok_flag", data[4]));
            myCommand.Parameters.Add(new SqlParameter("@ng_msg ", data[5]));
            myCommand.Parameters.Add(new SqlParameter("@user_id", data[6]));
            myCommand.Parameters.Add(new SqlParameter("@flag", data[7]));
            myCommand.Parameters.Add(new SqlParameter("@eqpt_loc_id", data[8]));
            myCommand.Parameters.Add(new SqlParameter("@major_state", data[9]));
            myCommand.Parameters.Add(new SqlParameter("@second_state", data[10]));
            myCommand.Parameters.Add(new SqlParameter("@aux_state", data[11]));
            myCommand.Parameters.Add(new SqlParameter("@data001", data[12]));
            myCommand.Parameters.Add(new SqlParameter("@data002", data[13]));
            //提交数据
            try
            {
                var send_to_database = myCommand.ExecuteNonQuery();
                //MessageBox.Show("数据库打开成功！");
                if (send_to_database != -1)
                {
                    AddLog("数据写入成功！");
                }
                else
                {
                    AddLog("数据写入失败！");

                }
            }
            catch { }
            finally
            {
                myConnection.Close();
            }

        }

       
        //添加产品信息按钮
        private void Btn_AddProduct_Click(object sender, EventArgs e)
        {
            Form_Product_Info product_info = new Form_Product_Info();
            //生成窗口前绑定委托传送产品信息事件
            product_info.send_product_info += new Form_Product_Info.SendProductInfo(receiveChildProductInfo);
            product_info.Show();
        }
        //接受修改产品信息窗口发送过来的数据
        private void receiveChildProductInfo(string msg)
        {
            Product_Info.Text = msg;
        }

    

        private void AddRows_Left(Command_str command)
        {
            object[] rows = {
                command_str.Command_count,
                command_str.Command_time,
                command_str.Command_test_status,
                command_str.Command_code,
                command_str.Command_Pressure,
                command_str.Command_Q

            };
            //dataGridView_Left.Rows.Add(rows);
        }

        private void AddRows_Right(Command_str command)
        {
            object[] rows = {
                command_str.Command_count,
                command_str.Command_time,
                command_str.Command_test_status,
                command_str.Command_code,
                command_str.Command_Pressure,
                command_str.Command_Q
            };
            //dataGridView_Right.Rows.Add(rows);
        }
 
        //工具栏进入系统设置
        private void ToolStripMenuItem_Btn_system_Click(object sender, EventArgs e)
        {
            ScanShield scanShield = new ScanShield();
            scanShield.ShowDialog();
          

            if (scanShield.DialogResult == DialogResult.OK)
            {
                //可以通过构造器实现对子窗口传送数据信息
                Form_system system_form = new Form_system(); ;  //可以通过构造方法把值传给系统设置页面
                system_form.send_msg += new Form_system.SendMsg(receiveChildMsg);
                system_form.Show();
                
            }
            else
            {
                MessageBox.Show("请输入正确的密码，谢谢");

            }
        }
        #region 数据禁止写入
        //工具栏左工位数据写入禁止
        private void ToolStripMenuItem_Btn_left_database_write_Click(object sender, EventArgs e)
        {
            ScanShield scanShield = new ScanShield();
            scanShield.ShowDialog();

            if (scanShield.DialogResult == DialogResult.OK)
            {
                /*MessageBoxButtons msgButton_sac_shield = MessageBoxButtons.YesNo;
                DialogResult dr = MessageBox.Show("确定修改扫码屏蔽", "提示", msgButton_sac_shield);*/
                DialogResult result = MessageBox.Show("确定禁止左工位数据库写入", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    //Btn_left_database_write.BackColor = Color.Red;
                    //Btn_left_database_write.Text = "数据库写入禁止";
                    //禁止写入执行代码
                    client.WriteDM(26, 1);
                    panel_left_database_write.BackColor = Color.Red;
                   
                }
                else
                {
                    //返回不执行代码
                    //MessageBox.Show("no");
                    //Btn_left_database_write.BackColor = Color.Transparent;
                    //Btn_left_database_write.Text = "数据库写入";
                    client.WriteDM(26, 0);
                    panel_left_database_write.BackColor = Color.Transparent;
                    return;
                }
            }
            else
            {


            }
        }
        //工具栏右工位数据写入禁止
        private void ToolStripMenuItem_Btn_right_database_write_Click(object sender, EventArgs e)
        {
            ScanShield scanShield = new ScanShield();
            scanShield.ShowDialog();

            if (scanShield.DialogResult == DialogResult.OK)
            {
                /*MessageBoxButtons msgButton_sac_shield = MessageBoxButtons.YesNo;
                DialogResult dr = MessageBox.Show("确定修改扫码屏蔽", "提示", msgButton_sac_shield);*/
                DialogResult result = MessageBox.Show("确定禁止右工位数据库写入", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    //ToolStripMenuItem_Btn_right_database_write_Click.BackColor = Color.Red;
                    //ToolStripMenuItem_Btn_right_database_write_Click.Text = "数据库写入禁止";
                    //禁止写入执行代码
                    //client.WriteDM(27, 1);
                    panel_right_database_write.BackColor = Color.Red;
                    
                }
                else
                {
                    //返回不执行代码
                    //MessageBox.Show("no");
                   // ToolStripMenuItem_Btn_right_database_write_Click.BackColor = Color.Transparent;
                    //ToolStripMenuItem_Btn_right_database_write_Click.Text = "数据库写入";
                    //client.WriteDM(27, 0);
                    panel_right_database_write.BackColor = Color.Transparent;
                    return;
                }
            }
            else
            {


            }
        }
        #endregion

        //报警复位按钮
        private void Btn_error_rst_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确认现场设备情况执行复位", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //给D29值置1 报警总复位
                client.WriteDM(29, 1);
                
            }
            else
            {
                
                return;
            }
        }
        #region 产量清零
        //左工位产量清零
        private void Btn_Lefe_Reset_Counter_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定清除左工位数量？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (master.ReadHoldingRegisters(1, 3, 1)[0] != 0)  //对左工位数量判断是否为0
                {
                    //往左仪器写入1 清除生产数量
                    if (serialPort.IsOpen)
                    {
                        master.WriteSingleRegister(1, 106, 1);
                        AddLog("左工位测试数量清零完成");                     
                    }
                }
                else 
                {
                    return;
                }
               /* //往左仪器写入1 清除生产数量
                if (serialPort.IsOpen) 
                {
                    master.WriteSingleRegister(1, 106, 1);
                    AddLog("左工位测试数量清零完成");
                }*/
            }
            else 
            {
                return;
            }            
        }

        //右工位产量清零
        private void Btn_Right_Reset_Counter_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定清除右工位数量？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (master_right.ReadHoldingRegisters(1, 3, 1)[0] != 0)
                {
                    if (serialPort_right.IsOpen)
                    {
                        master_right.WriteSingleRegister(1, 106, 1);
                        AddLog_Right("右工位测试数量清零完成");                      
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {            
                return;
            }
        }
        #endregion

        //工具栏启动测试按钮
        private void ToolStripMenuItem_Btn_Start_Click(object sender, EventArgs e)
        {
            //实例化取消触发器 
            cts_1 = new CancellationTokenSource();
            cts_2 = new CancellationTokenSource();
            cts_3 = new CancellationTokenSource();
            //线程1 主要是用来实现刷新耗时读取数据
            /*Task task1 = Task.Factory.StartNew(new Action(() => 
             {
                 while (!cts_1.IsCancellationRequested)
                 {
                    //RefreshData(); //刷新数据
            
                     Thread.Sleep(100); 
                 }                    
             }),cts_1.Token);*/

            Task task3 = new Task(() =>
            {
                //this.Invoke(new Action(() => {               
                while (true)
                {
                    GetHoldingRegisters();
                    GetDatas();

                    Thread.Sleep(100);
                    Application.DoEvents();
                }
                //}));

            }, cts_3.Token);
            task3.Start();
            //线程2启动

            Thread.Sleep(2000);

            //创建线程实例2
            Task task2 = new Task(() =>
                {                         
                    while (true)
                    {
                        showTest1();
                        RefreshUI();
                                      
                        Thread.Sleep(100);
                        Application.DoEvents();
                    }
            }, cts_2.Token);
            task2.Start();  //线程2启动



        }

        bool testWriteDb = true;
        //显示左工位条码信息
        private void LeftCodeInfo()
        {
            string str_y1 = str_y.ToString("x");
            string str_z1 = str_z.ToString("x");
            string str_left_code_value3_3 = str_left_code_value3.ToString("x");
            string str_left_code_value4_4 = str_left_code_value4.ToString("x");
            string str_left_code_value5_5 = str_left_code_value5.ToString("x");
            string str_left_code_value6_6 = str_left_code_value6.ToString("x");


            int start = 0;
            try
            {
                string s = JX_HexToASCII.HexStringToASCLL(str_y1.Trim().Substring(start, str_y1.Length - 2));
                string m = JX_HexToASCII.HexStringToASCLL(str_y1.Substring(start + 2, 2));

                string s1 = JX_HexToASCII.HexStringToASCLL(str_z1.Substring(start, str_z1.Length - 2));
                string m1 = JX_HexToASCII.HexStringToASCLL(str_z1.Substring(start + 2, 2));

                string s2 = JX_HexToASCII.HexStringToASCLL(str_left_code_value3_3.Substring(start, str_left_code_value3_3.Length - 2));
                string m2 = JX_HexToASCII.HexStringToASCLL(str_left_code_value3_3.Substring(start + 2, 2));

                string s3 = JX_HexToASCII.HexStringToASCLL(str_left_code_value4_4.Substring(start, str_left_code_value4_4.Length - 2));
                string m3 = JX_HexToASCII.HexStringToASCLL(str_left_code_value4_4.Substring(start + 2, 2));

                string s4 = JX_HexToASCII.HexStringToASCLL(str_left_code_value5_5.Substring(start, str_left_code_value5_5.Length - 2));
                string m4 = JX_HexToASCII.HexStringToASCLL(str_left_code_value5_5.Substring(start + 2, 2));

                string s5 = JX_HexToASCII.HexStringToASCLL(str_left_code_value6_6.Substring(start, str_left_code_value6_6.Length - 2));
                string m5 = JX_HexToASCII.HexStringToASCLL(str_left_code_value6_6.Substring(start + 2, 2));
                
                this.Invoke((EventHandler)delegate
                {
                    Left_Code.Text = s.ToString() + m.ToString() + s1.ToString() + m1.ToString() + s2.ToString()
                    + m2.ToString() + s3.ToString() + m3.ToString() + s4.ToString() + m4.ToString() + s5.ToString()
                    + m5.ToString();
                });
                if (testWriteDb)
                {
                    testWriteDb = false;
                    database_write(new string[14]
                    {
                        "1",
                        "str2" ,
                        g_Right_Programmer.ToString(),
                        System.DateTime.Now.ToString(),
                        "true",
                        "no",
                        "JX_001",
                        "4",
                        "5",
                        "6",
                        "7",
                        "8",
                        "15.235",
                        "123"
                     });
                }

            }
            catch (Exception ex)
            {

           
            }
        }
        //显示右工位条码信息
        private void RightCodeInfo()
        {
          /*  ushort[] buffer = new ushort[32];
            bool result = client.ReadDMs(354,ref buffer, 30);
            int length = buffer.Length;
            
            string str = JX_HexToASCII.HexStringToASCLL(((buffer[0] * 256) + buffer[1]).ToString());
            Right_Code.Text = str;*/

            //short str_y_right = client.ReadDM_Short(354);
            //short str_z_right = client.ReadDM_Short(355);
            //short str_right_code_value3 = client.ReadDM_Short(356);
            //short str_right_code_value4 = client.ReadDM_Short(357);
            //short str_right_code_value5 = client.ReadDM_Short(358);
            //short str_right_code_value6 = client.ReadDM_Short(359);

            //short str_right_code_value7 = client.ReadDM_Short(360);
            //short str_right_code_value8 = client.ReadDM_Short(361);
            //short str_right_code_value9 = client.ReadDM_Short(362);
            //short str_right_code_value10 = client.ReadDM_Short(363);

            string str_y1_right = str_y_right.ToString("x");
            string str_z1_right = str_z_right.ToString("x");
            string str_right_code_value3_3 = str_right_code_value3.ToString("x");
            string str_right_code_value4_4 = str_right_code_value4.ToString("x");
            string str_right_code_value5_5 = str_right_code_value5.ToString("x");
            string str_right_code_value6_6 = str_right_code_value6.ToString("x");

            string str_right_code_value7_7 = str_right_code_value3.ToString("x");
            string str_right_code_value8_8 = str_right_code_value4.ToString("x");
            string str_right_code_value9_9 = str_right_code_value5.ToString("x");
            string str_right_code_value10_10 = str_right_code_value6.ToString("x");

            int start_right = 0;
            try
            {
                string s_right = JX_HexToASCII.HexStringToASCLL(str_y1_right.Substring(start_right, str_y1_right.Length - 2));
                string m_right = JX_HexToASCII.HexStringToASCLL(str_y1_right.Substring(start_right + 2, 2));
                //string m = str_y1.Substring(start + 2, str_y1.Length - 0);
                string s1_right = JX_HexToASCII.HexStringToASCLL(str_z1_right.Substring(start_right, str_z1_right.Length - 2));
                string m1_right = JX_HexToASCII.HexStringToASCLL(str_z1_right.Substring(start_right + 2, 2));

                string s2_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value3_3.Substring(start_right, str_right_code_value3_3.Length - 2));
                string m2_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value3_3.Substring(start_right + 2, 2));

                string s3_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value4_4.Substring(start_right, str_right_code_value4_4.Length - 2));
                string m3_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value4_4.Substring(start_right + 2, 2));

                string s4_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value5_5.Substring(start_right, str_right_code_value5_5.Length - 2));
                string m4_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value5_5.Substring(start_right + 2, 2));

                string s5_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value6_6.Substring(start_right, str_right_code_value6_6.Length - 2));
                string m5_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value6_6.Substring(start_right + 2, 2));

                string s6_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value7_7.Substring(start_right, str_right_code_value7_7.Length - 2));
                string m6_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value7_7.Substring(start_right + 2, 2));

                string s7_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value8_8.Substring(start_right, str_right_code_value8_8.Length - 2));
                string m7_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value8_8.Substring(start_right + 2, 2));

                string s8_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value9_9.Substring(start_right, str_right_code_value9_9.Length - 2));
                string m8_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value9_9.Substring(start_right + 2, 2));

                string s9_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value10_10.Substring(start_right, str_right_code_value10_10.Length - 2));
                string m9_right = JX_HexToASCII.HexStringToASCLL(str_right_code_value10_10.Substring(start_right + 2, 2));

                this.Invoke((EventHandler)delegate
                {
                    Right_Code.Text = s_right.ToString() + m_right.ToString() + s1_right.ToString() + m1_right.ToString()
                                   + s2_right.ToString() + m2_right.ToString() + s3_right.ToString() + m3_right.ToString()
                                   + s4_right.ToString() + m4_right.ToString() + s5_right.ToString() + m5_right.ToString()
                                   + s6_right.ToString() + m6_right.ToString() + s7_right.ToString() + m7_right.ToString()
                                   + s8_right.ToString() + m8_right.ToString() + s9_right.ToString() + m9_right.ToString();
                });
            }
            catch (Exception)
            {


            }
          
            //AddRows_Right(new Command_str("", DateTime.Now.ToString(),g_Left_Test_Status?"OK":"NG", Right_Code.Text, "", ""));
        }

        //统计2个工位的产量和产品良率
        private void Output() 
        {
            int left = Convert.ToInt16(g_left_counter);
            //int left = int.Parse(g_left_counter);
            //int right = int.Parse(g_right_counter);
            int right = Convert.ToInt16(g_right_counter);
            int count_total = 0;
    
            count_total = left + right;

            //int left_ok = int.Parse(g_left_ok);
            //int right_ok = int.Parse(g_right_ok);
            int left_ok = Convert.ToInt16(g_left_ok);
            int right_ok = Convert.ToInt16(g_right_ok);

            int ok_total = left_ok + right_ok;
            double yield = 0;
            if (count_total > 0)
            {
                yield = ok_total * 100 / count_total;
            }
            else 
            {
                return;
            }
            //double yield = ok_total * 100 / count_total;

            Total_Count.Text = count_total.ToString();
            Yield.Text = yield.ToString() + "%";
        }

        //左工位日志显示
        private void AddLog(string info) 
        {
            this.listBox_Info.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"  " + info + Environment.NewLine);
        }
        //右工位日志测试日志
        private void AddLog_Right(string info)
        {
            this.listBox_Info_Right.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + info + Environment.NewLine);
        }
        //启动手动操作
        private void ToolStripMenuItem_Manual_Open_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确认周围环境，打开手动操作","提示",MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (client == null)
                {
                    AddLog("t通信未连接");
                }
                client.WriteDM(31, 1);  //PLC D20值 1 位自动运行模式
            }
            else {

                return;
            }
            
           

        }

        private void ToolStripMenuItem_Manual_CLOSE_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("确定关闭手动操作","提示",MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                client.WriteDM(31, 0);
            }
            else 
            {
                return;
            
            }
        }

        private void 点检设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //左点检窗口
        private void ToolStripMenuItem_L_Check_Click(object sender, EventArgs e)
        {
            //点击进行点检测试 即启动进行正常测试，测试结果保存
            client.WriteDM(34,1);  
            Form_L_Check l_check = new Form_L_Check();
            l_check.Show();
        }
        //右点检窗口
        private void ToolStripMenuItem_R_Check_Click(object sender, EventArgs e)
        {
            //点检测试启动正常测试 测试结果进行保存
            client.WriteDM(35,1);
            Form_R_Check r_check = new Form_R_Check();
            r_check.Show();
        }
        //工具栏about
        private void ToolStripMenuItem_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("压差法气密性测试设备 -- 先机智能科技","关于about",MessageBoxButtons.OK);
        }


        //左工位扫码重复判断，是在100个条码的容量内判断
/*        private bool Left_VerifyCode(string newcode)
        {
            foreach (var item in Left_CodeList)
            {
                if (item == newcode)
                {
                    return false;
                }
            }
            if (Left_CodeList.Count <= Code_MaxLength)
            {
                this.Left_CodeList.Add(newcode);
            }
            else
            {
                this.Left_CodeList.RemoveAt(0);
                this.Left_CodeList.Add(newcode);
            }
            return true;
        }*/

        //右工位扫码重复判断，是在100个条码的容量内判断
/*        private bool Right_VerifyCode(string newcode)
        {
            foreach (var item in Right_CodeList)
            {
                if (item == newcode)
                {
                    return false;
                }
            }
            if (Right_CodeList.Count <= Code_MaxLength)
            {
                this.Right_CodeList.Add(newcode);
            }
            else
            {
                this.Right_CodeList.RemoveAt(0);
                this.Right_CodeList.Add(newcode);
            }
            return true;
        }*/

        private void ToolStripMenuItem_Btn_left_scan_shield_Click(object sender, EventArgs e)
        {
            ScanShield scanShield = new ScanShield();
            scanShield.ShowDialog();

            if (scanShield.DialogResult == DialogResult.OK)
            {
                /*MessageBoxButtons msgButton_sac_shield = MessageBoxButtons.YesNo;
                DialogResult dr = MessageBox.Show("确定修改扫码屏蔽", "提示", msgButton_sac_shield);*/
                DialogResult result = MessageBox.Show("确定修改扫码屏蔽", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    //Btn_scan_shield.BackColor = Color.Red;
                    // Btn_scan_shield.Text = "扫码屏蔽中";
                    //禁止写入执行代码
                    client.WriteDM(24, 1);
                   
                    panel_left_scan_shield.BackColor = Color.Red;
                   
                }
                else
                {
                    //返回不执行代码
                    //MessageBox.Show("no");
                    //Btn_scan_shield.BackColor = Color.Transparent;
                    //Btn_scan_shield.Text = "扫码屏蔽";

                    client.WriteDM(24, 0);
            
                    panel_left_scan_shield.BackColor = Color.Transparent;
                   
                    return;
                }
            }
            else
            {
                return;
            }

        }

        private void ToolStripMenuItem_Btn_right_scan_shield_Click(object sender, EventArgs e)
        {
            ScanShield scanShield = new ScanShield();
            scanShield.ShowDialog();

            if (scanShield.DialogResult == DialogResult.OK)
            {
                /*MessageBoxButtons msgButton_sac_shield = MessageBoxButtons.YesNo;
                DialogResult dr = MessageBox.Show("确定修改扫码屏蔽", "提示", msgButton_sac_shield);*/
                DialogResult result = MessageBox.Show("确定修改扫码屏蔽", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    //Btn_scan_shield.BackColor = Color.Red;
                    // Btn_scan_shield.Text = "扫码屏蔽中";
                    //禁止写入执行代码
                    
                    client.WriteDM(25, 1);
                   
                    panel_right_scan_shield.BackColor = Color.Red;
                }
                else
                {
                    //返回不执行代码
                    //MessageBox.Show("no");
                    //Btn_scan_shield.BackColor = Color.Transparent;
                    //Btn_scan_shield.Text = "扫码屏蔽";

                   
                    client.WriteDM(25, 0);
                
                    panel_right_scan_shield.BackColor = Color.Transparent;
                    return;
                }
            }
            else
            {
                return;
            }

        }

        //开始前读取串口配置信息
        private InstrumentParam Get_serial_Config(string Path) 
        {
            if (File.Exists(Path))
            {
                return null;
            }
            InstrumentParam param = new InstrumentParam();

            try
            {
                param.ProtName = IniConfigHelper.ReadIniData("泄露仪串口参数", "串口号", "", Path);
                param.BaudRate = Convert.ToInt32(IniConfigHelper.ReadIniData("泄露仪串口参数", "波特率", "", Path));
                param.DataBits = Convert.ToInt32(IniConfigHelper.ReadIniData("泄露仪串口参数", "数据位", "", Path));
                param.StopBits = (StopBits)Enum.Parse(typeof(StopBits), IniConfigHelper.ReadIniData("泄露仪串口参数", "停止位", "", Path));
                param.Parity = (Parity)Enum.Parse(typeof(Parity), IniConfigHelper.ReadIniData("泄露仪串口参数", "校验位", "", Path));

                return param;
            }
            catch (Exception)
            {
                AddLog("读取泄露仪串口参数出错，请确认配置文件");
                return null;
            }          
        }
        //实现对数据库的查询，并显示出来
        private void ToolStripMenuItem_database_query_Click(object sender, EventArgs e)
        {
            //生成数据查询页面实例，把数据库连接的对象通过构造器传到数据查询软件
            Form_DatabaseQuery dataQuery = new Form_DatabaseQuery(myConnection);
            //显示数据查询页面
            dataQuery.Show();
        }

        private void ToolStripMenuItem_Database_Connect_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("数据库已经连接成功","提示",MessageBoxButtons.OK);
            if (result == DialogResult.Yes)
            {
                return;
            }
        }

        //新建一个数据表对象
        DataTable t = new DataTable();
        
        //左工位数据导出，导出是需要先打开指定文件，然后选择需要导出到的文件
        private void ToolStripMenuItem_Left_Dataexport_Click(object sender, EventArgs e)
        {
            //这里需要对表格的相关数据进行填充
            t.Columns.Add("123");
            t.Rows.Add("456");
            ExportDataToExcel( t , "C:\\Users\\Lenovo\\Desktop\\Left_Data.xls");
        }
        //右工位数据导出
        private void ToolStripMenuItem_Right_Dataexport_Click(object sender, EventArgs e)
        {
           
            t.Columns.Add("1900");
            t.Rows.Add("4996");
            ExportDataToExcel(t, "C:\\Users\\Lenovo\\Desktop\\Right_Data.xls");
        }

        
        //数据导出到Excel
        private void ExportDataToExcel(DataTable TableName,string FileName) 
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "导出Excel文件";
            saveFileDialog.Filter = "Microsoft Office Excel工作簿（*.xls)|*.xls";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.AddExtension = true;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = FileName;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string localFilePath = saveFileDialog.FileName.ToString();
                int TotalCount;
                int RowRead = 0;
                int Percent = 0;

                TotalCount = TableName.Rows.Count;

                Stream myStream = saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(myStream,Encoding.GetEncoding("gb2312"));
                string strHead = "";

                Stopwatch timerWatch = new Stopwatch();
                timerWatch.Start();

                try
                {
                    for (int i = 0; i < TableName.Columns.Count; i++)
                    {
                        if (i > 0)
                        {
                            strHead += "\t";
                        }
                        strHead += TableName.Columns[i].ColumnName.ToString();
                    }

                    sw.WriteLine(strHead);

                    for (int t = 0; t < TableName.Rows.Count; t++)
                    {
                        RowRead++;
                        Percent = (int)(100 * RowRead / TotalCount);
                        Application.DoEvents();

                        string strData = "";
                        for (int j = 0; j < TableName.Columns.Count; j++)
                        {
                            if (j > 0)
                            {
                                strData = "\t";
                            }
                            strData += TableName.Rows[t][j].ToString();
                        }
                        sw.WriteLine(strData);
                    }
                    sw.Close();
                    myStream.Close();

                    timerWatch.Reset();
                    timerWatch.Stop();
                }

                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally 
                {
                    sw.Close();
                    myStream.Close();
                  
                    timerWatch.Stop();
                }
                if (MessageBox.Show("导出成功，是否立即打开？","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.OK)
                {
                    System.Diagnostics.Process.Start("C:\\Users\\Lenovo\\Desktop\\Left_Data.xls");
                }
            }       
        }
        //操作说明显示
        private void ToolStripMenuItem_OperatingInstructions_Click(object sender, EventArgs e)
        {
            Form_OperatingInstructions oi = new Form_OperatingInstructions();
            oi.Show();
        }

        private void sTimer_Tick(object sender, EventArgs e)
        {
            softRunTimeToolStripStatusLabel.Text = "软件运行时间：" + softRunWatch.Elapsed.ToString(@"hh\:mm\:ss");
            workRunTimeToolStripStatusLabel.Text = "" + workRunWatch.Elapsed.ToString(@"hh\:mm\:ss\:ffff");
        }

        private void Btn_start_Click(object sender, EventArgs e)
        {
            //判断启动条件是否满足
            
            //启动清空左右日志显示信息
            if (listBox_Info.Items.Count > 0 )
            {
                listBox_Info.Items.Clear();
            }
            if (listBox_Info_Right.Items.Count > 0)
            {
                listBox_Info_Right.Items.Clear();
            }

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Output();
        }

        private void Btn_Check_Right_Click(object sender, EventArgs e)
        {
            AddLog_Right("右工位开始点检测试...");

            Thread.Sleep(1000);
            AddLog_Right("点检完成");
        }

        private void Btn_Check_Left_Click(object sender, EventArgs e)
        {
            AddLog("左工位开始点检测试...");
            Thread.Sleep(1000);
            AddLog("点检完成");
        }




        #region 2日志窗口跟随新数据显示
        private void listBox_Info_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }

        private void listBox_Info_Right_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion



        List<ushort> LeftHoldingRegisters = new List<ushort>();
        List<ushort> RightHoldingRegisters = new List<ushort>();

        private void RefreshUI()
        {
            this.Invoke((EventHandler) delegate
            {
                #region Left

                Pressure_Value.Text = str_array_left[13];
                Left_NG_Count.Text = str_array_left[0];
                Left_Total_Count.Text = str_array_left[2];
                //Total_Count.Text = Left_Total_Count.Text + "0";
                Left_OK_Count.Text = (string)JX_strTOintTOstr(str_array_left[2], str_array_left[0]);
                Left_Programmer.Text = str_array_left[29];
                //comboBox_Left_ProNum.Text = Left_Programmer.Text;
                Left_Test_result_Pressure.Text = JX_FUNCTION_DecimalToBinary_right(int.Parse(str_array_left[38]));
                //Left_Test_result_Q.Text = str_array[41] + "+" + str_array[42];
                Left_Test_result_Q.Text = JX_FUNCTION_DecimalToBinary_right(int.Parse(str_array_left[40]));

                #endregion

                #region Right

                Right_NG_Count.Text = str_array_right[0];
                Right_Total_Count.Text = str_array_right[2];
                try
                {
                    if (str_array_right[2] == null && str_array_right[0] == null)
                    {
                        return;
                    }
                    else
                    {
                        int str1 = int.Parse(str_array_right[2]);
                        int str2 = int.Parse(str_array_right[0]);
                        //int r = Convert.ToInt32(str_array[2]);
                        //int b = Convert.ToInt32(str_array[0]);
                        Right_OK_Count.Text = (str1 - str2).ToString();

                    }
                }
                catch
                {
                    MessageBox.Show("读取错误");
                    throw;
                }
                Right_OK_Count.Text = (string)JX_strTOintTOstr(str_array_right[2], str_array_right[0]);
                Right_Programmer.Text = str_array_right[29];
                Right_Test_result_Pressure.Text = JX_FUNCTION_DecimalToBinary_right(int.Parse(str_array_right[38]));
                Right_Test_result_Q.Text = JX_FUNCTION_DecimalToBinary_right_2bit(int.Parse(str_array_right[40]));

                #endregion

                #region Status

                if (CIO3 == 1)
                {
                    Left_Status.BackColor = Color.Green;
                    Left_Status.Text = "OK";
                    g_Left_Test_Status = true;
                }
                else if (CIO4 == 1)
                {
                    Left_Status.BackColor = Color.Red;
                    Left_Status.Text = "NG";
                    g_Left_Test_Status = false;

                }
                else if (CIO2 == 1)
                {
                    Left_Status.BackColor = Color.Transparent;
                    Left_Status.Text = "检测中...";
                }
                else
                {
                    Left_Status.BackColor = Color.Transparent;
                    Left_Status.Text = "待检测";
                }
                if (CIO6 == 1)
                {
                    Right_Status.BackColor = Color.Green;
                    Right_Status.Text = "OK";
                    g_Right_Test_Status = true;
                    //label_scroll.Text = "右工位测试OK";
                }
                else if (CIO7 == 1)
                {
                    Right_Status.BackColor = Color.Red;
                    Right_Status.Text = "NG";
                    g_Right_Test_Status = false;
                    // label_scroll.Text = "右工位测试NG";
                }
                else if (CIO5 == 1)
                {
                    Right_Status.BackColor = Color.Transparent;
                    Right_Status.Text = "检测中...";
                    // label_scroll.Text = "右工测试中...";
                }
                else
                {
                    Right_Status.BackColor = Color.Transparent;
                    Right_Status.Text = "待检测";
                }

                #endregion
            });


        }
        private void showTest1()
        {

            SetMsg(LeftHoldingRegisters);
            LeftCodeInfo();
            SetMsg_right(RightHoldingRegisters);
            RightCodeInfo();


        }

        private void GetHoldingRegisters()
        {
            LeftHoldingRegisters = master.ReadHoldingRegisters(1, 1, 45).ToList();
            RightHoldingRegisters = master_right.ReadHoldingRegisters(1, 1, 45).ToList();
        }


        #region 数据定义
        short str_y = 0;
        short str_z = 0;
        short str_left_code_value3 = 0;
        short str_left_code_value4 = 0;
        short str_left_code_value5 = 0;
        short str_left_code_value6 = 0;

        short str_y_right = 0;
        short str_z_right = 0;
        short str_right_code_value3 = 0;
        short str_right_code_value4 = 0;
        short str_right_code_value5 = 0;
        short str_right_code_value6 = 0;
        short str_right_code_value7 = 0;
        short str_right_code_value8 = 0;
        short str_right_code_value9 = 0;
        short str_right_code_value10 = 0;

        int CIO2 = 0;
        int CIO3 = 0;
        int CIO4 = 0;
        int CIO5 = 0;
        int CIO6 = 0;
        int CIO7 = 0;
        #endregion

        private void GetDatas()
        {
            #region Left
             str_y = client.ReadDM_Short(304);
             str_z = client.ReadDM_Short(305);
             str_left_code_value3 = client.ReadDM_Short(306);
             str_left_code_value4 = client.ReadDM_Short(307);
             str_left_code_value5 = client.ReadDM_Short(308);
             str_left_code_value6 = client.ReadDM_Short(309);
            #endregion
            #region Right
             str_y_right = client.ReadDM_Short(354);
             str_z_right = client.ReadDM_Short(355);
             str_right_code_value3 = client.ReadDM_Short(356);
             str_right_code_value4 = client.ReadDM_Short(357);
             str_right_code_value5 = client.ReadDM_Short(358);
             str_right_code_value6 = client.ReadDM_Short(359);

             str_right_code_value7 = client.ReadDM_Short(360);
             str_right_code_value8 = client.ReadDM_Short(361);
             str_right_code_value9 = client.ReadDM_Short(362);
             str_right_code_value10 = client.ReadDM_Short(363);
            #endregion

            CIO2 = client.ReadBitCIO(0, 2);
            CIO3 = client.ReadBitCIO(0, 3);
            CIO4 = client.ReadBitCIO(0, 4);
            CIO5 = client.ReadBitCIO(0, 5);
            CIO6 = client.ReadBitCIO(0, 6);
            CIO7 = client.ReadBitCIO(0, 7);
        }



        private void Btn_mode_Click(object sender, EventArgs e)
        {
            if (ModeChange == false)
            {
                Btn_mode.Text = "自动模式";
                ModeChange = true;
                onProcessNameToolStripStatusLabel.Text = "当前模式：自动模式";
            }
            else {
                Btn_mode.Text = "手动模式";
                ModeChange = false;
                onProcessNameToolStripStatusLabel.Text = "当前模式：手动模式";
            }
        }
    }
}

 