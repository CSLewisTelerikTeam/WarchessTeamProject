using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BoardGame.UnitClasses
{
    class HordeDemolisher : RaceHorde
    {
        //Attack & Health start values
        public const int InitialAttackLevel = 6;
        public const int InitialHealthLevel = 8;

        //Unit constructor
        public HordeDemolisher(double col, double row)
        {
            this.Type = UnitTypes.Demolisher;
            this.AttackLevel = InitialAttackLevel;
            this.HealthLevel = InitialHealthLevel;
            this.CounterAttackLevel = InitialAttackLevel / 2;
            this.CurrentPosition = new Point(col, row);

            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\demolisher_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\demolisher_big.png");
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

            //Invalid move
            if (deltaCol != 0 && deltaRow != 0)
            {
                return false;
            }
            else if (deltaCol == 0)
            {
                double currentRow = this.CurrentPosition.Y;
                double currentCol = this.CurrentPosition.X;

                for (int i = 0; i < Math.Abs(deltaRow); i++)
                {
                    if (deltaRow < 0)
                    {
                        currentRow--;
                    }
                    else
                    {
                        currentRow++;
                    }

                    foreach (var unit in InitializedTeams.AllianceTeam)
                    {
                        if (unit.CurrentPosition.X == currentCol && unit.CurrentPosition.Y == currentRow)
                        {
                            return false;
                        }
                    }

                    foreach (var unit in InitializedTeams.HordeTeam)
                    {
                        if (unit.CurrentPosition.X == currentCol && unit.CurrentPosition.Y == currentRow)
                        {
                            return false;
                        }
                    }


                }

                return true;
            }
            else if (deltaRow == 0)
            {
                double currentRow = this.CurrentPosition.Y;
                double currentCol = this.CurrentPosition.X;

                for (int i = 0; i < Math.Abs(deltaCol); i++)
                {
                    if (deltaCol < 0)
                    {
                        currentCol--;
                    }
                    else
                    {
                        currentCol++;
                    }

                    foreach (var unit in InitializedTeams.AllianceTeam)
                    {
                        if (unit.CurrentPosition.X == currentCol && unit.CurrentPosition.Y == currentRow)
                        {
                            return false;
                        }
                    }

                    foreach (var unit in InitializedTeams.HordeTeam)
                    {
                        if (unit.CurrentPosition.X == currentCol && unit.CurrentPosition.Y == currentRow)
                        {
                            return false;
                        }
                    }


                }

                return true;
            }

            return false;
        }
    }
}
