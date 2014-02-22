using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoardGame.SecretFieldClasses
{
    abstract class NeutralField : SecretField
    {
        public NeutralField(SecretFields secretFieldName)
            : base(FieldTypes.NeutralField, secretFieldName)
        {
        }
    }
}
