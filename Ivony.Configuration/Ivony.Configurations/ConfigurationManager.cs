using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Configurations
{

  /// <summary>
  /// 提供配置管理的一系列静态方法以及协助获取当前配置
  /// </summary>
  public abstract class ConfigurationManager
  {

    private static object _sync = new object();
    private static bool _initialized = false;


    /// <summary>
    /// 初始化配置
    /// </summary>
    public static void Initialize()
    {
      if ( _initialized )
        return;

      lock ( _sync )
      {
        if ( _initialized )
          return;

        InitializeProviders();
      }
    }

    private static void InitializeProviders()
    {
      providers = new ConfigurationProvider[] { new BuiltInConfigurationProvider(), new ExternalConfigurationProvider() };

    }


    private static ConfigurationProvider[] providers;



    private static Lazy<ConfigurationObject> lazyLoader = new Lazy<ConfigurationObject>( () =>
     {
       Initialize();

       var result = new JObject();
       foreach ( var item in providers )
       {
         result.Merge( item.GetConfigurationData() );
       }

       return ConfigurationObject.Create( result );
     } );

    /// <summary>
    /// 获取当前环境的配置数据
    /// </summary>
    /// <returns>全局的配置数据</returns>
    public static ConfigurationObject GlobalConfiguration
    {
      get { return lazyLoader.Value; }
    }



    /// <summary>
    /// 获取指定命名空间的配置数据
    /// </summary>
    /// <typeparam name="T">用于查找命名空间的类型</typeparam>
    /// <param name="obj">用于查找命名空间的类型实例</param>
    public static ConfigurationObject GetConfiguration<T>( T obj = default( T ) )
    {
      Type type;
      if ( obj == null )
        type = typeof( T );
      else
        type = obj.GetType();

      return GetConfiguration( type.Namespace );
    }


    /// <summary>
    /// 获取指定命名空间的配置数据
    /// </summary>
    public static ConfigurationObject GetConfiguration( string @namespace )
    {
      if ( string.IsNullOrEmpty( @namespace ) )
        return GlobalConfiguration;

      else
        return (ConfigurationObject) GlobalConfiguration["." + @namespace];
    }


    /// <summary>
    /// 获取当前命名空间的配置数据
    /// </summary>
    /// <returns>类型所处命名空间的配置数据</returns>
    public ConfigurationObject Configuration
    {
      get
      {
        return GetConfiguration( GetType().Namespace );
      }
    }

  }
}
