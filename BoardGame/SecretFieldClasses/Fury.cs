using System;
using System.Linq;
using BoardGame.UnitClasses;

namespace BoardGame.SecretFieldClasses
{
    class Fury : HelpfulField
    {
        public Fury() : base(SecretFields.Fury)
        {
        }

        public override void OpenSecret(Unit target)
        {
            target.AttackLevel += 2;
        }
    }
}
