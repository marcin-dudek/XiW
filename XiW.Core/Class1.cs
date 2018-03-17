using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiW.Core
{
    public class XiConnection : IDisposable
    {
        public XiConnection(string filename)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo(filename)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            p.StartInfo = info;
            p.OutputDataReceived += (sender, args) => { Debug.Write(args.Data); };
            p.Start();
        }



        public void Dispose()
        {
        }
    }
}
