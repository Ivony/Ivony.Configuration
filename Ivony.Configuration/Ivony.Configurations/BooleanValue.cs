using System;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class BooleanValue : ConfigurationValue
  {
    public bool Value { get; }

    public BooleanValue( bool value )
    {
      Value = value;
    }

    protected override DynamicMetaObject GetMetaObject( Expression parameter )
    {
      return new DynamicMetaObject( parameter, BindingRestrictions.Empty, Value );
    }




    public override string ToString()
    {
      return Value.ToString( CultureInfo.InvariantCulture );
    }

  }
}