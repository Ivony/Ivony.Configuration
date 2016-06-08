using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class NullValue : ConfigurationValue
  {

    public override object TryConvert( Type type )
    {
      if ( type.IsValueType == false || type.GetGenericTypeDefinition() == typeof( Nullable<> ) )
        return null;

      else
        throw new InvalidCastException();
    }
  }
}