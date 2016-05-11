using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class NumberValue : ConfigurationValue
  {
    private decimal value;

    public NumberValue( decimal value )
    {
      this.value = value;
    }

    protected override DynamicMetaObject GetMetaObject( Expression parameter )
    {
      return new DynamicMetaObject( parameter, BindingRestrictions.Empty, value );
    }


    public override string ToString()
    {
      return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

  }
}