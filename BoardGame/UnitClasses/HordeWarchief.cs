namespace BoardGame.UnitClasses
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    
    class HordeWarchief : RaceHorde
    {
        //Attack & Health start values
        private const int Attack = 8;
        private const int Health = 10;

        //Unit constructor
        public HordeWarchief(Point currentPosition) : base(UnitTypes.Warchief, Health, Attack,
        0, true, currentPosition, false)
        {
            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\warchief_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\warchief_big.png");
            this.BigImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        public override void PlayAttackSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Horde\Warchief_Attack.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override void PlaySelectSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Horde\Warchief_Select.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override void PlayDieSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Horde\Warchief_Death.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override bool IsClearWay(Point destination)
        {
            double deltaRow = destination.Y - this.CurrentPosition.Y;
            double deltaCol = destination.X - this.CurrentPosition.X;

            //Check horizonal line if it's clear to move
            if (deltaRow == 0)
            {
                if (deltaCol > 0)
                {
                    for (double currentCol = this.CurrentPosition.X + 1; currentCol < destination.X; currentCol++)
                    {
                        foreach (var unit in InitializedTeams.AllianceTeam)
                        {
                            if (unit.CurrentPosition.X == currentCol && unit.CurrentPosition.Y == this.CurrentPosition.Y)
                            {
                                return false;
                            }
                        }

                        foreach (var unit in InitializedTeams.HordeTeam)
                        {
                            if (unit.CurrentPosition.X == currentCol && unit.CurrentPosition.Y == this.CurrentPosition.Y)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }

                if (deltaCol < 0)
                {
                    for (double currentCol = this.CurrentPosition.X - 1; currentCol > destination.X; currentCol--)
                    {
                        foreach (var unit in InitializedTeams.AllianceTeam)
                        {
                            if (unit.CurrentPosition.X == currentCol && unit.CurrentPosition.Y == this.CurrentPosition.Y)
                            {
                                return false;
                            }
                        }

                        foreach (var unit in InitializedTeams.HordeTeam)
                        {
                            if (unit.CurrentPosition.X == currentCol && unit.CurrentPosition.Y == this.CurrentPosition.Y)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }
            }
            //Check vertical line if it's clear to move
            else if (deltaCol == 0)
            {
                if (deltaRow > 0)
                {
                    for (double currentRow = this.CurrentPosition.Y + 1; currentRow < destination.Y; currentRow++)
                    {
                        foreach (var unit in InitializedTeams.AllianceTeam)
                        {
                            if (unit.CurrentPosition.Y == currentRow && unit.CurrentPosition.X == this.CurrentPosition.X)
                            {
                                return false;
                            }
                        }

                        foreach (var unit in InitializedTeams.HordeTeam)
                        {
                            if (unit.CurrentPosition.Y == currentRow && unit.CurrentPosition.X == this.CurrentPosition.X)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }

                if (deltaRow < 0)
                {
                    for (double currentRow = this.CurrentPosition.Y - 1; currentRow > destination.Y; currentRow--)
                    {
                        foreach (var unit in InitializedTeams.AllianceTeam)
                        {
                            if (unit.CurrentPosition.Y == currentRow && unit.CurrentPosition.X == this.CurrentPosition.X)
                            {
                                return false;
                            }
                        }

                        foreach (var unit in InitializedTeams.HordeTeam)
                        {
                            if (unit.CurrentPosition.Y == currentRow && unit.CurrentPosition.X == this.CurrentPosition.X)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }
            }
            // Check diagonal line if it's clear to move
            else if (Math.Abs(deltaRow) == Math.Abs(deltaCol))
            {
                double currentRow = this.CurrentPosition.Y;
                double currentCol = this.CurrentPosition.X;

                for (int i = 0; i < Math.Abs(deltaRow) - 1; i++)
                {
                    if (deltaRow < 0 && deltaCol < 0)
                    {
                        currentCol--;
                        currentRow--;

                        foreach (var unit in InitializedTeams.AllianceTeam)
                        {
                            if (currentRow == unit.CurrentPosition.Y && currentCol == unit.CurrentPosition.X)
                            {
                                return false;
                            }
                        }

                        foreach (var unit in InitializedTeams.HordeTeam)
                        {
                            if (currentRow == unit.CurrentPosition.Y && currentCol == unit.CurrentPosition.X)
                            {
                                return false;
                            }
                        }
                    }
                    else if (deltaRow < 0 && deltaCol > 0)
                    {
                        currentCol++;
                        currentRow--;
                        foreach (var unit in InitializedTeams.AllianceTeam)
                        {
                            if (currentRow == unit.CurrentPosition.Y && currentCol == unit.CurrentPosition.X)
                            {
                                return false;
                            }
                        }

                        foreach (var unit in InitializedTeams.HordeTeam)
                        {
                            if (currentRow == unit.CurrentPosition.Y && currentCol == unit.CurrentPosition.X)
                            {
                                return false;
                            }
                        }
                    }
                    else if (deltaRow > 0 && deltaCol > 0)
                    {
                        currentCol++;
                        currentRow++;
                        foreach (var unit in InitializedTeams.AllianceTeam)
                        {
                            if (currentRow == unit.CurrentPosition.Y && currentCol == unit.CurrentPosition.X)
                            {
                                return false;
                            }
                        }

                        foreach (var unit in InitializedTeams.HordeTeam)
                        {
                            if (currentRow == unit.CurrentPosition.Y && currentCol == unit.CurrentPosition.X)
                            {
                                return false;
                            }
                        }
                    }
                    else if (deltaRow > 0 && deltaCol < 0)
                    {
                        currentCol--;
                        currentRow++;
                        foreach (var unit in InitializedTeams.AllianceTeam)
                        {
                            if (currentRow == unit.CurrentPosition.Y && currentCol == unit.CurrentPosition.X)
                            {
                                return false;
                            }
                        }

                        foreach (var unit in InitializedTeams.HordeTeam)
                        {
                            if (currentRow == unit.CurrentPosition.Y && currentCol == unit.CurrentPosition.X)
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public override bool IsCorrectMove(Point destination)
        {
            double deltaRow = destination.Y - this.CurrentPosition.Y;
            double deltaCol = destination.X - this.CurrentPosition.X;

            if (deltaRow == 0 && deltaCol != 0)
            {
                return true;
            }
            else if (deltaRow != 0 && deltaCol == 0)
            {
                return true;
            }
            else if (Math.Abs(deltaRow) == Math.Abs(deltaCol))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}