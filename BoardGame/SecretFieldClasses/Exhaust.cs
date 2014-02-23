namespace BoardGame.SecretFieldClasses
{
    using BoardGame.UnitClasses;
    
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