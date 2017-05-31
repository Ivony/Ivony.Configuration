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

  /// <summary>
  /// 定义配置值
  /// </summary>
  public abstract class ConfigurationValue : IDynamicMetaObjectProvider
  {


    internal static ConfigurationValue Create(JValue value)
    {


      if (value == null)
        return null;

      switch (value.Type)
      {


        case JTokenType.Integer:
        case JTokenType.Float:
        case JTokenType.Bytes:
          return new NumberValue(value.Value<decimal>());

        case JTokenType.String:
          return new StringValue(value.Value<string>());

        case JTokenType.Guid:
        case JTokenType.Date:
        case JTokenType.TimeSpan:
        case JTokenType.Uri:
        case JTokenType.Raw:
          return new StringValue(value.Value<string>());

        case JTokenType.Boolean:
          return new BooleanValue(value.Value<bool>());

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




    DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
    {
      return new DynamicProxy(parameter, this);
    }

    private class DynamicProxy : DynamicMetaObject
    {

      private readonly Type type = typeof(ConfigurationValue);

      public DynamicProxy(Expression expression, ConfigurationValue obj)
        : base(expression, BindingRestrictions.GetTypeRestriction(expression, obj.GetType()), obj) { }



      public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
      {
        var expression = Expression.Call(Expression.Convert(Expression, typeof(ConfigurationValue)), type.GetProperty("Item").GetGetMethod(), Expression.Constant(binder.Name, typeof(string)));
        return new DynamicMetaObject(expression, Restrictions);
      }
    }






    /// <summary>
    /// 通过指定键获取配置值
    /// </summary>
    /// <param name="key">配置键</param>
    /// <returns>配置值</returns>
    public virtual ConfigurationValue this[string key] { get { throw new NotSupportedException(); } }








    /// <summary>
    /// 尝试将配置值转换成指定类型
    /// </summary>
    /// <param name="type">要转换的类型</param>
    /// <param name="value">转换类型后的值</param>
    /// <returns>是否转换成功</returns>
    protected virtual bool TryConvertTo(Type type, out object value)
    {
      value = null;
      return false;
    }


    /// <summary>
    /// 转换配置值类型
    /// </summary>
    /// <param name="type">目标类型</param>
    /// <returns>转换后的配置值</returns>
    private object CastTo(Type type)
    {
      if (TryConvertTo(type, out object value) == false)
        throw new InvalidCastException();

      return value;
    }


    /// <summary>
    /// 转换配置值类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="value">要转换的配置值</param>
    /// <returns>转换后的配置值</returns>
    private static T CastTo<T>(ConfigurationValue value)
    {
      var type = typeof(T);


      if (type.IsValueType)
      {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {

          if (value == null || value is NullValue)
            return default(T);

          else
            return (T)value.CastTo(Nullable.GetUnderlyingType(type));
        }


        if (value == null || value is NullValue)
          throw new InvalidCastException(string.Format("cannot convert null value to type \"{0}\"", type.AssemblyQualifiedName));
      }


      if (value == null || value is NullValue)
        return default(T);


      return (T)value.CastTo(type);
    }



    /// <summary>
    /// 将值显示类型转换成为 string 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator string(ConfigurationValue value)
    {

      return CastTo<string>(value);
    }



    /// <summary>
    /// 将值显示类型转换成为 int 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator int(ConfigurationValue value)
    {
      return CastTo<int>(value);
    }

    /// <summary>
    /// 将值显示类型转换成为 int? 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator int? (ConfigurationValue value)
    {
      return CastTo<int?>(value);
    }



    /// <summary>
    /// 将值显示类型转换成为 decimal 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator decimal(ConfigurationValue value)
    {
      return CastTo<decimal>(value);
    }

    /// <summary>
    /// 将值显示类型转换成为 decimal? 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator decimal? (ConfigurationValue value)
    {
      return CastTo<decimal?>(value);
    }




    /// <summary>
    /// 将值显示类型转换成为 double 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator double(ConfigurationValue value)
    {
      return CastTo<double>(value);
    }

    /// <summary>
    /// 将值显示类型转换成为 double? 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator double? (ConfigurationValue value)
    {
      return CastTo<double?>(value);
    }



    /// <summary>
    /// 将值显示类型转换成为 float 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator float(ConfigurationValue value)
    {
      return CastTo<float>(value);
    }

    /// <summary>
    /// 将值显示类型转换成为 float? 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator float? (ConfigurationValue value)
    {
      return CastTo<float?>(value);
    }



    /// <summary>
    /// 将值显示类型转换成为 long 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator long(ConfigurationValue value)
    {
      return CastTo<long>(value);
    }

    /// <summary>
    /// 将值显示类型转换成为 long? 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator long? (ConfigurationValue value)
    {
      return CastTo<long?>(value);
    }



    /// <summary>
    /// 将值显示类型转换成为 bool 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator bool(ConfigurationValue value)
    {
      return CastTo<bool>(value);
    }

    /// <summary>
    /// 将值显示类型转换成为 bool? 类型的运算符
    /// </summary>
    /// <param name="value">要转换的值</param>
    public static explicit operator bool? (ConfigurationValue value)
    {
      return CastTo<bool?>(value);
    }



  }
}
