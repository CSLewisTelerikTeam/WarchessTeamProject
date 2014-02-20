using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.UnitClasses
{
    abstract class RaceHorde : Unit
    {
        //InflictDamage takes two parameters: attacked unit (of race opposite to that of the attacker)
        //and damage source (attack, counterattack, spells...) 
        public void InflictDamage(RaceAlliance attackedUnit, double damageSource)
        {
            attackedUnit.HealthLevel -= damageSource;
        }
    }
}
