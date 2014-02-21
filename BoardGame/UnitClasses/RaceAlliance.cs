using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace BoardGame.UnitClasses
{
    abstract class RaceAlliance : Unit
    {
        public RaceAlliance(UnitTypes type, double healthLevel,
            double attackLevel, double counterAttackLevel, int level, bool isAlive,
            Point currentPosition, bool isSelected)
                : base(type, RaceTypes.Alliance, healthLevel, attackLevel, counterAttackLevel,
            level, isAlive, currentPosition, isSelected)
        {
        }

        //InflictDamage takes two parameters: attacked unit (of race opposite to that of the attacker)
        //and damage source (attack, counterattack, spells...)
        public void InflictDamage(RaceHorde attackedUnit, double damageSource)
        {
            attackedUnit.HealthLevel -= damageSource;
        }
    }
}
