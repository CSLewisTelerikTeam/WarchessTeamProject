using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BoardGame.UnitClasses
{
    class HordeWarlock : RaceHorde
    {
        //Attack & Health start values
        private const double InitialAttackLevel = 30;
        private const double InitialHealthLevel = 60;

        //Unit constructor
        public HordeWarlock(double col, double row)
        {
            this.Type = UnitTypes.Warlock;
            this.AttackLevel = InitialAttackLevel;
            this.HealthLevel = InitialHealthLevel;
            this.CurrentPosition = new Point(col, row);

            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\warlock_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\warlock_big.png");
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

            int deltaRow = (int)Math.Abs(destination.Y - this.CurrentPosition.Y);
            int deltaCol = (int)Math.Abs(destination.X - this.CurrentPosition.X);

            //Check if the destination cell is corresponding to the unit move rules
            if ((deltaRow == 2 && deltaCol == 1) || (deltaRow == 1 && deltaCol == 2))
            {
                return true;
            }

            return false;
        }
    }
}
