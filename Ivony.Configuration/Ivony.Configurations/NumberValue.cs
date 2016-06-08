using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class NumberValue : ConfigurationValue
  {
    private readonly decimal value;

    public NumberValue( decimal value )
    {
      this.value = value;
    }


    public override string ToString()
    {
      return value.ToString( System.Globalization.CultureInfo.InvariantCulture );
    }






    public override object TryConvert( Type type )
    {
      if ( type == typeof( decimal ) )
        return value;

      else if ( type == typeof( int ) )
        return (int) value;

      else if ( type == typeof( long ) )
        return (long) value;

      else if ( type == typeof( float ) )
        return (float) value;

      else if ( type == typeof( double ) )
        return (double) value;


      else
        throw new InvalidCastException();
    }


  }
}