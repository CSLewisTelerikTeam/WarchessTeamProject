using System;
using System.Linq;
using System.Windows;

namespace BoardGame.UnitClasses
{
    abstract class RaceHorde : Unit
    {
        public RaceHorde(UnitTypes type, int initialHealthLevel, int initialAttackLevel,
            int level, bool isAlive, Point currentPosition, bool isSelected) 
            : base(type, RaceTypes.Horde, initialHealthLevel, initialAttackLevel,
            level, isAlive, currentPosition, isSelected)
        {
        }

        //InflictDamage takes two parameters: attacked unit (of race opposite to that of the attacker)
        //and damage source (attack, counterattack, spells...) 
        //public void InflictDamage(RaceAlliance attackedUnit, double damageSource)
        //{
        //    attackedUnit.HealthLevel -= damageSource;
        //}
    }
}
