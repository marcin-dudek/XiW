using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Jil;

namespace XiW.Core
{
    public class XiConnection : IDisposable
    {
        private readonly Stream _input;
        private long id = 0;
        public EventHandler<DataReceivedEventArgs> OnError;

        public XiConnection(string filename)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo(filename)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8
            };
            p.StartInfo = info;
            p.EnableRaisingEvents = true;
            p.OutputDataReceived += ReceiveData;
            p.ErrorDataReceived += ReceiveError;
            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();

            _input = p.StandardInput.BaseStream;
        }

        private void ReceiveError(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
            OnError?.Invoke(this, e);
        }

        private void ReceiveData(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
            if (e.Data != null)
            {
                var x = JSON.Deserialize<object>(e.Data);
            }
        }


        public void Send(string method, object @params)
        {
            var req = new { method, @params, id = Interlocked.Increment(ref id) };

            //using (TextWriter writer = new StreamWriter(_input, Encoding.UTF8, 41, true))
            //{
            //    JSON.Serialize(req, writer);
            //    writer.Write('\n');
            //    writer.Flush();
            //}

            StringBuilder sb = new StringBuilder(100);
            using (TextWriter writer = new StringWriter(sb))
            {
                JSON.Serialize(req, writer);
            }

            sb.Append('\n'); // important!

            byte[] y = Encoding.UTF8.GetBytes(sb.ToString());
            _input.Write(y, 0, y.Length);
            _input.Flush();
        }

        public void Dispose()
        {
        }
    }
}