using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SANHUA_MAIN
{
    class JX_APP
    {
        public void Connect(JX_Device device, Action<bool> CallBack)
        {
            if (device.IsConnected)
            {
                //创建回调参数
                bool obj = true;
                //异步回调
                CallBack?.BeginInvoke(obj, null, null);
                return;
            }
            else
            {
                //创建异步委托
                //Action<SoftConfig, Action<bool>> myAsy = new Action<SoftConfig, Action<bool>>(device.Connect);
                //异步执行委托
                //myAsy.BeginInvoke(curConfig, CallBack, null, null);
                return;
            }
        }


    }
}
