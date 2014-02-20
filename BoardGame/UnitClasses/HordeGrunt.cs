using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BoardGame.UnitClasses
{
    class HordeGrunt : RaceHorde
    {
        //Attack & Health start values
        private const double InitialAttackLevel = 2;
        private const double InitialHealthLevel = 4;

        //Unit constructor
        public HordeGrunt(double col, double row)
        {
            this.Type = UnitTypes.Grunt;
            this.AttackLevel = InitialAttackLevel;
            this.HealthLevel = InitialHealthLevel;
            this.CounterAttackLevel = InitialAttackLevel / 2;
            this.CurrentPosition = new Point(col, row);

            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\grunt_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\grunt_big.png");
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

            //invalid move
            if (Math.Abs(deltaCol) > 1 || Math.Abs(deltaRow) > 1)
            {
                return false;
            }
            return true;
        }
    }
}
