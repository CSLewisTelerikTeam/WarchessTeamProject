namespace BoardGame.SecretFieldClasses
{
    using System.Windows.Controls;
    using BoardGame.UnitClasses;
    using System;
    using System.Windows.Media.Imaging;
    
    class CurseOfWeakness : HarmfulField
    {
        public CurseOfWeakness() : base(SecretFields.MinkovsCurseOfWeakness)
        {
            this.SmallImage = new Image();
            this.SmallImage.Source = new BitmapImage(new Uri(SecretField.smallImgPath, UriKind.Absolute));
            this.BigImage = new Image();
            this.BigImage.Source = new BitmapImage(new Uri(SecretField.bigImgPath, UriKind.Absolute));
        }

        public override void OpenSecret(Unit target)
        {
            if (target.AttackLevel - 2 <= 0)
            {
                target.AttackLevel = 0;
            }
            else
            {
                target.AttackLevel -= 2;
            }
        }
    }
}