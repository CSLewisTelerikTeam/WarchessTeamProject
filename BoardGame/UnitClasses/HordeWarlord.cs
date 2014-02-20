using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.UnitClasses
{
    class HordeWarlord : RaceHorde
    {
        //Attack & Health start values
        private const int InitialAttackLevel = 35;
        private const int InitialHealthLevel = 70;

        //Unit constructor
        public HordeWarlord()
        {
            this.UnitType = HordeTypeUnits.Warlord;
            this.AttackLevel = InitialAttackLevel;
            this.HealthLevel = InitialHealthLevel;
        }
    }
}
