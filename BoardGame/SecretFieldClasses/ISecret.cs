using System;
using System.Linq;

namespace BoardGame.SecretFieldClasses
{
    interface ISecret
    {
        void OpenSecret(UnitClasses.Unit target);
    }
}
