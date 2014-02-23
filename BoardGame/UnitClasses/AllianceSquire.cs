namespace BoardGame.UnitClasses
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    class AllianceSquire : RaceAlliance
    {
        //Attack & Health start values
        private const int Attack = 4;
        private const int Health = 4;

        //Unit constructor
        public AllianceSquire(Point currentPosition) : base(UnitTypes.Squire, Health, Attack,
        0, true, currentPosition, false)
        {
            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\Frames\squire_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\Frames\squire_big.png");
            this.BigImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        public override void PlayAttackSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Alliance\Squire_Attack.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override void PlaySelectSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Alliance\Squire_Select.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override void PlayDieSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Alliance\Squire_Death.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }

        public override bool IsClearWay(Point destination)
        {
            return true;
        }

        public override bool IsCorrectMove(Point destination)
        {
            double deltaCol = destination.X - this.CurrentPosition.X;
            double deltaRow = destination.Y - this.CurrentPosition.Y;

            //invalid move
            if (Math.Abs(deltaCol) > 1 || Math.Abs(deltaRow) > 1)
            {
                return false;
            }
            return true;
        }
    }
}