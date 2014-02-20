using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BoardGame.UnitClasses
{
    class AllianceMage :RaceAlliance, IMoveable
    {
        //Attack & Health start values
        public const int InitialAttackLevel = 4;
        public const int InitialHealthLevel = 6;

        //Unit constructor
        public AllianceMage(double col, double row)
        {
            this.Type = UnitTypes.Mage;
            this.AttackLevel = InitialAttackLevel;
            this.HealthLevel = InitialHealthLevel;
            this.CounterAttackLevel = InitialAttackLevel / 2;
            this.CurrentPosition = new Point(col, row);

            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\Frames\mage_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\Frames\mage_big.png");
            this.BigImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        public override bool IsClearWay(Point destination)
        { 
            return true;
        }

        public override bool IsCorrectMove(Point destination)
        {
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
