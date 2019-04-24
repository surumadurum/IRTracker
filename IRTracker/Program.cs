using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using IRTracker.ObjectDetection;
namespace IRTracker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            BinaryDecoder binaryDecoder = new BinaryDecoder();
            binaryDecoder.Decode(new List<int>() { 64,32,64,64 });

            TestEqual test = new TestEqual();
            test.Run();
        }

 
    }
}
