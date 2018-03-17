using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace XiW.Core
{
    public class XiConnection : IDisposable
    {
        private Stream _input;

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
            p.OutputDataReceived += ReceiveData;
            p.ErrorDataReceived += (sender, args) => { Debug.Write(args.Data); };
            _input = p.StandardInput.BaseStream;
            p.Start();

        }

        private void ReceiveData(object sender, DataReceivedEventArgs e)
        {
            Jil.JSON.Deserialize<object>(e.Data);
        }


        public Task Send(string method, object parameters)
        {
            byte[] array = new Byte[65000];

            var req = new Dictionary<string, dynamic> { { "method", method }, { "params", parameters } };
            //if (callback != null)
            //{
            //    req.Add("id", rpcIndex);
            //    var index = rpcIndex;
            //    rpcIndex++;
            //    pending.Add(index, callback);
            //}

            TextWriter writer = new StreamWriter(new MemoryStream(array));
            Jil.JSON.Serialize(req, writer);

            return _input.WriteAsync(array, 0, array.Length);
        }

        public void Dispose()
        {
        }
    }
}