using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BoardGame.UnitClasses;
using BoardGame;

namespace OOPGameWoWChess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadBackgroundImage();
            LoadBackgroundMusic();
            InitializeUnits();
        }

        private bool isMouseCapture;
        private double mouseXOffset;
        private double mouseYOffset;
        private TranslateTransform translateTransform;
        private MediaPlayer backgroundMusic = new MediaPlayer();

        public static Unit SelectedUnit;
        private bool isSomeUnitSelected;
        private Border selectedBorder;
        
        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image img = new Image();

            string smallImgSource = (sender as Image).Source.ToString();

            string bigImgSource = smallImgSource.Replace("small", "big");

            img.Source = new BitmapImage(new Uri(bigImgSource, UriKind.Absolute));

            this.BigCardImage.Width = 250;
            this.BigCardImage.Source = img.Source;
        }
        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            this.BigCardImage.Source = null;
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            Border border = (Border)image.Parent;
            Grid grid = (Grid)border.Parent;

            if (isSomeUnitSelected && (selectedBorder != border))
            {
                selectedBorder.BorderThickness = new Thickness(0, 0, 0, 0);
                selectedBorder.BorderBrush = Brushes.Transparent;
                var path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\empty_cell.png");
                selectedBorder.Background = new ImageBrush(new BitmapImage(new Uri(path, UriKind.Absolute)));
            }

            selectedBorder = border;

            image.CaptureMouse();
            isMouseCapture = true;
            mouseXOffset = e.GetPosition(border).X;
            mouseYOffset = e.GetPosition(border).Y;
                        
            SelectedUnit = GetUnitOnPosition(e.GetPosition(grid), grid);

            if (SelectedUnit != null)
            {
                SelectedUnit.IsSelected = true;
                isSomeUnitSelected = true;
            }

            //Mark the selected item
            if (SelectedUnit.GetType().BaseType.Name == "RaceAlliance")
            {
                border.BorderThickness = new Thickness(2, 2, 2, 2);
                border.BorderBrush = Brushes.RoyalBlue;
                border.Background = Brushes.LightSkyBlue;
            }
            else
            {
                border.BorderThickness = new Thickness(2, 2, 2, 2);
                border.BorderBrush = Brushes.Crimson;
                border.Background = Brushes.LightCoral;
            }
        }
        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isSomeUnitSelected)
            {
                return;
            }

            Image image = (Image)sender;
            Border border = (Border)image.Parent;
            Grid grid = (Grid)border.Parent;

            if (isMouseCapture)
            {
                translateTransform = new TranslateTransform();

                translateTransform.X = e.GetPosition(border).X - mouseXOffset;
                translateTransform.Y = e.GetPosition(border).Y - mouseYOffset;

                image.RenderTransform = translateTransform;
            }
        }
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (translateTransform == null)
            {
                return;
            }
            
            Image image = (Image)sender;
            Border border = (Border)image.Parent;
            Grid grid = (Grid)border.Parent;

            image.ReleaseMouseCapture();
            isMouseCapture = false;

            Point coordinates;

            //Get the new position it row and col
            grid.GetDynamicRowColumn(e.GetPosition(border), SelectedUnit.CurrentPosition, out coordinates);

            //The selected unit current position + calculated coordinates
            coordinates.X = (int)SelectedUnit.CurrentPosition.X + coordinates.X;
            coordinates.Y = (int)SelectedUnit.CurrentPosition.Y + coordinates.Y;

            if (SelectedUnit.IsMoveable(new Point(coordinates.X, coordinates.Y)))
            {
                Grid.SetRow(border, (int)coordinates.Y);
                Grid.SetColumn(border, (int)coordinates.X);

                //Change the selected unit current position if the unit can move on that position
                SelectedUnit.CurrentPosition = new Point(coordinates.X, coordinates.Y);

                //Unmark the selected item
                selectedBorder.BorderThickness = new Thickness(0, 0, 0, 0);
                selectedBorder.BorderBrush = Brushes.Transparent;
                var path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\empty_cell.png");
                selectedBorder.Background = new ImageBrush(new BitmapImage(new Uri(path, UriKind.Absolute)));

                //Deselect the selected unit
                SelectedUnit.IsSelected = false;
                isSomeUnitSelected = false;
            }

            translateTransform = new TranslateTransform();

            translateTransform.X = 0;
            translateTransform.Y = 0;

            mouseXOffset = 0;
            mouseYOffset = 0;

            image.RenderTransform = translateTransform;
        }
        private Unit GetUnitOnPosition(Point position, Grid grid)
        {
            Point coordinates;

            grid.GetRowColumn(position, out coordinates);

            foreach (var alliance in InitializedTeams.AllianceTeam)
            {
                if (alliance.CurrentPosition == coordinates)
                {
                    return alliance;
                }
            }

            foreach (var horde in InitializedTeams.HordeTeam)
            {
                if (horde.CurrentPosition == coordinates)
                {
                    return horde;
                }
            }

            return null;
        }
        private void LoadBackgroundImage()
        {
            ImageBrush myBrush = new ImageBrush();
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\background_image.png");
            myBrush.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.Background = myBrush;

            //path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\empty_cell.png");

            //foreach (var child in this.Playfield.Children)
            //{
            //    (child as Border).Background = new ImageBrush(new BitmapImage(new Uri(path, UriKind.Absolute)));
            //}
        }
        private void InitializeUnits()
        {
            //Alliance initialization
            for (int i = 0; i < 16; i++)
            {
                var allianceUnit = InitializedTeams.AllianceTeam[i];

                allianceUnit.SmallImage.MouseLeftButtonDown += new MouseButtonEventHandler(Image_MouseLeftButtonDown);
                allianceUnit.SmallImage.MouseMove += new MouseEventHandler(Image_MouseMove);
                allianceUnit.SmallImage.MouseLeftButtonUp += new MouseButtonEventHandler(Image_MouseLeftButtonUp);
                allianceUnit.SmallImage.MouseEnter += new MouseEventHandler(Image_MouseEnter);
                allianceUnit.SmallImage.MouseLeave += new MouseEventHandler(Image_MouseLeave);

                if (i >= 0 && i < 8)
                {
                    var squire = (Border)this.Playfield.FindName("Unit6" + i);
                    squire.Child = allianceUnit.SmallImage;
                }
            }

            var unitField = (Border)this.Playfield.FindName("Unit71");
            unitField.Child = (InitializedTeams.AllianceTeam[8]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit76");
            unitField.Child = (InitializedTeams.AllianceTeam[9]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit72");
            unitField.Child = (InitializedTeams.AllianceTeam[10]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit75");
            unitField.Child = (InitializedTeams.AllianceTeam[11]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit70");
            unitField.Child = (InitializedTeams.AllianceTeam[12]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit77");
            unitField.Child = (InitializedTeams.AllianceTeam[13]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit73");
            unitField.Child = (InitializedTeams.AllianceTeam[14]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit74");
            unitField.Child = (InitializedTeams.AllianceTeam[15]).SmallImage;

            //Horde initialization

            for (int i = 0; i < 16; i++)
            {
                var hordeUnit = InitializedTeams.HordeTeam[i];

                hordeUnit.SmallImage.MouseLeftButtonDown += new MouseButtonEventHandler(Image_MouseLeftButtonDown);
                hordeUnit.SmallImage.MouseMove += new MouseEventHandler(Image_MouseMove);
                hordeUnit.SmallImage.MouseLeftButtonUp += new MouseButtonEventHandler(Image_MouseLeftButtonUp);
                hordeUnit.SmallImage.MouseEnter += new MouseEventHandler(Image_MouseEnter);
                hordeUnit.SmallImage.MouseLeave += new MouseEventHandler(Image_MouseLeave);

                if (i >= 0 && i < 8)
                {
                    var grunt = (Border)this.Playfield.FindName("Unit1" + i);
                    grunt.Child = hordeUnit.SmallImage;
                }
            }

            unitField = (Border)this.Playfield.FindName("Unit01");
            unitField.Child = (InitializedTeams.HordeTeam[8]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit06");
            unitField.Child = (InitializedTeams.HordeTeam[9]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit02");
            unitField.Child = (InitializedTeams.HordeTeam[10]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit05");
            unitField.Child = (InitializedTeams.HordeTeam[11]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit00");
            unitField.Child = (InitializedTeams.HordeTeam[12]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit07");
            unitField.Child = (InitializedTeams.HordeTeam[13]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit03");
            unitField.Child = (InitializedTeams.HordeTeam[14]).SmallImage;
            unitField = (Border)this.Playfield.FindName("Unit04");
            unitField.Child = (InitializedTeams.HordeTeam[15]).SmallImage;
        }
        private void LoadBackgroundMusic()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Background_music\background_music.mp3");
            backgroundMusic.Open(new Uri(path));
            backgroundMusic.MediaEnded += new EventHandler(Media_Ended);
            backgroundMusic.Play();
        }
        private void Media_Ended(object sender, EventArgs e)
        {
            backgroundMusic.Position = TimeSpan.Zero;
            backgroundMusic.Play();
        }


        //Yanko's work ====================================================================================================
        
        //private static bool HordeTurn = true;
        private void Attack(Unit aggressor, Unit target)
        {
            //string race;
            //int unitIndex;
            //Position targetPosition = new Position(target.RowPosition, target.ColPosition);

            //if (aggressor.UnitRace != target.UnitRace)
            //{
            //    if (aggressor.UnitRace == UnitRaceType.alliance)
            //    {
            //        //Check if the aggresssor could reach the target
            //        if (IsMoveable((aggressor as RaceAlliance).Type.ToString(), targetPosition, out unitIndex, out race))
            //        {
            //            target.HealthLevel -= aggressor.AttackLevel;
            //            aggressor.HealthLevel -= target.CounterAttackLevel;
            //        }
            //    }
            //    else if (aggressor.UnitRace == UnitRaceType.horde)
            //    {
            //        //Check if the aggresssor could reach the target
            //        if (IsMoveable((aggressor as RaceHorde).Type.ToString(), targetPosition, out unitIndex, out race))
            //        {
            //            target.HealthLevel -= aggressor.AttackLevel;
            //            aggressor.HealthLevel -= target.CounterAttackLevel;
            //        }
            //    }
            //}

        }
       
        ////Unit attack event
        //private void Unit_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    Unit aggressor = CheckForSelectedEnemy();

        //    for (int i = 0; i < UnitsPerTeam; i++)
        //    {
        //        if (InitializedTeams.AllianceTeam[i].Type.ToString() == (sender as Image).Name)
        //        {
        //            Attack(aggressor, InitializedTeams.AllianceTeam[i]);
        //            break;
        //        }
        //        if (InitializedTeams.HordeTeam[i].Type.ToString() == (sender as Image).Name)
        //        {
        //            Attack(aggressor, InitializedTeams.HordeTeam[i]);
        //            break;
        //        }

        //    }
        
    }
}