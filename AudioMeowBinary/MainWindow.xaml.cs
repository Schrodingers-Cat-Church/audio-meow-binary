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
            char[] charArray = meowBinaryText.ToCharArray();
            Concatenate("./out.wav", charArray);
        }

        private void Concatenate(string outputFile, IEnumerable<char> sourceFiles)
        {
            if (File.Exists(@"./out.wav"))
            {
                File.Delete(@"./out.wav");
            }

            byte[] buffer = new byte[1024];
            WaveFileWriter waveFileWriter = null;

            try
            {
                foreach (char sourceFile in sourceFiles)
                {
                    UnmanagedMemoryStream audRes = sourceFile == '0' ? Properties.Resources.meow_lower : Properties.Resources.meow_upper;
                    using WaveFileReader reader = new WaveFileReader(audRes);
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

        private void PlayAudio(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("./out.wav");
            player.Play();
        }

        private void ViewAudioFile(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", Path.GetFullPath("./out.wav")));
        }
    }
}
