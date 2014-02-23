using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BoardGame.UnitClasses
{
    class HordeCommander : RaceHorde
    {
        //Attack & Health start values
        private const int Attack = 6;
        private const int Health = 6;

        //Unit constructor
        public HordeCommander(Point currentPosition)
            : base(UnitTypes.Commander, Health, Attack,
                       0, true, currentPosition, false)
        {
            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\commander_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\commander_big.png");
            this.BigImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        public override void PlayAttackSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Horde\Commander_Attack.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override void PlaySelectSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Horde\Commander_Select.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override void PlayDieSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Horde\Commander_Death.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override bool IsCorrectMove(Point destination)
        {
            double deltaCol = destination.X - this.CurrentPosition.X;
            double deltaRow = destination.Y - this.CurrentPosition.Y;

            if (Math.Abs(deltaRow) == Math.Abs(deltaCol))
            {
                return true;
            }
            return false;
        }

        public override bool IsClearWay(Point destination)
        {
            double deltaCol = destination.X - this.CurrentPosition.X;
            double deltaRow = destination.Y - this.CurrentPosition.Y;

            // Check diagonal line if it's clear to move
            if (Math.Abs(deltaRow) == Math.Abs(deltaCol))
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
    }
}
