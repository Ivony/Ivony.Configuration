using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class NullValue : ConfigurationValue
  {
    protected override DynamicMetaObject GetMetaObject( Expression parameter )
    {
      return new DynamicMetaObject( parameter, BindingRestrictions.Empty );
    }
  }
}