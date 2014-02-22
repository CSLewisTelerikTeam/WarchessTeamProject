using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BoardGame.SecretFieldClasses;
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
            InitializeSecret();
            SecretField.GenerateSecret(this, secret);
            SetTurn();
        }
          
        private bool isMouseCapture;
        private double mouseXOffset;
        private double mouseYOffset;
        private TranslateTransform translateTransform;
        private MediaPlayer backgroundMusic = new MediaPlayer();

        public static Unit SelectedUnit;
        public static SecretField secret;
        private bool isSomeUnitSelected;
        private Border selectedBorder;
        private static bool HordeTurn = true;
        
        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            //Get the hovered unit
            Image image = (Image)sender;
            Border border = (Border)image.Parent;
            Grid grid = (Grid)border.Parent;

            Point coordinates;

            grid.GetRowColumn(e.GetPosition(grid), out coordinates);

            if (secret.CurrentPosition == coordinates)
            {
                SetRightSidebarImage(sender);
            }

            //Get the hovered image and change the right sidebar image            
            Unit hoveredUnit = GetUnitOnMousePosition(e.GetPosition(grid), grid);

            if (hoveredUnit != null)
            {
                SetRightSidebarImage(sender);
                SetRightSidebarStats(hoveredUnit);
            }
            else
            {
            }
            
            //Mark enemy's cell border in red 
            HighLightPossibleEnemy(sender, hoveredUnit);
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            if (SelectedUnit != null)
            {
                this.BigCardImage.Source = SelectedUnit.BigImage.Source;

                this.Health.Text = "Health: " + SelectedUnit.HealthLevel.ToString();
                this.Damage.Text = "Attack: " + SelectedUnit.AttackLevel.ToString();
                this.Defence.Text = "Defence: " + SelectedUnit.CounterAttackLevel.ToString();
                this.Level.Text = "Level: " + SelectedUnit.Level.ToString();
            }
            else
            {
                this.BigCardImage.Source = null;

                this.Health.Text = "";
                this.Damage.Text = "";
                this.Defence.Text = "";
                this.Level.Text = "";
            }
            
            DownLightPossibleMoves();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            Border border = (Border)image.Parent;
            Grid grid = (Grid)border.Parent;

            SelectedUnit = GetUnitOnMousePosition(e.GetPosition(grid), grid);

            RaceTypes RaceTurn = HordeTurn ? RaceTypes.Horde : RaceTypes.Alliance;

            if (SelectedUnit.Race != RaceTurn)
            {
                SelectedUnit = null;
                return;
            }

            if (isSomeUnitSelected && (selectedBorder != border))
            {
                DeselectUnit();
            }

            selectedBorder = border;

            image.CaptureMouse();
            isMouseCapture = true;
            mouseXOffset = e.GetPosition(border).X;
            mouseYOffset = e.GetPosition(border).Y;

            if (SelectedUnit != null)
            {
                SelectedUnit.IsSelected = true;
                isSomeUnitSelected = true;

                //Get the hovered image and change the right sidebar image            
                Unit hoveredUnit = GetUnitOnMousePosition(e.GetPosition(grid), grid);
                SetRightSidebarImage(sender);
                SetRightSidebarStats(hoveredUnit);

                SelectedUnit.PlaySelectSound();
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

            HighLightPossibleMoves();
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

            if (SelectedUnit != null)
            {
                //Get the new position it row and col
                grid.GetDynamicRowColumn(e.GetPosition(border), SelectedUnit.CurrentPosition, out coordinates);

                //The selected unit current position + calculated coordinates
                coordinates.X = (int)SelectedUnit.CurrentPosition.X + coordinates.X;
                coordinates.Y = (int)SelectedUnit.CurrentPosition.Y + coordinates.Y;

                if (SelectedUnit.IsClearWay(new Point(coordinates.X, coordinates.Y)) &&
                    SelectedUnit.IsCorrectMove(new Point(coordinates.X, coordinates.Y)) &&
                    SelectedUnit.IsSomeoneAtThisPosition(new Point(coordinates.X, coordinates.Y)))
                {
                    Grid.SetRow(border, (int)coordinates.Y);
                    Grid.SetColumn(border, (int)coordinates.X);

                    //Change the selected unit current position if the unit can move on that position
                    SelectedUnit.CurrentPosition = new Point(coordinates.X, coordinates.Y);

                    //Open the secret field if the coordinates matches
                    if (secret.CurrentPosition == SelectedUnit.CurrentPosition)
                    {
                        secret.OpenSecret(SelectedUnit);
                        SecretField.GenerateSecret(this, secret);
                    }

                    DeselectUnit();

                    DownLightPossibleMoves();

                    SetTurn();

                    SelectedUnit = null;
                }
            }
            
            translateTransform = new TranslateTransform();

            translateTransform.X = 0;
            translateTransform.Y = 0;

            mouseXOffset = 0;
            mouseYOffset = 0;

            image.RenderTransform = translateTransform;            
        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isSomeUnitSelected)
            {
                return;
            }

            Image image = (Image)sender;
            Border border = (Border)image.Parent;
            Grid grid = (Grid)border.Parent;

            Unit targetUnit = GetUnitOnMousePosition(e.GetPosition(grid), grid);
            bool successAttack = false;

            if (SelectedUnit.GetType().BaseType.Name != targetUnit.GetType().BaseType.Name)
            {
                SelectedUnit.Attack(targetUnit, out successAttack);
            }
            else if (SelectedUnit.Type == UnitTypes.Shaman)
            {
                (SelectedUnit as HordeShaman).Heal(targetUnit);
            }
            else if (SelectedUnit.Type == UnitTypes.Priest)
            {
                (SelectedUnit as AlliancePriest).Heal(targetUnit);
            }

            if (successAttack)
            {
                DeselectUnit();
                SetTurn();
            }
        }

        private void HighLightPossibleEnemy(object sender, Unit HoveredUnit)
        {
            if (SelectedUnit != null &&
                ((SelectedUnit.Race == RaceTypes.Horde && HordeTurn) || (SelectedUnit.Race == RaceTypes.Alliance && !HordeTurn)) &&
                (SelectedUnit.Race != HoveredUnit.Race) &&
                SelectedUnit.IsCorrectMove(HoveredUnit.CurrentPosition) &&
                SelectedUnit.IsClearWay(HoveredUnit.CurrentPosition))
            {
                ((sender as Image).Parent as Border).BorderBrush = Brushes.Red;
                ((sender as Image).Parent as Border).BorderThickness = new Thickness(2);
            }
        }

        private void HighLightPossibleMoves()
        {
            for (int i = 0; i < Playfield.Children.Count; i++)
            {
                if (Playfield.Children[i] as Border != null)
                {
                    int row = (int)Playfield.Children[i].GetValue(Grid.RowProperty);
                    int col = (int)Playfield.Children[i].GetValue(Grid.ColumnProperty);
                    if (SelectedUnit.IsCorrectMove(new Point(col, row)) && SelectedUnit.IsClearWay(new Point(col, row)) &&
                        SelectedUnit.IsSomeoneAtThisPosition(new Point(col, row)))
                    { 
                        (Playfield.Children[i] as Border).BorderBrush = Brushes.LightGreen;
                        (Playfield.Children[i] as Border).BorderThickness = new Thickness(2);
                    }
                }
            }
        }

        private void DownLightPossibleMoves()
        {
            for (int i = 0; i < Playfield.Children.Count; i++)
            {
                if (Playfield.Children[i] as Border != null)
                {
                    (Playfield.Children[i] as Border).BorderBrush = Brushes.Transparent;
                }
            }
        }

        private void DeselectUnit()
        {
            //Unmark the selected item
            selectedBorder.BorderThickness = new Thickness(0, 0, 0, 0);
            selectedBorder.BorderBrush = Brushes.Transparent;
            //var path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\empty_cell.png");
            //selectedBorder.Background = new ImageBrush(new BitmapImage(new Uri(path, UriKind.Absolute)));
            selectedBorder.Background = null;

            //Deselect the selected unit
            SelectedUnit.IsSelected = false;
            isSomeUnitSelected = false;
        }

        private void SetTurn()
        {
            HordeTurn = !HordeTurn;

            var allianceLogoPath = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\alliance_turn.png");
            var hordeLogoPath = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\horde_turn.png");

            BitmapImage allianceLogo = new BitmapImage(new Uri(allianceLogoPath, UriKind.Absolute));
            BitmapImage hordeLogo = new BitmapImage(new Uri(hordeLogoPath, UriKind.Absolute));

            this.RaceTurn.Source = HordeTurn ? hordeLogo : allianceLogo;
        }

        public void SetRightSidebarStats(Unit hoveredUnit)
        {
            this.Health.Text = "Health: " + hoveredUnit.HealthLevel.ToString();
            this.Damage.Text = "Attack: " + hoveredUnit.AttackLevel.ToString();
            this.Defence.Text = "Defence: " + hoveredUnit.CounterAttackLevel.ToString();
            this.Level.Text = "Level: " + hoveredUnit.Level.ToString();
        }

        public void SetRightSidebarImage(object sender)
        {
            Image img = new Image();

            string smallImgSource = (sender as Image).Source.ToString();

            string bigImgSource = smallImgSource.Replace("small", "big");

            img.Source = new BitmapImage(new Uri(bigImgSource, UriKind.Absolute));

            this.BigCardImage.Source = img.Source;
        }

        private Unit GetUnitOnMousePosition(Point position, Grid grid)
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

        public static Unit GetUnitOnPosition(Point position)
        {
            foreach (var alliance in InitializedTeams.AllianceTeam)
            {
                if (alliance.CurrentPosition == position)
                {
                    return alliance;
                }
            }

            foreach (var horde in InitializedTeams.HordeTeam)
            {
                if (horde.CurrentPosition == position)
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
            
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\empty_cell.png");

            foreach (var child in this.Playfield.Children)
            {
                (child as Border).BorderThickness = new Thickness(2);
                (child as Border).BorderBrush = null;
                (child as Border).Background = new ImageBrush(new BitmapImage(new Uri(path, UriKind.Absolute)));
            }
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
                allianceUnit.SmallImage.MouseRightButtonDown += new MouseButtonEventHandler(Image_MouseRightButtonDown);                

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
                hordeUnit.SmallImage.MouseRightButtonDown += new MouseButtonEventHandler(Image_MouseRightButtonDown);

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
        
        private void InitializeSecret()
        {
            secret = SecretField.GetRandomSecret();
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
    }
}