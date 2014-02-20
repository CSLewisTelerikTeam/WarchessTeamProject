using OOPGameWoWChess;
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
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        public StartScreen()
        {
            InitializeComponent();

            LoadBackgroundImage();
            LoadBackgroundMusic();

            InitializeButtons();

        }

        private MediaPlayer backgroundMusic = new MediaPlayer();

        private void PlayGame(object sender, RoutedEventArgs e)
        {
           var mainWindow = new MainWindow();
           mainWindow.Owner = this.Owner;
           mainWindow.Show();
           this.Close();
        }

        private void LoadBackgroundImage()
        {
            ImageBrush myBrush = new ImageBrush();
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\start_screen.png");
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
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\button_newgame_unhover.png");
            newGameImg.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.Play.Child = newGameImg;
            

            Image instImg = new Image();
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\button_instr_unhover.png");
            instImg.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.Instructions.Child = instImg;

            Image exitImg = new Image();
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\button_exit_unhover.png");
            exitImg.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.Exit.Child = exitImg;

            //Image newGameImgHover = new Image();
            //path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\button_exit_hover.png");
            //newGameImgHover.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            //this.Unit00.Child = newGameImgHover;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            //Image img = new Image();

            //string smallImgSource = (sender as Image).Source.ToString();

            (((sender as Border).Child) as Image).Source.ToString().Replace("unhover", "hover");

            //string bigImgSource = smallImgSource.Replace("small", "big");

            //img.Source = new BitmapImage(new Uri(bigImgSource, UriKind.Absolute));

            //this.BigCardImage.Width = 330;
            //this.BigCardImage.Source = img.Source;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Image)(sender as Border).Child).Source.ToString().Replace("hover", "unhover");
            //this.BigCardImage.Source = null;
        }

    }
}





