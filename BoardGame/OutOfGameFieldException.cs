namespace BoardGame
{
    using System;

    public class OutOfGameFieldException<T> : ApplicationException
    {
        //private T minRow;

        //private T maxRow;

        //private T minCol;

        //private T maxCol;

        public OutOfGameFieldException(T minRow, T maxRow, T minCol, T maxCol)
        {
            this.MinRow = minRow;
            this.MaxRow = maxRow;
            this.MinCol = minCol;
            this.MaxCol = maxCol;
        }

        public T MinRow { get; private set; }

        public T MaxRow { get; private set; }

        public T MinCol { get; private set; }

        public T MaxCol { get; private set; }

        public override string Message
        {
            get
            {
                string message = "You cannot move outside the game field";
                return message;
            }
        }
    }
}