using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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


            for (int i = 0; i < byteArray.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    meowBinaryText += (byteArray[i] & 0x80) > 0 ? "1" : "0";
                    byteArray[i] <<= 1;
                }
            }
            Console.WriteLine(meowBinaryText);


            EnglishText.Text = meowBinaryText;
        }
    }
}
