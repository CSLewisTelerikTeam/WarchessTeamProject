namespace BoardGame.UnitClasses
{
    using System.Windows;
    
    //Interfaces describe the behavior of the units
    
    interface IMoveable
    {
        //Return true if trip to the destination position is possible
        bool IsCorrectMove(Point destination);

        bool IsClearWay(Point destination);

        bool IsSomeoneAtThisPosition(Point destination);
    }

    interface IAttacking
    {
        void Attack(Unit targetUnit, out bool successAttack);
    }

    interface IHealing
    {
        void Heal(Unit objectToHeal, out bool successHeal);
    }

    interface ISound
    {
        void PlayAttackSound();

        void PlaySelectSound();

        void PlayDieSound();
    }
}