using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SANHUA_MAIN
{
    //数据显示
    class Command_str
    {
        private string command_count;                   
        private string command_time;
        private string   command_test_status;
        private string command_code;                 
        private string command_Pressure;
        private string command_Q;



        public Command_str() { }

        public Command_str(string command_count,
            string command_time,
            string command_test_status,
            string command_code,
            string command_Pressure,
            string command_Q)
        {
            this.command_count = command_count;
            this.command_time = command_time;
            this.command_test_status = command_test_status;
            this.command_code = command_code;
            this.command_Pressure = command_Pressure;
            this.command_Q = command_Q;
        }

        public string Command_count { get => command_count; set => command_count = value; }
        public string Command_time { get => command_time; set => command_time = value; }
        public string Command_test_status { get => command_test_status; set => command_test_status = value; }
        public string Command_code { get => command_code; set => command_code = value; }
        public string Command_Pressure { get => command_Pressure; set => command_Pressure = value; }
        public string Command_Q { get => command_Q; set => command_Q = value; }
    }
}
