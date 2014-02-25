namespace BoardGame.SecretFieldClasses
{
    using BoardGame.UnitClasses;
    using System;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    
    class ShoutOfStrength : HelpfulField
    {
        public ShoutOfStrength() : base(SecretFields.GeorgievsShoutOfStrength)
        {
            this.SmallImage = new Image();
            this.SmallImage.Source = new BitmapImage(new Uri(SecretField.smallImgPath, UriKind.Absolute));
            this.BigImage = new Image();
            this.BigImage.Source = new BitmapImage(new Uri(SecretField.bigImgPath, UriKind.Absolute));
        }

        public override void OpenSecret(Unit target)
        {
            target.AttackLevel += 2;
            target.MaxAttackLevel += 2;
        }
    }
}