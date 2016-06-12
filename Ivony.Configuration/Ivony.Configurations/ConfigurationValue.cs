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


    internal static ConfigurationValue Create( JValue value )
    {


      if ( value == null )
        return null;

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




    DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject( Expression parameter )
    {
      return new DynamicProxy( parameter, this );
    }

    private class DynamicProxy : DynamicMetaObject
    {

      private readonly Type type = typeof( ConfigurationValue );

      public DynamicProxy( Expression expression, ConfigurationValue obj )
        : base( expression, BindingRestrictions.GetTypeRestriction( expression, obj.GetType() ), obj ) { }



      public override DynamicMetaObject BindGetMember( GetMemberBinder binder )
      {
        var expression = Expression.Call( Expression.Convert( Expression, typeof( ConfigurationValue ) ), type.GetProperty( "Item" ).GetGetMethod(), Expression.Constant( binder.Name, typeof( string ) ) );
        return new DynamicMetaObject( expression, Restrictions );
      }
    }







    public virtual ConfigurationValue this[string key] { get { throw new NotSupportedException(); } }








    public virtual object TryConvert( Type type )
    {
      throw new InvalidCastException();
    }




    private static T CastTo<T>( ConfigurationValue value )
    {
      var type = typeof( T );


      if ( type.IsValueType )
      {
        if ( type.IsGenericType && type.GetGenericTypeDefinition() == typeof( Nullable<> ) )
        {

          if ( value == null || value is NullValue )
            return default( T );

          else
            return (T) value.TryConvert( Nullable.GetUnderlyingType( type ) );
        }


        if ( value == null || value is NullValue )
          throw new InvalidCastException( string.Format( "cannot convert null value to type \"{0}\"", type.AssemblyQualifiedName ) );
      }


      if ( value == null || value is NullValue )
        return default( T );


      return (T) value.TryConvert( type );
    }



    public static explicit operator string( ConfigurationValue value )
    {

      return CastTo<string>( value );
    }



    public static explicit operator int( ConfigurationValue value )
    {
      return CastTo<int>( value );
    }

    public static explicit operator int? ( ConfigurationValue value )
    {
      return CastTo<int?>( value );
    }



    public static explicit operator decimal( ConfigurationValue value )
    {
      return CastTo<decimal>( value );
    }

    public static explicit operator decimal? ( ConfigurationValue value )
    {
      return CastTo<decimal?>( value );
    }




    public static explicit operator double( ConfigurationValue value )
    {
      return CastTo<double>( value );
    }

    public static explicit operator double? ( ConfigurationValue value )
    {
      return CastTo<double?>( value );
    }



    public static explicit operator float( ConfigurationValue value )
    {
      return CastTo<float>( value );
    }

    public static explicit operator float? ( ConfigurationValue value )
    {
      return CastTo<float?>( value );
    }



    public static explicit operator long( ConfigurationValue value )
    {
      return CastTo<long>( value );
    }

    public static explicit operator long? ( ConfigurationValue value )
    {
      return CastTo<long?>( value );
    }



    public static explicit operator bool( ConfigurationValue value )
    {
      return CastTo<bool>( value );
    }

    public static explicit operator bool? ( ConfigurationValue value )
    {
      return CastTo<bool?>( value );
    }



  }
}
