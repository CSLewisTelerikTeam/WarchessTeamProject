using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BoardGame.UnitClasses
{
    class HordeWarchief : RaceHorde
    {
        //Attack & Health start values
        private const double InitialAttackLevel = 5;
        private const double InitialHealthLevel = 10;

        //Unit constructor
        public HordeWarchief(double col, double row)
        {
            this.Type = UnitTypes.Warchief;
            this.AttackLevel = InitialAttackLevel;
            this.HealthLevel = InitialHealthLevel;
            this.CounterAttackLevel = InitialAttackLevel / 2;
            this.CurrentPosition = new Point(col, row);

            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\warchief_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\warchief_big.png");
            this.BigImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        public override bool IsMoveable(Point destination)
        {
            //Check if the destination cell is not busy of Alliance unit
            foreach (var unit in InitializedTeams.HordeTeam)
            {
                if (destination.X == unit.CurrentPosition.X && destination.Y == unit.CurrentPosition.Y)
                {
                    return false;
                }
            }

            double deltaRow = destination.Y - this.CurrentPosition.Y;
            double deltaCol = destination.X - this.CurrentPosition.X;

            //Check horizonal line if it's clear to move
            if (deltaRow == 0)
            {
                if (deltaCol > 0)
                {
                    for (double currentCol = this.CurrentPosition.X + 1; currentCol <= destination.X; currentCol++)
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
                    for (double currentCol = this.CurrentPosition.X - 1; currentCol >= destination.X; currentCol--)
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
                    for (double currentRow = this.CurrentPosition.Y + 1; currentRow <= destination.Y; currentRow++)
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
                    for (double currentRow = this.CurrentPosition.Y - 1; currentRow >= destination.Y; currentRow--)
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

                for (int i = 0; i < Math.Abs(deltaRow); i++)
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
    }
}
