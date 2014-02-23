namespace BoardGame.SecretFieldClasses
{
    abstract class NeutralField : SecretField
    {
        public NeutralField(SecretFields secretFieldName) : base(FieldTypes.NeutralField, secretFieldName)
        {

        }
    }
}