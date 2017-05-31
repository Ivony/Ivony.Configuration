using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class NumberValue : ConfigurationValue
  {
    private readonly decimal value;

    public NumberValue(decimal value)
    {
      this.value = value;
    }


    public override string ToString()
    {
      return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }






    protected override bool TryConvertTo(Type type, out object value)
    {
      if (type == typeof(decimal))
      {
        value = this.value;
        return true;
      }

      else if (type == typeof(int))
      {
        value = (int)this.value;
        return true;
      }

      else if (type == typeof(long))
      {
        value = (long)this.value;
        return true;
      }

      else if (type == typeof(float))
      {
        value = (float)this.value;
        return true;
      }

      else if (type == typeof(double))
      {
        value = (double)this.value;
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