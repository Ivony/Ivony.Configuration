using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  public abstract class ConfigurationValue : IDynamicMetaObjectProvider
  {


    public static ConfigurationValue Create( JToken value )
    {


      if ( value == null )
        return null;


      var obj = value as JObject;
      if ( obj != null )
        return new ConfigurationObject( obj );


      var _value = value as JValue;
      if ( _value != null )
      {
        switch ( value.Type )
        {


          case JTokenType.Integer:
          case JTokenType.Float:
          case JTokenType.Bytes:
            return new NumberValue( value.Value<decimal>() );

          case JTokenType.String:
            return new StringValue( value.Value<string>() );

          case JTokenType.Guid:
          case JTokenType.Date:
          case JTokenType.TimeSpan:
          case JTokenType.Uri:
          case JTokenType.Raw:
            return new StringValue( value.Value<string>() );

          case JTokenType.Boolean:
            return new BooleanValue( value.Value<bool>() );

          case JTokenType.Null:
          case JTokenType.Undefined:
            return new NullValue();



          case JTokenType.None:
          case JTokenType.Object:
          case JTokenType.Array:
          case JTokenType.Constructor:
          case JTokenType.Property:
          case JTokenType.Comment:
          default:
            throw new InvalidOperationException();
        }
      }

      throw new InvalidOperationException();
    }




    DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject( Expression parameter )
    {
      return GetMetaObject( parameter );
    }


    protected abstract DynamicMetaObject GetMetaObject( Expression parameter );




    public virtual ConfigurationValue this[string key] { get { throw new NotSupportedException(); } }







    public static explicit operator string( ConfigurationValue value )
    {
      if ( value == null || value is NullValue )
        return null;

      var stringValue = value as StringValue;
      if ( stringValue == null )
        throw new InvalidCastException();

      return stringValue.ToString();
    }



    public static explicit operator int( ConfigurationValue value )
    {
      var number = value as NumberValue;
      if ( value == null || number == null )
        throw new InvalidCastException();

      return (int) number.Value;
    }

    public static explicit operator int? ( ConfigurationValue value )
    {
      if ( value is NullValue )
        return null;

      return (int) value;
    }



    public static explicit operator decimal( ConfigurationValue value )
    {
      var number = value as NumberValue;
      if ( value == null || number == null )
        throw new InvalidCastException();

      return number.Value;
    }

    public static explicit operator decimal? ( ConfigurationValue value )
    {
      if ( value is NullValue )
        return null;

      return (decimal) value;
    }




    public static explicit operator double( ConfigurationValue value )
    {
      var number = value as NumberValue;
      if ( value == null || number == null )
        throw new InvalidCastException();

      return (double) number.Value;
    }

    public static explicit operator double? ( ConfigurationValue value )
    {
      if ( value is NullValue )
        return null;

      return (double) value;
    }



    public static explicit operator float( ConfigurationValue value )
    {
      var number = value as NumberValue;
      if ( value == null || number == null )
        throw new InvalidCastException();

      return (float) number.Value;
    }

    public static explicit operator float? ( ConfigurationValue value )
    {
      if ( value is NullValue )
        return null;

      return (float) value;
    }



    public static explicit operator long( ConfigurationValue value )
    {
      var number = value as NumberValue;
      if ( number == null )
        throw new InvalidCastException();

      return (long) number.Value;
    }

    public static explicit operator long? ( ConfigurationValue value )
    {
      if ( value == null || value is NullValue )
        return null;

      return (long) value;
    }



    public static explicit operator bool( ConfigurationValue value )
    {
      var boolean = value as BooleanValue;
      if ( boolean == null )
        throw new InvalidCastException();

      return boolean.Value;
    }

    public static explicit operator bool? ( ConfigurationValue value )
    {
      if ( value == null || value is NullValue )
        return null;

      return (bool) value;
    }



  }
}
