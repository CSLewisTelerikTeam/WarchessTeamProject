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
                new ShoutOfStrength(),
                new BlessingOfWisdom(),
                new CurseOfWeakness(),
                new BaneOfDoom()
            };
        }
    }
}