using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SANHUA_MAIN
{
    public abstract class JX_Device
    {
        public JX_Device()
        {
            IsConnected = false;
        }
        public bool IsConnected { get; set; }

       // public abstract void Connect(SoftConfig config, Action<bool> CallBack);
       // public abstract bool Connect(string conParam, SoftConfig config, ref string errMsg);
       // public abstract string SendCmd(string cmd);
    }
}
