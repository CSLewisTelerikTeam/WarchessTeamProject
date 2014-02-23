namespace BoardGame
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public static class ConvertPixelsToRowCol
    {
        //GeDynamicRowCol is to get the row and col after first move
        public static void GetDynamicRowColumn(this Grid @this, Point mousePos, Point unitPos, out Point newPoint)
        {
            int unitX = Convert.ToInt32(unitPos.X);
            int unitY = Convert.ToInt32(unitPos.Y);

            newPoint = new Point();
            
            double total;
            
            if (mousePos.X >= 0)
            {
                newPoint.X = -1;
                total = 0;

                for (int i = unitX; i < 8; i++)
                {
                    ColumnDefinition clm = @this.ColumnDefinitions[i];

                    if (mousePos.X < total)
                    {
                        break;
                    }

                    newPoint.X++;
                    total += clm.ActualWidth;
                }
            }
            else
            {
                newPoint.X = 0;
                total = unitX;

                for (int i = unitX; i >= 0; i--)
                {
                    ColumnDefinition clm = @this.ColumnDefinitions[i];

                    if (mousePos.X >= total)
                    {
                        break;
                    }

                    newPoint.X--;
                    total -= clm.ActualWidth;
                }
            }

            if (mousePos.Y >= 0)
            {
                newPoint.Y = -1;
                total = 0;

                for (int i = unitY; i < 8; i++)
                {
                    RowDefinition rowDef = @this.RowDefinitions[i];

                    if (mousePos.Y < total)
                    {
                        break;
                    }

                    newPoint.Y++;
                    total += rowDef.ActualHeight;
                }
            }
            else
            {
                newPoint.Y = 0;
                total = unitY;

                for (int i = unitY; i >= 0; i--)
                {
                    RowDefinition rowDef = @this.RowDefinitions[i];

                    if (mousePos.Y >= total)
                    {
                        break;
                    }

                    newPoint.Y--;
                    total -= rowDef.ActualHeight;
                }
            }
        }

        //GetRowCol is to get the current row and col
        public static void GetRowColumn(this Grid @this, Point mousePos, out Point newPoint)
        {
            newPoint = new Point();
            newPoint.X = -1;

            double total = 0;
            
            foreach (ColumnDefinition clm in @this.ColumnDefinitions)
            {
                if (mousePos.X < total)
                {
                    break;
                }

                newPoint.X++;
                total += clm.ActualWidth;
            }

            newPoint.Y = -1;
            total = 0;

            foreach (RowDefinition rowDef in @this.RowDefinitions)
            {
                if (mousePos.Y < total)
                {
                    break;
                }
                
                newPoint.Y++;
                total += rowDef.ActualHeight;
            }
        }
    }
}