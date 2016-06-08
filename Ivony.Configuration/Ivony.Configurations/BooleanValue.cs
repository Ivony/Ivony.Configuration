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




    public override string ToString()
    {
      return Value.ToString( CultureInfo.InvariantCulture );
    }


    public override object TryConvert( Type type )
    {
      if ( type == typeof( bool ) )
        return Value;

      else
        throw new InvalidCastException();
    }

  }
}