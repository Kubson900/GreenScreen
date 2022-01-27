using System;
using System.Windows.Forms;

namespace GreenScreenUI
{
    /*
     *  Punkt wejsciowy programu
     *  Uruchamia GUI
     */
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GreenScreenForm());
        }
    }
}
