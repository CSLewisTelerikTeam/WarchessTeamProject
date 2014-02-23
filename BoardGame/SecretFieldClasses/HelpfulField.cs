namespace BoardGame.SecretFieldClasses
{
    abstract class HelpfulField : SecretField
    {
        public HelpfulField(SecretFields secretFieldName) : base(FieldTypes.HelpfulField, secretFieldName)
        {

        }
    }
}