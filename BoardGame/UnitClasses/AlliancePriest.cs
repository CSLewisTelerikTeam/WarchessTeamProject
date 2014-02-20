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
    class AlliancePriest : RaceAlliance, IHealing
    {
        //Attack & Health start values
        public const int InitialAttackLevel = 0;
        public const int InitialHealthLevel = 12;

        //Unit constructor
        public AlliancePriest(double col, double row)
        {
            this.Type = UnitTypes.Priest;
            this.AttackLevel = InitialAttackLevel;
            this.HealthLevel = InitialHealthLevel;
            this.CounterAttackLevel = InitialAttackLevel / 2;
            this.CurrentPosition = new Point(col, row);

            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\Frames\priest_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\Frames\priest_big.png");
            this.BigImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        public override bool IsMoveable(Point destination)
        {
            return false;
        }
                
        public void Heal(Unit objectToHeal)
        {
            //Healing method. "objectToHeal" is the unit that will be healed.
            if (this.HealthLevel > 0)
            {
                //Increase target unit's health level
                //if the priest has 1 health, he cant restore 2 health
                if (this.HealthLevel == 1)
                {
                    objectToHeal.HealthLevel++;
                    this.HealthLevel -= 1;
                }

                else
                {
                    objectToHeal.HealthLevel += 2;
                    this.HealthLevel -= 2;
                }
                
                //check if the priest is dead
                if (this.HealthLevel <= 0)
                {
                    this.IsAlive = false;
                }
            }
        }
    }
}
