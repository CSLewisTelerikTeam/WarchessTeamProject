using OOPGameWoWChess;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private void PlayGame()
        {
           backgroundMusic.Stop();
           var mainWindow = new MainWindow();
           mainWindow.Owner = this.Owner;
           mainWindow.Show();
           this.Close();
        }
        private void OpenInstructions()
        {
           backgroundMusic.Stop();
           var instructions = new Instructions();
           instructions.Owner = this.Owner;
           instructions.Show();
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
            this.Play.Child.MouseEnter += new MouseEventHandler(Image_MouseEnter);
            this.Play.Child.MouseLeave += new MouseEventHandler(Image_MouseLeave);
            this.Play.Child.MouseLeftButtonDown += new MouseButtonEventHandler(Image_MouseLeftButtonDown);
            
            Image instImg = new Image();
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\button_instr_unhover.png");
            instImg.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.Instructions.Child = instImg;
            this.Instructions.Child.MouseEnter += new MouseEventHandler(Image_MouseEnter);
            this.Instructions.Child.MouseLeave += new MouseEventHandler(Image_MouseLeave);
            this.Instructions.Child.MouseLeftButtonDown += new MouseButtonEventHandler(Image_MouseLeftButtonDown);

            Image exitImg = new Image();
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\button_exit_unhover.png");
            exitImg.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.Exit.Child = exitImg;
            this.Exit.Child.MouseEnter += new MouseEventHandler(Image_MouseEnter);
            this.Exit.Child.MouseLeave += new MouseEventHandler(Image_MouseLeave);
            this.Exit.Child.MouseLeftButtonDown += new MouseButtonEventHandler(Image_MouseLeftButtonDown);
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
            if (((sender as Image).Parent as Border).Name == "Play")
            {
                PlayGame();
            }
            else if (((sender as Image).Parent as Border).Name == "Instructions")
            {
                OpenInstructions();
            }
            else if (((sender as Image).Parent as Border).Name == "Exit")
            {
                Environment.Exit(0);
            }
        }
    }
}





