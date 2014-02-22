using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoardGame.SecretFieldClasses
{
    abstract class HelpfulField : SecretField
    {
        public HelpfulField(SecretFields secretFieldName) 
            : base(FieldTypes.HelpfulField, secretFieldName)
        {
        }
    }
}
