﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitLauncher
{
    public class Logger : TextWriter
    {
        private TextWriter originalOut;
        private FileStream ostrm;
        private StreamWriter writer;

        public Logger()
        {
            originalOut = Console.Out;
            try
            {
                var LogFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Celant Interactive\\Orbit Launcher\\log\\");
                Directory.CreateDirectory(LogFolder);
                ostrm = new FileStream(LogFolder + "latest.txt", FileMode.Create, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                originalOut.WriteLine("Cannot open log for writing");
                originalOut.WriteLine(e.Message);
                return;
            }
        }

        public override Encoding Encoding
        {
            get { return new System.Text.ASCIIEncoding(); }
        }
        public override void WriteLine(string message)
        {
            writer.WriteLine(String.Format("[{0}] {1}", DateTime.Now, message));
            writer.Flush();
        }
        public override void Write(string message)
        {
            writer.Write(String.Format("[{0}] {1}", DateTime.Now, message));
            writer.Flush();
        }


    }
}
