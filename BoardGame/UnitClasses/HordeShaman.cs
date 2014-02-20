using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BoardGame.UnitClasses
{
    class HordeShaman : RaceHorde
    {
        //Attack & Health start values
        private const double InitialAttackLevel = 0;
        private const double InitialHealthLevel = 50;

        //Unit constructor
        public HordeShaman(double col, double row)
        {
            this.Type = UnitTypes.Shaman;
            this.AttackLevel = InitialAttackLevel;
            this.HealthLevel = InitialHealthLevel;
            this.CurrentPosition = new Point(col, row);

            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\shaman_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\shaman_big.png");
            this.BigImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }
        public override bool IsMoveable(Point destination)
        {
            return false;
        }
    }
}
