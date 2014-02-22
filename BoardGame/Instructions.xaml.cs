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
using System.Windows.Shapes;

namespace BoardGame
{
    /// <summary>
    /// Interaction logic for Instructions.xaml
    /// </summary>
    public partial class Instructions : Window
    {
        public Instructions()
        {
            InitializeComponent();
            LoadBackgroundImage();
            LoadBackgroundMusic();
            InitializeButtons();
        }

        private MediaPlayer backgroundMusic = new MediaPlayer();

        private void LoadBackgroundImage()
        {
            ImageBrush myBrush = new ImageBrush();
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\Instructions.png");
            myBrush.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.Background = myBrush;
        }
        private void LoadBackgroundMusic()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Background_music\start_menu_music.mp3");
            backgroundMusic.Open(new Uri(path));
            backgroundMusic.Play();
        }
        private void InitializeButtons()
        {
            Image newGameImg = new Image();
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\backtomenu_unhover.png");
            newGameImg.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.Back.Child = newGameImg;
            this.Back.Child.MouseEnter += new MouseEventHandler(Image_MouseEnter);
            this.Back.Child.MouseLeave += new MouseEventHandler(Image_MouseLeave);
            this.Back.Child.MouseLeftButtonDown += new MouseButtonEventHandler(Image_MouseLeftButtonDown);
        }
        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            string replacedPath = (sender as Image).Source.ToString().Replace("unhover", "hover");
            (sender as Image).Source = new BitmapImage(new Uri(replacedPath, UriKind.Absolute));
            //Try without this hack
        }
        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            string replacedPath = (sender as Image).Source.ToString().Replace("hover", "unhover");
            (sender as Image).Source = new BitmapImage(new Uri(replacedPath, UriKind.Absolute));
            //Try without this hack
        }
        private void Image_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            BackToMenu();
        }
        private void BackToMenu()
        {
            backgroundMusic.Stop();
            var startScreen = new StartScreen();
            startScreen.Owner = this.Owner;
            startScreen.Show();
            this.Close();
        }
    }
}