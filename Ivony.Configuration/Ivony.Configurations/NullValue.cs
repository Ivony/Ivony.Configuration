using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class NullValue : ConfigurationValue
  {

    protected override bool TryConvertTo(Type type, out object value)
    {
      value = null;
      if (type.IsValueType == false || type.GetGenericTypeDefinition() == typeof(Nullable<>))
        return true;

      else
        return false;
    }
  }
}