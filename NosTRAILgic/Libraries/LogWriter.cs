using System;
using System.IO;

namespace NosTRAILgic.Libraries
{
    /************************************************************************************
     * Description: XXXXXX                                                              *
     *                                                                                  *
     * Developer: Elson                                                                 *
     *                                                                                  *
     * Date: 02/04/2016                                                                 *
     ************************************************************************************/
    public sealed class LogWriter
    {
        private static volatile LogWriter instance;
        private static object syncRoot = new Object();

        private LogWriter() { }

        public static LogWriter Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new LogWriter();
                    }
                }

                return instance;
            }
        }

        public void LogInfo(string message)
        {
            string sPath = AppDomain.CurrentDomain.BaseDirectory + @"\Logs";
            System.IO.Directory.CreateDirectory(sPath);
            DateTime currentdt = DateTime.Now;
            string date = currentdt.ToString("yyyy-MM-dd");
            TextWriter tw = new StreamWriter(sPath + @"\" + date + ".txt", true);
            tw.WriteLine("[" + currentdt.ToString() + "] " + message);
            tw.Close();
        }
    }
}