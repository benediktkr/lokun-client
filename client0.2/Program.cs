using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace lokunclient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var process_name = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            var instance_count = Process.GetProcessesByName(process_name).Length;
            if (instance_count == 1 || Debugger.IsAttached)
            {
                // Hey, it's just me around here!
                File.WriteAllBytes("c:\\users\\benedikt\\Desktop\\test.txt", new byte[] { (byte)'h', (byte)'a', (byte)'l', (byte)'l', (byte)'o' });
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new lokunclientform());
            }
            else
            {
                // do nothing
            }
        }
    }
}
