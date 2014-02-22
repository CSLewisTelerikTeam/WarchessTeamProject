using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoardGame.SecretFieldClasses
{
    abstract class HarmfulField : SecretField
    {
        public HarmfulField(SecretFields secretFieldName)
            : base(FieldTypes.HarmfulField, secretFieldName)
        {
        }
    }
}
