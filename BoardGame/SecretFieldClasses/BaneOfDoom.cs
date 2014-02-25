namespace BoardGame.SecretFieldClasses
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using BoardGame.UnitClasses;
    using System;

    class BaneOfDoom : HarmfulField
    {
        public BaneOfDoom() : base(SecretFields.KenovsBaneOfDoom)
        {
            this.SmallImage = new Image();
            this.SmallImage.Source = new BitmapImage(new Uri(SecretField.smallImgPath, UriKind.Absolute));
            this.BigImage = new Image();
            this.BigImage.Source = new BitmapImage(new Uri(SecretField.bigImgPath, UriKind.Absolute));
        }

        public override void OpenSecret(Unit target)
        {
            if (target.HealthLevel - 2 <= 0)
            {
                target.PlayDieSound();
                target.SmallImage.Source = new BitmapImage();
                (target.SmallImage.Parent as Border).Background = null;
                target.CurrentPosition = new Point(-1, -1);
                target.IsAlive = false;
            }
            else
            {
                target.HealthLevel -= 2;
            }
        }
    }
}