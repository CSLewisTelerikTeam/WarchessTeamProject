namespace BoardGame.UnitClasses
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    class AllianceMage : RaceAlliance, IMoveable
    {
        //Attack & Health start values
        private const int Attack = 4;
        private const int Health = 6;

        //Unit constructor
        public AllianceMage(Point currentPosition) : base(UnitTypes.Mage, Health, Attack,
        0, true, currentPosition, false)
        {
            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\Frames\mage_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\Frames\mage_big.png");
            this.BigImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        public override void PlayAttackSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Alliance\Mage_Attack.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override void PlaySelectSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Alliance\Mage_Select.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override void PlayDieSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Alliance\Mage_Death.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
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