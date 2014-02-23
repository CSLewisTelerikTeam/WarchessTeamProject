namespace BoardGame.SecretFieldClasses
{
    abstract class HarmfulField : SecretField
    {
        public HarmfulField(SecretFields secretFieldName) : base(FieldTypes.HarmfulField, secretFieldName)
        {

        }
    }
}