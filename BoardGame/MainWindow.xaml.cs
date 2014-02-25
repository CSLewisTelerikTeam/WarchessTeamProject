using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BoardGame.SecretFieldClasses;
using BoardGame.UnitClasses;
using BoardGame;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            DrawSecret();
            ChooseTurn();
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
        private bool isTurnSelected;
        private static bool turn = false;
        private RaceTypes winner;
        private bool isFirstSecret = true;
        
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

            if (SelectedUnit != null && (SelectedUnit.Type == UnitTypes.Priest || SelectedUnit.Type == UnitTypes.Shaman))
            {
                //Mark object's cell border in blue 
                HighLightPossibleObjectToHeal(sender, hoveredUnit);
            }
            else
            {
                //Mark enemy's cell border in red 
                HighLightPossibleEnemy(sender, hoveredUnit);
            }
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
                ClearRightSidebar();
            }
            
            DownLightPossibleMoves();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            Border border = (Border)image.Parent;
            Grid grid = (Grid)border.Parent;

            SelectedUnit = GetUnitOnMousePosition(e.GetPosition(grid), grid);

            RaceTypes RaceTurn = turn ? RaceTypes.Horde : RaceTypes.Alliance;

            if (!isTurnSelected || SelectedUnit.Race != RaceTurn)
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
                    //Change the selected unit current position if the unit can move on that position
                    SelectedUnit.CurrentPosition = new Point(coordinates.X, coordinates.Y);

                    //Open the secret field if the coordinates matches
                    if (secret.CurrentPosition == SelectedUnit.CurrentPosition)
                    {
                        secret.OpenSecret(SelectedUnit);
                        secret.RevealSound();
                        DrawSecretInfo(secret);
                        DrawSecret();
                    }

                    try
                    {
                        if (coordinates.X < 0 || coordinates.X > 7 || 
                            coordinates.Y <0 || coordinates.Y > 7)
                        {
                            throw new OutOfGameFieldException();
                        }
                        Grid.SetRow(border, (int)coordinates.Y);
                        Grid.SetColumn(border, (int)coordinates.X);
                    }
                    catch (OutOfGameFieldException)
                    {
                        translateTransform = new TranslateTransform();

                        translateTransform.X = 0;
                        translateTransform.Y = 0;

                        mouseXOffset = 0;
                        mouseYOffset = 0;

                        image.RenderTransform = translateTransform;            
                        return;
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
            bool successHeal;

            if (SelectedUnit == null)
            {
                return;
            }
           
            if (SelectedUnit.GetType().BaseType.Name != targetUnit.GetType().BaseType.Name)
            {
                SelectedUnit.Attack(targetUnit, out successAttack);
                bool isGameOver = IsGameOver();
                if (isGameOver)
                {
                    GameOver(winner);
                }
            }
            else if (SelectedUnit.Type == UnitTypes.Shaman)
            {
                (SelectedUnit as HordeShaman).Heal(targetUnit, out successHeal);
                if (successHeal)
                {
                    DeselectUnit();
                    SetTurn();
                }
            }
            else if (SelectedUnit.Type == UnitTypes.Priest)
            {
                (SelectedUnit as AlliancePriest).Heal(targetUnit, out successHeal);
                if (successHeal)
                {
                    DeselectUnit();
                    SetTurn();
                }
            }

            if (successAttack)
            {
                DeselectUnit();
                SetTurn();
            }
        }
        
        private void WinButton_MouseEnter(object sender, MouseEventArgs e)
        {
            string replacedPath = (sender as Image).Source.ToString().Replace("unhover", "hover");
            (sender as Image).Source = new BitmapImage(new Uri(replacedPath, UriKind.Absolute));
        }

        private void WinButton_MouseLeave(object sender, MouseEventArgs e)
        {
            string replacedPath = (sender as Image).Source.ToString().Replace("hover", "unhover");
            (sender as Image).Source = new BitmapImage(new Uri(replacedPath, UriKind.Absolute));
        }

        private void WinButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((sender as Image).Parent as Border).Name == "Back")
            {
                backgroundMusic.Stop();
                ResetState();
                var startScreen = new StartScreen();
                startScreen.Owner = this.Owner;
                startScreen.Show();
                this.Close();
            }
            else if (((sender as Image).Parent as Border).Name == "HordeButton")
            {
                isTurnSelected = true;
                turn = false;
                ChangeRaceChooseToRace();

                Task.Factory.StartNew(() =>
                {
                    MediaPlayer hordeChoose = new MediaPlayer();
                    var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Horde\horde_choose.mp3");
                    hordeChoose.Open(new Uri(path));
                    hordeChoose.Play();
                });

                SetTurn();
            }
            else if (((sender as Image).Parent as Border).Name == "AllianceButton")
            {
                isTurnSelected = true;
                turn = true;
                ChangeRaceChooseToRace();

                Task.Factory.StartNew(() =>
                {
                    MediaPlayer allianceChoose = new MediaPlayer();
                    var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Alliance\alliance_choose.mp3");
                    allianceChoose.Open(new Uri(path));
                    allianceChoose.Play();
                });

                SetTurn();
            }
        }

        private void ChooseTurn()
        {
            //Initialize the buttons and the zIndex of the Canvas
            Image hordeButton = new Image();
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\button_horde_unhover.png");
            hordeButton.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.HordeButton.Child = hordeButton;
            this.HordeButton.Child.MouseEnter += new MouseEventHandler(WinButton_MouseEnter);
            this.HordeButton.Child.MouseLeave += new MouseEventHandler(WinButton_MouseLeave);
            this.HordeButton.Child.MouseLeftButtonDown += new MouseButtonEventHandler(WinButton_MouseLeftButtonDown);

            Image allianceButton = new Image();
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\button_alliance_unhover.png");
            allianceButton.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.AllianceButton.Child = allianceButton;
            this.AllianceButton.Child.MouseEnter += new MouseEventHandler(WinButton_MouseEnter);
            this.AllianceButton.Child.MouseLeave += new MouseEventHandler(WinButton_MouseLeave);
            this.AllianceButton.Child.MouseLeftButtonDown += new MouseButtonEventHandler(WinButton_MouseLeftButtonDown);
        }

        private void ChangeRaceChooseToRace()
        { 
            this.ChooseText.Text = "";
            this.HordeButton.Child = new Image();
            this.AllianceButton.Child = new Image();
            Canvas.SetZIndex(this.ChooseRaceTurn, -10);
            this.ChooseRaceTurn = new Canvas();
        }

        private void SetSecretInfo()
        {
            Canvas.SetZIndex(this.SecretInfo, 100);
            ImageBrush img = new ImageBrush();
            string path;

            if (secret.SecretFieldName == SecretFields.KenovsBaneOfDoom)
            {
                path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\secret_revealed_baneofdoom.png");
                img.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
                this.SecretInfo.Background = img;
            }
            else if (secret.SecretFieldName == SecretFields.KostovsBlessingOfWisdom)
            {
                path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\secret_revealed_blessingofwisdom.png");
                img.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
                this.SecretInfo.Background = img;
            }
            else if (secret.SecretFieldName == SecretFields.MinkovsCurseOfWeakness)
            {
                path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\secret_revealed_curseofweakness.png");
                img.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
                this.SecretInfo.Background = img;
            }
            else if (secret.SecretFieldName == SecretFields.GeorgievsShoutOfStrength)
            {
                path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\secret_revealed_shoutofstrength.png");
                img.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
                this.SecretInfo.Background = img;
            }
        }

        private async void RemoveSecretInfo()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            this.SecretInfo.Background = new ImageBrush();
            Canvas.SetZIndex(this.SecretInfo, 0);
        }

        private void DrawSecretInfo(SecretField secret)
            {
            SetSecretInfo();
            RemoveSecretInfo();
        }
  
        private void DrawSecret()
        {
            int secretX;
            int secretY;

            if (secret != null)
            {
                secretX = (int)secret.CurrentPosition.X;
                secretY = (int)secret.CurrentPosition.Y;
            }
            else
            {
                secretX = 0;
                secretY = 0;
            }

            InitializeSecret();

            secret.CurrentPosition = new Point(secretX, secretY);

            Border secretField;
            if (isFirstSecret)
            {
                isFirstSecret = false;
                secretField = (Border)this.Playfield.FindName("Unit20");
                secretField.Child = null;
            }
            else
            {
                secretField = (Border)this.Playfield.FindName("Unit" + secret.CurrentPosition.Y + secret.CurrentPosition.X);
                secretField.Child = null;
            }

            secret = SecretField.GenerateSecret();

            secretField = (Border)this.Playfield.FindName("Unit" + secret.CurrentPosition.Y + secret.CurrentPosition.X);
            secretField.Child = secret.SmallImage;
            }

        private void ResetState()
        {
            foreach (var item in this.Playfield.Children)
            {
                (item as Border).Child = new Image();
            }

            SelectedUnit = null;
            secret = null;

            InitializedTeams.AllianceTeam = new List<RaceAlliance>
            {
                new AllianceSquire(new Point(0, 6)),
                new AllianceSquire(new Point(1, 6)),
                new AllianceSquire(new Point(2, 6)),
                new AllianceSquire(new Point(3, 6)),
                new AllianceSquire(new Point(4, 6)),
                new AllianceSquire(new Point(5, 6)),
                new AllianceSquire(new Point(6, 6)),
                new AllianceSquire(new Point(7, 6)),
                new AllianceMage(new Point(1, 7)),
                new AllianceMage(new Point(6, 7)),
                new AllianceCaptain(new Point(2, 7)),
                new AllianceCaptain(new Point(5, 7)),
                new AllianceWarGolem(new Point(0, 7)),
                new AllianceWarGolem(new Point(7, 7)),
                new AlliancePriest(new Point(3, 7)),
                new AllianceKing(new Point(4, 7)),
            };

            InitializedTeams.HordeTeam = new List<RaceHorde>
            {
                new HordeGrunt(new Point(0, 1)),
                new HordeGrunt(new Point(1, 1)),
                new HordeGrunt(new Point(2, 1)),
                new HordeGrunt(new Point(3, 1)),
                new HordeGrunt(new Point(4, 1)),
                new HordeGrunt(new Point(5, 1)),
                new HordeGrunt(new Point(6, 1)),
                new HordeGrunt(new Point(7, 1)),
                new HordeWarlock(new Point(1, 0)),
                new HordeWarlock(new Point(6, 0)),
                new HordeCommander(new Point(2, 0)),
                new HordeCommander(new Point(5, 0)),
                new HordeDemolisher(new Point(0, 0)),
                new HordeDemolisher(new Point(7, 0)),
                new HordeShaman(new Point(3, 0)),
                new HordeWarchief(new Point(4, 0)),
            };

            isMouseCapture = false;
            mouseXOffset = 0;
            mouseYOffset = 0;
            translateTransform = new TranslateTransform();
            backgroundMusic = new MediaPlayer();
            isSomeUnitSelected = false;
            selectedBorder = new Border();
            turn = true;
        }

        private void GameOver(RaceTypes winner)
        {
            Image playAgainBtn = new Image();
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\backtomenu_unhover.png");
            playAgainBtn.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.Back.Child = playAgainBtn;
            this.Back.Child.MouseEnter += new MouseEventHandler(WinButton_MouseEnter);
            this.Back.Child.MouseLeave += new MouseEventHandler(WinButton_MouseLeave);
            this.Back.Child.MouseLeftButtonDown += new MouseButtonEventHandler(WinButton_MouseLeftButtonDown);

            Canvas.SetZIndex(this.WinnerScreen, 10);

            ImageBrush img = new ImageBrush();

            if (winner == RaceTypes.Alliance)
            {
                path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\alliance_wins.png");
                img.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
                this.WinnerScreen.Background = img;
                backgroundMusic.Stop();
                path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\alliance_wins.mp3");
                backgroundMusic.Open(new Uri(path));
                backgroundMusic.Play();
            }
            else if (winner == RaceTypes.Horde)
            {
                path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\horde_wins.png");
                img.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
                this.WinnerScreen.Background = img;
                backgroundMusic.Stop();
                path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\horde_wins.mp3");
                backgroundMusic.Open(new Uri(path));
                backgroundMusic.Play();
            }
            else
            {
                path = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\draw.png");
                img.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
                this.WinnerScreen.Background = img;
                backgroundMusic.Stop();
                path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\draw.mp3");
                backgroundMusic.Open(new Uri(path));
                backgroundMusic.Play();
            }
        }

        private bool IsGameOver()
        {
            bool isAllianceWinner = false;
            bool isHordeWinner = false;

            if (!InitializedTeams.AllianceTeam[14].IsAlive && !InitializedTeams.AllianceTeam[15].IsAlive)
            {
                isHordeWinner = true;
            }

            if (!InitializedTeams.HordeTeam[14].IsAlive && !InitializedTeams.HordeTeam[15].IsAlive)
            {
                isAllianceWinner = true;
            }

            if (isAllianceWinner)
            {
                if (isHordeWinner)
                {
                    winner = RaceTypes.Hybrid;
                    return true;
                }
                else
                {
                    winner = RaceTypes.Alliance;
                    return true;
                }
            }
            else
            {
                if (isHordeWinner)
                {
                    winner = RaceTypes.Horde;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void HighLightPossibleEnemy(object sender, Unit HoveredUnit)
        {
            if ((SelectedUnit != null && HoveredUnit != null) &&
                ((SelectedUnit.Race == RaceTypes.Horde && turn) || (SelectedUnit.Race == RaceTypes.Alliance && !turn)) &&
                (SelectedUnit.Race != HoveredUnit.Race) &&
                SelectedUnit.IsCorrectMove(HoveredUnit.CurrentPosition) &&
                SelectedUnit.IsClearWay(HoveredUnit.CurrentPosition))
            {
                ((sender as Image).Parent as Border).BorderBrush = Brushes.Red;
                ((sender as Image).Parent as Border).BorderThickness = new Thickness(2);
            }
        }

        private void HighLightPossibleObjectToHeal(object sender, Unit HoveredUnit)
        {
            if (HoveredUnit == null)
            {
                return;
            }

            if ((SelectedUnit.Race == HoveredUnit.Race) && HoveredUnit.HealthLevel < HoveredUnit.MaxHealthLevel)
            {
                ((sender as Image).Parent as Border).BorderBrush = Brushes.DeepSkyBlue;
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

        private void ClearRightSidebar()
        {
            this.BigCardImage.Source = null;

            this.Health.Text = "";
            this.Damage.Text = "";
            this.Defence.Text = "";
            this.Level.Text = "";
        }

        private void SetTurn()
        {
            turn = !turn;

            var allianceLogoPath = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\alliance_turn.png");
            var hordeLogoPath = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\horde_turn.png");

            BitmapImage allianceLogo = new BitmapImage(new Uri(allianceLogoPath, UriKind.Absolute));
            BitmapImage hordeLogo = new BitmapImage(new Uri(hordeLogoPath, UriKind.Absolute));

            if (isTurnSelected)
            {
                this.RaceTurn.Source = turn ? hordeLogo : allianceLogo;
            }
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

            foreach (var secret in InitializedSecrets.Secrets)
            {
                secret.SmallImage.MouseEnter += new MouseEventHandler(Image_MouseEnter);
                secret.SmallImage.MouseLeave += new MouseEventHandler(Image_MouseLeave);
                var secField = (Border)this.Playfield.FindName("Unit20");
                secField.Child = (InitializedSecrets.Secrets[0]).SmallImage;
            }
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