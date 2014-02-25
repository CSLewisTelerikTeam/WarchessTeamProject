namespace BoardGame.SecretFieldClasses
{
    using BoardGame.UnitClasses;
    using System;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    
    class BlessingOfWisdom : HelpfulField
    {
        public BlessingOfWisdom() : base(SecretFields.KostovsBlessingOfWisdom)
        {
            this.SmallImage = new Image();
            this.SmallImage.Source = new BitmapImage(new Uri(SecretField.smallImgPath, UriKind.Absolute));
            this.BigImage = new Image();
            this.BigImage.Source = new BitmapImage(new Uri(SecretField.bigImgPath, UriKind.Absolute));
        }

        public override void OpenSecret(Unit target)
        {
            target.HealthLevel += 2;
            target.MaxHealthLevel += 2;
        }
    }
}