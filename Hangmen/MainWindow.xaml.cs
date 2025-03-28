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
        private int letters = 0;
        private int guessedWords = 0;
        private int maxFail = 10;
        private int fail = 10;
        public MainWindow()
        {
            InitializeComponent();

            MakeLetters();
            this.subjectCBox.ItemsSource = DAL.DAL.GetDataView("Select * from SubjectTbl");
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
            if(this.subjectCBox.SelectedIndex > -1)
            {
                if (this.wordDataTable.Rows.Count !=0)
                {
                    this.fail = 10;
                    this.DisplayFailImage();
                    this.checkEnd();
                    this.wordGrid.Children.Clear();
                    this.MakeLetters();
                    letters = 0;
                    int rndNum = this.rnd.Next(this.wordDataTable.Rows.Count);
                    this.selectedWord = this.wordDataTable.Rows[rndNum]["Word"].ToString();
                    this.wordDataTable.Rows.RemoveAt(rndNum);

                    for (int i = 0; i < this.selectedWord.Length; i++)
                    {
                        TextBox tb = new TextBox
                        {
                            IsReadOnly = true,
                            Name = (this.selectedWord[i] != ' ') ? Name = selectedWord[i].ToString() : "S",
                            Margin = new Thickness(5),
                            FontSize = 30,

                        };

                        if (selectedWord[i] == ' ')
                        {
                            this.letters++;
                            tb.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        }
                        this.wordGrid.Children.Add(tb);
                    }
                }
                else
                {
                    MessageBox.Show("no more words", "error");
                }
            }
            else
            {
                MessageBox.Show("no subject selected", "error");
            }
            
            
            //
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if(this.wordGrid.Children.Count == 0)
            {
                MessageBox.Show("no word", "error");
            }
            else
            {
                Button btn = (Button)sender;
                btn.IsEnabled = false;

                char letter = char.Parse(btn.Content.ToString());

                if (selectedWord.Contains(letter))
                {

                    foreach (TextBox tb in this.wordGrid.Children)
                    {
                        if (tb.Name.Equals(letter.ToString()))
                        {
                            letters++;
                            tb.Text = letter.ToString();
                        }
                    }
                    if (letters == selectedWord.Length)
                    {
                        MessageBox.Show("you gussed the word! - " + this.selectedWord, "Good Job", MessageBoxButton.OK, MessageBoxImage.None);
                        guessedWords++;
                        this.guessedWordsCounter.Text = "Guessed Words: " + guessedWords;
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
                    if (this.fail == 0)
                    {
                        MessageBox.Show("you failed!", "Lose", MessageBoxButton.OK, MessageBoxImage.None);
                        foreach (Button b in this.lettersGrid.Children)
                        {
                            b.IsEnabled = false;
                        }
                        this.Show_Solution(null, null);
                    }
                }
            }
            
         
        }
        private void Show_Solution(object sender, RoutedEventArgs e)
        {
            if(this.wordGrid.Children.Count == 0)
            {
                MessageBox.Show("no word", "error");
            }
            else
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
            if (this.wordDataTable.Rows.Count == 0)
            {
                MessageBox.Show("no more words", "game ended", MessageBoxButton.OK, MessageBoxImage.None);
                this.newWordBtn.IsEnabled = false;
            }
        
        }

        private void subjectCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.newWordBtn.IsEnabled = true;
            int subjId = (int)subjectCBox.SelectedValue;
            string sqlStr = $"SELECT * FROM WordTbl WHERE Subject = {subjId}";
            wordDataTable = DAL.DAL.GetDataTable(sqlStr);
        }
    }
}
