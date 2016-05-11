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





  }
}
