namespace BoardGame.SecretFieldClasses
{
    using BoardGame.UnitClasses;
    
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