using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace SANHUA_MAIN
{
    class InstrumentParam
    {


       public  InstrumentParam(string portName, int baudRate, Parity parity,int dataBits,StopBits stopBits) 
        {
            this.ProtName = portName;
            this.BaudRate = baudRate;
            this.Parity = parity;
            this.DataBits = dataBits;
            this.StopBits = stopBits;
        }
        public InstrumentParam() { }

        public string ProtName { get; set; }

        public int BaudRate { get; set; }

        public Parity Parity { get; set; }

        public int DataBits { get; set; }

        public StopBits StopBits { get; set; }

    }
}
