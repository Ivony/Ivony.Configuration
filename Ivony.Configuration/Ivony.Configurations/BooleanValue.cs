using System;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class BooleanValue : ConfigurationValue
  {
    public bool Value { get; }

    public BooleanValue(bool value)
    {
      Value = value;
    }




    public override string ToString()
    {
      return Value.ToString(CultureInfo.InvariantCulture);
    }


    protected override bool TryConvertTo(Type type, out object value)
    {
      if (type == typeof(bool))
      {
        value = Value;
        return true;
      }
      else
      {
        value = null;
        return false;
      }
    }

  }
}