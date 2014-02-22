using System;
using System.Linq;
using BoardGame.UnitClasses;

namespace BoardGame.SecretFieldClasses
{
    class Exhaust : HarmfulField
    {
        public Exhaust() : base(SecretFields.Exhaust)
        {
        }
        public override void OpenSecret(Unit target)
        {
            target.AttackLevel -= 2;
        }
    }
}
