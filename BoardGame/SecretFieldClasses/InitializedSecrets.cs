namespace BoardGame.SecretFieldClasses
{
    using System.Collections.Generic;

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