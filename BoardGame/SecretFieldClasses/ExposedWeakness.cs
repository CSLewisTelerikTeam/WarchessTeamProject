using System;
using System.Linq;
using BoardGame.UnitClasses;

namespace BoardGame.SecretFieldClasses
{
    class ExposedWeakness : HarmfulField
    {
        public ExposedWeakness() : base(SecretFields.ExposedWeakness)
        {
        }
        public override void OpenSecret(Unit target)
        {
            target.AttackLevel -= 2;
        }
    }
}
