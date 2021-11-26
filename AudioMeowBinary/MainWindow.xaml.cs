using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace AudioMeowBinary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        string textboxText;
        string meowBinaryText;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertText(object sender, RoutedEventArgs e)
        {
            textboxText = EnglishText.Text;
            byte[] byteArray = Encoding.ASCII.GetBytes(textboxText);

            meowBinaryText = "";
            for (int i = 0; i < byteArray.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    meowBinaryText += (byteArray[i] & 0x80) > 0 ? "1" : "0";
                    byteArray[i] <<= 1;
                }
            }
            Console.WriteLine(meowBinaryText);
            char[] charArray = meowBinaryText.ToCharArray();
            string[] strArray = new string[charArray.Length];
            for (int i = 0; i < charArray.Length; i++)
            {
                strArray[i] = charArray[i] == '0' ? "meow_lower.wav" : "meow_upper.wav";
            }
            Concatenate("./out.wav", strArray);
        }

        private void Concatenate(string outputFile, IEnumerable<string> sourceFiles)
        {
            if (File.Exists(@"./out.wav"))
            {
                File.Delete(@"./out.wav");
            }

            byte[] buffer = new byte[1024];
            WaveFileWriter waveFileWriter = null;

            try
            {
                foreach (string sourceFile in sourceFiles)
                {
                    using WaveFileReader reader = new WaveFileReader(sourceFile);
                    if (waveFileWriter == null)
                    {
                        waveFileWriter = new WaveFileWriter(outputFile, reader.WaveFormat);
                    }

                    int read;
                    while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        waveFileWriter.Write(buffer, 0, read);
                    }
                }
            }
            finally
            {
                if (waveFileWriter != null)
                {
                    waveFileWriter.Dispose();
                }
            }
        }
    }
}
