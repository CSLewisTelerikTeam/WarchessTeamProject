using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoardGame.UnitClasses
{
    //Interfaces describe the behavior of the units
    
    interface IMoveable
    {
        //Return true if trip to the destination position is possible
        bool IsMoveable(Point destination);
    }

    interface IAttacking
    {
        void Attack(Unit targetUnit);
    }

    interface IHealing
    {
        void Heal(Unit objectToHeal);
    }
}
