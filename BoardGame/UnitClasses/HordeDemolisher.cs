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

        public override bool IsClearWay(Point destination)
        {

            double deltaRow = destination.Y - this.CurrentPosition.Y;
            double deltaCol = destination.X - this.CurrentPosition.X;


            if (deltaCol == 0)
            {
                double currentRow = this.CurrentPosition.Y;
                double currentCol = this.CurrentPosition.X;

                for (int i = 0; i < Math.Abs(deltaRow) - 1; i++)
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

                for (int i = 0; i < Math.Abs(deltaCol) - 1; i++)
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

        public override bool IsCorrectMove(Point destination)
        {
            //Invalid move
            double deltaRow = destination.Y - this.CurrentPosition.Y;
            double deltaCol = destination.X - this.CurrentPosition.X;

            if (deltaCol != 0 && deltaRow != 0)
            {
                return false;
            }
            return true;
        }
    }
}
