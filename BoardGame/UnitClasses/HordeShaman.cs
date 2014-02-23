using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BoardGame.UnitClasses
{
    class HordeShaman : RaceHorde, IHealing
    {
        //Attack & Health start values
        private const int Attack = 0;
        private const int Health = 12;

        //Unit constructor
        public HordeShaman(Point currentPosition)
            : base(UnitTypes.Shaman, Health, Attack,
                       0, true, currentPosition, false)
        {
            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\shaman_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Horde\Frames\shaman_big.png");
            this.BigImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        public override void PlayAttackSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Horde\Shaman_Spell.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override void PlaySelectSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Horde\Shaman_Select.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override void PlayDieSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Horde\Shaman_Death.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override bool IsClearWay(Point destination)
        {
            return false;
        }

        public override bool IsCorrectMove(Point destination)
        {
            return false;
        }

        public void Heal(Unit objectToHeal)
        {
            //Healing method. "objectToHeal" is the unit that will be healed.
            if (this.HealthLevel > 0)
            {
                //Increase target unit's health level
                //if the priest has 1 health, he can't restore 2 health
                if (this.HealthLevel == 1)
                {
                    if (objectToHeal.HealthLevel == objectToHeal.MaxHealthLevel)
                    {
                        return;
                    }

                    if ((objectToHeal.HealthLevel + 1) > objectToHeal.MaxHealthLevel)
                    {
                        objectToHeal.HealthLevel = objectToHeal.MaxHealthLevel;
                    }
                    else
                    {
                        objectToHeal.HealthLevel++;
                    }
                    
                    this.HealthLevel -= 1;
                }
                else
                {
                    if (objectToHeal.HealthLevel == objectToHeal.MaxHealthLevel)
                    {
                        return;
                    }

                    if ((objectToHeal.HealthLevel + 2) > objectToHeal.MaxHealthLevel)
                    {
                        objectToHeal.HealthLevel = objectToHeal.MaxHealthLevel;
                        this.HealthLevel -= 1;
                    }
                    else
                    {
                        objectToHeal.HealthLevel += 2;
                        this.HealthLevel -= 2;
                    }
                }

                //Check if the priest is dead
                if (this.HealthLevel <= 0)
                {
                    this.IsAlive = false;
                    this.SmallImage.Source = new BitmapImage();
                    (this.SmallImage.Parent as Border).Background = null;
                    this.CurrentPosition = new Point(-1, -1);
                }
            }
        }
    }
}
