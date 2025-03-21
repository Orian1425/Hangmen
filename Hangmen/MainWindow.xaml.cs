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
        private HashSet<char> wordLetters = new HashSet<char>();
        private int letters = 0;
        public MainWindow()
        {
            InitializeComponent();

            MakeLetters();
            New_Word(null,null);
            
        }
        private void MakeLetters()
        {
            this.lettersGrid.Children.Clear();
            for (char ch = 'a'; ch <= 'z'; ch++)
            {
                Button btn = new Button
                {

                    Content = ch.ToString(),
                    FontSize = 30,
                    Margin = new Thickness(2)

                };
                btn.Click += Btn_Click;
                this.lettersGrid.Children.Add(btn);
            }
        }

        private void New_Word(object sender,RoutedEventArgs e)
        {
            this.checkEnd();
            this.wordGrid.Children.Clear();
            this.MakeLetters();
            this.wordLetters.Clear();
            letters = 0;
            int rndNum = this.rnd.Next(0,this.wordList.Count());
            this.selectedWord = this.wordList[rndNum];
            this.wordList.Remove(selectedWord);

            for(int i = 0; i<this.selectedWord.Length; i++)
            {
                TextBox tb = new TextBox
                {
                    IsReadOnly = true,
                    Text = selectedWord[i].ToString(),
                    Foreground = Brushes.White,
                    Margin = new Thickness(5),
                    FontSize = 30
                };
                if (selectedWord[i] == ' ')
                {
                    this.letters++;
                    tb.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
                this.wordLetters.Add(selectedWord[i]);
                this.wordGrid.Children.Add(tb);
            }
            
            //
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;

            char letter = char.Parse(btn.Content.ToString());

            if(selectedWord.Contains(letter))
            {
                letters++;
                foreach(TextBox tb in this.wordGrid.Children)
                {
                    if (tb.Text.Equals(letter.ToString()))
                    {
                        tb.Foreground = Brushes.Black;
                    }
                }
                if(letters == wordLetters.Count())
                {
                    MessageBox.Show("you gussed the word! - "+this.selectedWord,"Good Job",MessageBoxButton.OK,MessageBoxImage.None);
                    this.checkEnd();
                }
            }
         
        }
        private void checkEnd()
        {
            if (this.wordList.Count() == 0)
            {
                MessageBox.Show("no more words", "game ended", MessageBoxButton.OK, MessageBoxImage.None);
                Close();
            }
        }
    }
}
