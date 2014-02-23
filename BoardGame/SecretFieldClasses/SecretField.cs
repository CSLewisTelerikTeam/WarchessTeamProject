namespace BoardGame.SecretFieldClasses
{
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

        public Point CurrentPosition { get; set; }

        public static string smallImgPath = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\secret_field_small.png");
        public static string bigImgPath = System.IO.Path.GetFullPath(@"..\..\Resources\Other_graphics\secret_field_big.png");

        //private Image smallImage = new Image();
        //private Image bigImage = new Image();

        //public static Image SmallImage 
        //{
        //    get 
        //    { 
        //        this.smallImage.Source = new BitmapImage(new Uri(smallImgPath, UriKind.Absolute));
        //        return this.smallImage;
        //    }
        //    private set;
        //}

        //public static Image BigImage 
        //{
        //    get 
        //    { 
        //        this.bigImage.Source = new BitmapImage(new Uri(bigImgPath, UriKind.Absolute));
        //        return this.bigImage;
        //    }
        //    private set;
        //}

        //Constructors
        public SecretField(FieldTypes type, SecretFields secretFieldName)
        {
            this.Type = type;
            this.SecretFieldName = secretFieldName;
        }
        
        //Methods
        public static void GenerateSecret(MainWindow window, SecretField secret)
        {
            Random rnd = new Random();

            int col = rnd.Next(7);
            int row = rnd.Next(7);
            
            Border brd = window.Secret;
            (brd.Child as Image).Source = new BitmapImage(new Uri(smallImgPath, UriKind.Absolute));

            while (true)
            {
                Unit randomUnit = MainWindow.GetUnitOnPosition(new Point(col, row));
                secret = GetRandomSecret();

                if (randomUnit == null)
                {
                    Grid.SetRow(brd, row);
                    Grid.SetColumn(brd, col);

                    secret.CurrentPosition = new Point(col, row);
                    break;
                }

                col = rnd.Next(7);
                row = rnd.Next(7);
            }
        }
  
        public static SecretField GetRandomSecret()
        {
            Random rnd = new Random();
            int indexOfSecret = rnd.Next(4);

            return InitializedSecrets.Secrets[indexOfSecret];
        }

        public abstract void OpenSecret(Unit target);
    }
}