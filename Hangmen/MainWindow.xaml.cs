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

namespace Hangmen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        private List<string> wordList = new List<string>()
        {
            "apple", "banana","grape fruit","watermelon","strawberry","mango","kiwi"
        };
        private string selectedWord;
        private char letter;
        private int letters = 0;
        public MainWindow()
        {
            InitializeComponent();

            for(char ch = 'a'; ch <= 'z'; ch++)
            {
                Button btn = new Button
                {
                    
                    Content = ch.ToString(), 
                    FontSize = 30,
                    Margin = new Thickness(2)
                    
                };
                btn.Click += Btn_Click;
                lettersGrid.Children.Add(btn);
            }
        }

        private void New_Word(object sender,RoutedEventArgs e)
        {
            wordGrid.Children.Clear();
         
            int rndNum = rnd.Next(0,wordList.Count());
            selectedWord = wordList[rndNum];
            wordList.Remove(selectedWord);

            for(int i = 0; i<selectedWord.Length; i++)
            {
                TextBox tb = new TextBox
                {
                    IsReadOnly = true,
                    Text = selectedWord[i].ToString(),
                    Foreground = Brushes.White,
                    Margin = new Thickness(5),
                    FontSize = 30,
                };
                if (selectedWord[i] == ' ')
                {
                    letter++;
                    tb.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
                wordGrid.Children.Add(tb);
            }
            if(wordList.Count() == 0)
            {
                MessageBox.Show("no more words", "game ended", MessageBoxButton.OK,MessageBoxImage.None);
                Close();
            }
            //
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;

            letter = char.Parse(btn.Content.ToString());
        }
    }
}
