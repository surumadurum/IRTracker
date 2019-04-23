using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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

            UnidentifiedObject obj = new UnidentifiedObject();
            obj.OnObjectIdentified += ObjectIdentifiedCallback;


            while(true)
            {
                bool value =Convert.ToBoolean(Char.GetNumericValue(Console.ReadKey().KeyChar));
                obj.SignatureNewValue(value);
            }

        }

        static void ObjectIdentifiedCallback(UnidentifiedObject obj, int ID)
        {
            Debug.WriteLine("[Program] Object identified:{0}", ID);
            obj = null;
            obj = new UnidentifiedObject();
            obj.OnObjectIdentified += ObjectIdentifiedCallback;
        }
    }
}
