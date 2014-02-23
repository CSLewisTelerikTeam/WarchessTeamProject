namespace BoardGame.SecretFieldClasses
{
    using BoardGame.UnitClasses;
    
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