using mcOMRON;
using SANHUA_TCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace SANHUA_MAIN
{
    public partial class Form_system : Form
    {
       
        public delegate void SendMsg(string msg);
        public event SendMsg send_msg;

        public Form_system()
        {
            InitializeComponent();
            InitGridView_Left_Data();
            
        }

        private void Form_system_Load(object sender, EventArgs e)
        {
            //读取配置文件中的通讯数据
           // path = Application.StartupPath + @"\config.ini";
           // TxtIP = OmronPLC.IniFile.ReadIniData("PlcComTCP", "IP", "", path);
           // Port = OmronPLC.IniFile.ReadIniData("PlcComTCP", "Port", "", path);

            //往配置文件写入IP和端口
           // OmronPLC.IniFile.WriteIniData("PlcComTCP", "IP", "", path);
            //OmronPLC.IniFile.WriteIniData("PlcComTCP", "Port", "", path);

            string local_ip = "192.168.250.100";
            string local_port = "";
            Local_Ip.Text = local_ip;
            Local_Port.Text = local_port;
            string plc_ip = "192.168.250.1";
            string plc_port = "9600";
            Plc_Ip.Text = plc_ip;
            Plc_Port.Text = plc_port;
            tb_left_delay.Text = "5000ms";
            tb_right_delay.Text = "5000ms";




            string[] PortList = SerialPort.GetPortNames();
            if (PortList.Length > 0)
            {
                cmb_Port.DataSource = PortList;
            }
            cmb_BaudRate.DataSource = new string[] {"4800","9600","11520","19200" };

         

            this.cmb_Parity.DataSource = Enum.GetNames(typeof(Parity));

            this.cmb_StopBits.DataSource = Enum.GetNames(typeof(StopBits));
            
            //从主窗口中获取取左右气缸延时的数值
           // Form_main form_main = new Form_main();
            //SendToSystem sendToSystem = form_main.Main_Get_Delay_Value;

            //int left_delay_value = sendToSystem();
            //tb_left_delay.Text = left_delay_value.ToString();

            //从主窗口中获取左右泄露仪数据
           //SendLeftDataToSystem sendLeftData_system = form_main.SendLeftData;
           //SendRightDataToSystem sendRightData_system = form_main.SendRightData;
           //T1.Text = sendLeftData_system.ToString();
           //T2.Text = sendRightData_system.ToString();


        }

        private void InitGridView_Left_Data() {
            dataGridView_Left_Data.ColumnCount = 23;
            dataGridView_Left_Data.Columns[0].Name = "包头";
            dataGridView_Left_Data.Columns[1].Name = "功能校验码";
            dataGridView_Left_Data.Columns[2].Name = "工序号";
            dataGridView_Left_Data.Columns[3].Name = "工位";
            dataGridView_Left_Data.Columns[4].Name = "员工条码";
            dataGridView_Left_Data.Columns[5].Name = "条码1长度";
            dataGridView_Left_Data.Columns[6].Name = "条码1";
            dataGridView_Left_Data.Columns[7].Name = "条码2长度";
            dataGridView_Left_Data.Columns[8].Name = "条码2";
            dataGridView_Left_Data.Columns[9].Name = "合格标志";
            dataGridView_Left_Data.Columns[10].Name = "参数1";
            dataGridView_Left_Data.Columns[11].Name = "参数2";
            dataGridView_Left_Data.Columns[12].Name = "参数3";
            dataGridView_Left_Data.Columns[13].Name = "参数4";
            dataGridView_Left_Data.Columns[14].Name = "参数5";
            dataGridView_Left_Data.Columns[15].Name = "参数6";
            dataGridView_Left_Data.Columns[16].Name = "参数7";
            dataGridView_Left_Data.Columns[17].Name = "参数8"; 
            dataGridView_Left_Data.Columns[18].Name = "参数9";
            dataGridView_Left_Data.Columns[19].Name = "参数10";
            dataGridView_Left_Data.Columns[20].Name = "参数11";
            dataGridView_Left_Data.Columns[21].Name = "参数12";

           // dataGridView_Left_Data.Rows.Add("左工位");
            //dataGridView_Left_Data.Rows[0].Selected = false;
            ushort[] a1 = { 0,1,2};
            ushort[] a2 = {3,4,5 };
            //AddRows(new Command(55, 00, 00, 01, 00, 10,a1,10,a2,1));


        }
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
            dataGridView_Left_Data.Rows.Add(rows);


        }

        public void button24_Click(object sender, EventArgs e)
        {

            send_msg("左工位气缸报警解除");
            
        }

        private string Serial_Path = Application.StartupPath + "\\serial_port_config.ini";
        private void Btn_WriteSerial_Click(object sender, EventArgs e)
        {
            bool result = true;
            result &= IniConfigHelper.WriteIniData("泄露仪串口参数", "串口号", this.cmb_Port.Text, Serial_Path);
            result &= IniConfigHelper.WriteIniData("泄露仪串口参数", "波特率", this.cmb_BaudRate.Text, Serial_Path);
            result &= IniConfigHelper.WriteIniData("泄露仪串口参数", "校验位", this.cmb_Parity.Text, Serial_Path);
            result &= IniConfigHelper.WriteIniData("泄露仪串口参数", "数据位", this.txt_DataBits.Text, Serial_Path);
            result &= IniConfigHelper.WriteIniData("泄露仪串口参数", "停止位", this.cmb_StopBits.Text, Serial_Path);

            MessageBox.Show(result?"保存成功":"保存失败","仪器串口号设置");
        }

 
    }
}
