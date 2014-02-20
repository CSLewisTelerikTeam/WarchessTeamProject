using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.UnitClasses
{
    class AllianceSergeant : RaceAlliance
    {
        //Attack & Health start values
        private const int InitialAttackLevel = 35;
        private const int InitialHealthLevel = 80;

        //Unit constructor
        public AllianceSergeant()
        {
            this.UnitType = AllianceTypeUnits.Sergeant;
            this.AttackLevel = InitialAttackLevel;
            this.HealthLevel = InitialHealthLevel;
        }
    }
}
