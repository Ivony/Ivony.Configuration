using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class NumberValue : ConfigurationValue
  {
    public decimal Value { get; }

    public NumberValue( decimal value )
    {
      Value = value;
    }

    protected override DynamicMetaObject GetMetaObject( Expression parameter )
    {
      return new DynamicMetaObject( parameter, BindingRestrictions.Empty, Value );
    }


    public override string ToString()
    {
      return Value.ToString( System.Globalization.CultureInfo.InvariantCulture );
    }

  }
}