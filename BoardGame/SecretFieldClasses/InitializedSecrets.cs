using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.SecretFieldClasses
{
    class InitializedSecrets
    {
        public static List<SecretField> Secrets;
 
        static InitializedSecrets()
        {
            InitializedSecrets.Secrets = new List<SecretField> 
            {
                new Fury(),
                new Toughness(),
                new Exhaust(),
                new ExposedWeakness()
            };
        }
    }
}
