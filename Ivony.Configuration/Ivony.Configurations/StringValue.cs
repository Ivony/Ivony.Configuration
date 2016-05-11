using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class StringValue : ConfigurationValue
  {
    private string value;

    public StringValue( string value )
    {
      this.value = value;
    }

    protected override DynamicMetaObject GetMetaObject( Expression parameter )
    {
      return new DynamicMetaObject( parameter, BindingRestrictions.Empty, value );
    }



    public override string ToString()
    {
      return value;
    }

  }
}