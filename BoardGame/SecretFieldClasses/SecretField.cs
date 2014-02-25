namespace BoardGame.SecretFieldClasses
{
    using System.Windows.Media;
    using BoardGame.UnitClasses;
    using OOPGameWoWChess;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    
    public abstract class SecretField : ISecret
    {
        public FieldTypes Type { get; set; }

        public SecretFields SecretFieldName { get; set; }

        private MediaPlayer playSound = new MediaPlayer();

        public Point CurrentPosition { get; set; }

        public static string smallImgPath = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\secret_field_small.png");
        public static string bigImgPath = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\secret_field_big.png");
        
        public Image SmallImage { get; set; }

        public Image BigImage { get; set; }

        //Constructors
        public SecretField(FieldTypes type, SecretFields secretFieldName)
        {
            this.Type = type;
            this.SecretFieldName = secretFieldName;
        }
        
        //Methods
        public static SecretField GenerateSecret()
        {
            Random rnd = new Random();
            SecretField randSecret = GetRandomSecret();

            int col = rnd.Next(7);
            int row = rnd.Next(2, 6);

            Border brd = new Border();
            brd.Child = new Image();
            (brd.Child as Image).Source = new BitmapImage(new Uri(smallImgPath, UriKind.Absolute));

            while (true)
            {
                Unit randomUnit = MainWindow.GetUnitOnPosition(new Point(col, row));

                if (randomUnit == null)
                {
                    Grid.SetRow(brd, row);
                    Grid.SetColumn(brd, col);

                    randSecret.CurrentPosition = new Point(col, row);
                    return randSecret;
                }

                col = rnd.Next(7);
                row = rnd.Next(2, 6);
            }
        }
  
        public static SecretField GetRandomSecret()
        {
            Random rnd = new Random();
            int indexOfSecret = rnd.Next(InitializedSecrets.Secrets.Count);
            
            return InitializedSecrets.Secrets[indexOfSecret];
        }

        public abstract void OpenSecret(Unit target);

        public void RevealSound()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Unit_Sounds\Secret\secret_reveal.mp3");
            playSound.Open(new Uri(path));
            playSound.Play();
        }
    }
}