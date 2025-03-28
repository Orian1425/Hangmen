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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.OleDb;
using DAL;

namespace Hangmen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        //private List<string> wordList = new List<string>();
        private DataTable wordDataTable = new DataTable();
        private string selectedWord;
=======
        private int letters = 0;
        private int guessedWords = 0;
        private int maxFail = 10;
        private int fail = 10;
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
            this.fail = 10;
            this.DisplayFailImage();
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
                    Name = (this.selectedWord[i] != ' ' ) ? Name = selectedWord[i].ToString() : "S" ,
                    Margin = new Thickness(5),
                    FontSize = 30,
                    
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
                    if (tb.Name.Equals(letter.ToString()))
                    {
                        tb.Text = letter.ToString();
                    }
                }
                if(letters == wordLetters.Count())
                {
                    MessageBox.Show("you gussed the word! - "+this.selectedWord,"Good Job",MessageBoxButton.OK,MessageBoxImage.None);
                    guessedWords++;
                    this.guessedWordsCounter.Text = "Guessed Words: "+guessedWords;
                    foreach (Button b in this.lettersGrid.Children)
                    {
                        b.IsEnabled = false;
                    }
                    this.checkEnd();
                }
            }
            else
            {
                this.fail--;
                this.DisplayFailImage();
                if(this.fail == 0)
                {
                    MessageBox.Show("you failed!", "Lose",MessageBoxButton.OK,MessageBoxImage.None);
                    foreach (Button b in this.lettersGrid.Children)
                    {
                        b.IsEnabled = false;
                    }
                    this.Show_Solution(null,null);
                    this.checkEnd();
                }
            }
         
        }
        private void Show_Solution(object sender, RoutedEventArgs e)
        {
            foreach (TextBox tb in this.wordGrid.Children)
            {
                tb.Text = tb.Name != "S" ? tb.Name.ToString() : "";
            }
            MessageBox.Show("Showed solution", "game ended", MessageBoxButton.OK, MessageBoxImage.None);
            foreach (Button b in this.lettersGrid.Children)
            {
                b.IsEnabled = false;
            }
            this.checkEnd();

        }
        private void DisplayFailImage()
        {
            string picName = $"{maxFail-fail}.jpg";

            var uri = new Uri("C://Users//Alex//Downloads//Hangmen//Hangmen//images//" + picName);
            var bitmap = new BitmapImage(uri);

            manImage.Source = bitmap;
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
