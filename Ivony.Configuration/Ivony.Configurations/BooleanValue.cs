using System;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class BooleanValue : ConfigurationValue
  {
    private bool value;

    public BooleanValue( bool value )
    {
      this.value = value;
    }

    protected override DynamicMetaObject GetMetaObject( Expression parameter )
    {
      return new DynamicMetaObject( parameter, BindingRestrictions.Empty, value );
    }




    public override string ToString()
    {
      return value.ToString( CultureInfo.InvariantCulture );
    }

  }
}