using System;
using System.Windows.Forms;

namespace GreenScreenUI
{
    static class Program
    {
        //[DllImport(@"C:\\GreenScreen\\x64\\Debug\\GreenScreenAsm.dll")]
        //static extern int MyProc1(int a, int b);

        //static void Main(string[] args)
        //{
        //    int x = 5, y = 7;
        //    int retVal = MyProc1(x, y);

        //    Console.WriteLine("\nHello World");
        //    Console.WriteLine(retVal);
        //    Console.ReadLine();
        //}

        // <summary>
        // The main entry point for the application.
        // </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GreenScreenForm());
        }
    }
}
