using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace Ivony.Configurations
{

  /// <summary>
  /// 定义配置对象
  /// </summary>
  public class ConfigurationObject : ConfigurationValue, IReadOnlyDictionary<string, ConfigurationValue>
  {


    private JObject _data;
    ConfigurationObject _parent;


    /// <summary>
    /// 创建配置对象
    /// </summary>
    /// <param name="data">配置数据</param>
    /// <param name="parent">用于继承的父级对象</param>
    private ConfigurationObject( JObject data, ConfigurationObject parent )
    {
      _data = (JObject) data.DeepClone();
      _parent = parent;
    }


    /// <summary>
    /// 获取指定配置名称的配置项
    /// </summary>
    /// <param name="name">配置名称</param>
    /// <returns>配置项</returns>
    public override ConfigurationValue this[string name]
    {
      get { return GetValue( name ); }
    }



    private static Regex nameRegex = new Regex( @"^([\w-]+)([\.][\w-]+)*$", RegexOptions.Compiled );


    /// <summary>
    /// 获取指定配置名称的配置项
    /// </summary>
    /// <param name="name">配置名称</param>
    /// <returns>配置项</returns>
    public ConfigurationValue GetValue( string name )
    {
      if ( name == null )
        throw new ArgumentNullException( "name" );

      var fallbackRoot = false;
      if ( name.StartsWith( "." ) )
      {
        fallbackRoot = true;
        name = name.Substring( 1 );
      }


      if ( nameRegex.IsMatch( name ) == false )
        throw new ArgumentException( "must provide a valid name", "name" );




      ConfigurationValue value = null;


      value = GetValueCore( name, fallbackRoot ? this : null );
      if ( value != null )
        return value;


      if ( _parent != null )
        return _parent.GetValue( name );

      return null;
    }




    /// <summary>
    /// 获取配置值
    /// </summary>
    /// <param name="name">配置名称</param>
    /// <param name="candidateFallback">候选的回溯对象</param>
    /// <returns>配置值</returns>
    protected ConfigurationValue GetValueCore( string name, ConfigurationObject candidateFallback = null )
    {

      var parentName = GetParentName( name );
      var value = _data[name] ?? (parentName == null ? _data["*"] : _data[parentName + ".*"]);

      if ( value == null )
      {
        if ( parentName == null )
          return candidateFallback;

        else
          return GetValueCore( parentName, candidateFallback );
      }

      else
        return GetValueCore( value, parentName, candidateFallback );
    }




    /// <summary>
    /// 获取配置值
    /// </summary>
    /// <param name="value">值对象</param>
    /// <param name="parentName">父级名称</param>
    /// <param name="candidateFallback">候选的回溯对象</param>
    /// <returns>配置项</returns>
    private ConfigurationValue GetValueCore( JToken value, string parentName, ConfigurationObject candidateFallback = null )
    {
      if ( value == null )
        throw new ArgumentNullException( "value" );


      var _value = value as JValue;
      if ( _value != null )
        return Create( _value );


      var obj = value as JObject;
      if ( obj != null )
      {
        ConfigurationObject parent = GetInheritObject( parentName );

        return new ConfigurationObject( obj, parent ?? candidateFallback );
      }

      throw new NotSupportedException();
    }



    /// <summary>
    /// 获取继承的对象
    /// </summary>
    /// <param name="parentName">父级名称</param>
    /// <returns>继承的配置对象</returns>
    private ConfigurationObject GetInheritObject( string parentName )
    {
      var parentObject = (parentName == null ? _data["."] : _data[parentName + "."]) as JObject;
      if ( parentObject == null )
        return parentName == null ? null : GetValue( parentName ) as ConfigurationObject;

      parentName = GetParentName( parentName );
      return new ConfigurationObject( parentObject, parentName == null ? null : GetValue( parentName ) as ConfigurationObject );
    }



    /// <summary>
    /// 获取父级名称
    /// </summary>
    /// <param name="name">当前配置名称</param>
    /// <returns>父级名称</returns>
    private string GetParentName( string name )
    {
      if ( name == null )
        return null;

      var index = name.LastIndexOf( '.' );
      if ( index <= 0 )
        return null;

      return name.Remove( index );
    }







    /// <summary>
    /// 从 JObject 对象创建 ConfigurationObject 对象
    /// </summary>
    /// <param name="dataObject">数据对象</param>
    /// <returns>配置对象</returns>
    public static ConfigurationObject Create( JObject dataObject )
    {
      return new ConfigurationObject( dataObject, null );
    }





    /// <summary>
    /// 以 JSON 格式输出配置对象
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return _data.ToString();
    }



    IEnumerable<string> IReadOnlyDictionary<string, ConfigurationValue>.Keys
    {
      get
      {
        return _data.Properties().Select( item => item.Name );
      }
    }

    IEnumerable<ConfigurationValue> IReadOnlyDictionary<string, ConfigurationValue>.Values
    {
      get
      {
        return _data.Properties().Select( item => GetValueCore( item.Value, GetParentName( item.Name ) ) );

      }
    }

    int IReadOnlyCollection<KeyValuePair<string, ConfigurationValue>>.Count
    {
      get
      {
        return _data.Properties().Count();
      }
    }




    bool IReadOnlyDictionary<string, ConfigurationValue>.ContainsKey( string key )
    {
      return _data.Property( key ) != null;
    }

    bool IReadOnlyDictionary<string, ConfigurationValue>.TryGetValue( string key, out ConfigurationValue value )
    {
      var property = _data.Property( key );
      if ( property != null )
      {
        value = GetValueCore( property.Value, GetParentName( property.Name ) );
        return true;
      }

      else
      {
        value = null;
        return false;
      }
    }

    IEnumerator<KeyValuePair<string, ConfigurationValue>> IEnumerable<KeyValuePair<string, ConfigurationValue>>.GetEnumerator()
    {
      return _data.Properties().Select( item => new KeyValuePair<string, ConfigurationValue>( item.Name, GetValueCore( item.Value, GetParentName( item.Name ) ) ) ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<string, ConfigurationValue>>) this).GetEnumerator();
    }




  }
}
