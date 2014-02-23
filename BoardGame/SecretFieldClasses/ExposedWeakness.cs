namespace BoardGame.SecretFieldClasses
{
    using BoardGame.UnitClasses;

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