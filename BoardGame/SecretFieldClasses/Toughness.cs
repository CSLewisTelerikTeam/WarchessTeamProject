using System;
using System.Linq;
using BoardGame.UnitClasses;

namespace BoardGame.SecretFieldClasses
{
    class Toughness : HelpfulField
    {
        public Toughness() : base(SecretFields.Toughness)
        {
        }
        public override void OpenSecret(Unit target)
        {
            target.HealthLevel += 2;
        }
    }
}
