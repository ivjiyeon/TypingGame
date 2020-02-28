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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace topic3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TypingGame currentGame = null;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentGame == null)
            {
                currentGame = new TypingGame((MainWindow)Application.Current.MainWindow);
            }
            currentGame.Reset();
        }

        private void textBoxText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentGame.TypedWord(textBoxText))
            {
                currentGame.Remove(textBoxText.Text);
                textBoxScore.Text = currentGame.Score.ToString();
                textBoxText.Clear();
            }
        }
    }

    public class TypingGame {
        private static string[] words = {"throat", "miss", "disagree", "bench", "miracle",
                                    "scenario", "freeze", "flush", "zone", "jewel",
                                    "shock", "fresh", "horn", "parachute", "manual",
                                    "stubborn", "slippery", "effective", "palace", "center",
                                    "secure", "contraction", "ball", "utter", "miserable",
                                    "pull", "deal", "leg", "comfort", "lot",
                                    "poem", "linear", "demonstrator", "battlefield", "wire",
                                    "doctor", "minority", "infect", "coal", "muscle",
                                    "mushroom", "demonstration", "serve", "election", "contact",
                                    "week", "rotate", "thumb", "shoulder", "spell"};
        private int score;       //earned by removing word block
        public int Score{
            get{return score;} }
        private int health;      //decreased when block reaches the bottom
        private int counter;     //indicates time
        private List<TextBlock> textblocks;
        private TextBlock block;
        private DispatcherTimer timer;

        MainWindow window;
        static Random rand = new Random();
        Color[] colors = new Color[] { Colors.Crimson, Colors.DarkOliveGreen, Colors.Firebrick, Colors.DarkSlateBlue, Colors.Purple, Colors.Teal };

        //Constructor of TypingGame
        public TypingGame(MainWindow window)
        {
            this.window = window;
            textblocks = new List<TextBlock>();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
        }

        //every time tick
        private void timer_Tick(object sender, EventArgs e)
        {
            counter--;
            window.textBoxTime.Text = counter + "s";

            GenerateBlock();
            Falling();
            FinishGame();
        }

        //Generate randomwords
        private static string GetRandomWord()
        {
            int randomIndex = rand.Next(0, words.Length);
            string randomWord = words[randomIndex];

            return randomWord;
        }

        //Generate texblock with random word inside
        private void GenerateBlock()
        {
            block = new TextBlock();
            block.Text = GetRandomWord();
            block.FontFamily = new FontFamily("Arial Black");
            block.TextWrapping = TextWrapping.Wrap;
            block.Width = 100;
            block.Height = 30;
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = colors[rand.Next(0, colors.Length)];
            block.Foreground = mySolidColorBrush;
            textblocks.Add(block);
            window.canvas2D.Children.Add(block);
            Canvas.SetLeft(block, rand.Next(320));
            Canvas.SetTop(block, rand.Next(40));
        }

        //make textblock to fall
        private void Falling()
        {
            foreach (TextBlock block in textblocks)
            {
                // Check for existing TranslateTransform
                var translation = block.RenderTransform as TranslateTransform;
                if (translation == null)
                {
                    block.RenderTransform = new TranslateTransform();
                }
                translation = block.RenderTransform as TranslateTransform;
                var anim = new DoubleAnimation();
                anim.By = 50;
                anim.Duration = TimeSpan.FromSeconds(10);
                anim.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut };                
                translation.BeginAnimation(TranslateTransform.YProperty, anim, HandoffBehavior.Compose);

                Point p = block.TranslatePoint(new Point(0, 0), window.canvas2D);                
                //if reaches bottom, remove;                
                if (p.Y > 227.5)
                {                    
                    Remove(block.Text);
                    health -= 10;
                    window.textBoxHealth.Text = health.ToString();
                    break;
                }                    
            }
        }

        //if correct, add score and clear the textbox
        public bool TypedWord(TextBox textbox)
        {
            if (Array.Exists(words, element => element == textbox.Text))
            {
                if (textbox.Text.Length > 5)
                    score += 20;
                else
                    score += 10;
                
                return true;
            }
            return false;
        }

        //remove the textblock from the canvas
        public void Remove(string content)
        {
            foreach (TextBlock block in textblocks)
            {
                if(block.Text == content)
                {
                    block.Visibility = Visibility.Hidden;
                    textblocks.Remove(block);
                    break;
                }                                    
            }            
        }

        //pop out message when game is finished
        public void FinishGame()
        {
            if(counter == 0 || health == 0)
            {                
                timer.Stop();
                MessageBox.Show(String.Format("Game Over. Your score is: {0}. Your health is: {1}", score, health));
            }
        }

        //reset the game
        public void Reset()
        {
            score = 0;
            health = 100;
            counter = 20;
            textblocks = new List<TextBlock>();
            timer.Interval = TimeSpan.FromSeconds(1);
            window.textBoxScore.Clear();
            window.textBoxHealth.Clear();
            window.textBoxText.Clear();
            window.canvas2D.Children.Clear();

            window.textBoxHealth.Text = health.ToString();
            window.textBoxScore.Text = score.ToString();
            timer.Start();
        }
    }
}
