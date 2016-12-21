using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Configurations
{

  /// <summary>
  /// 指示内置的配置文件名的特性
  /// </summary>
  [AttributeUsage( AttributeTargets.Assembly )]
  public class BuiltInConfigurationAttribute : Attribute
  {


    /// <summary>
    /// 创建 BuiltInConfigurationAttribute 对象
    /// </summary>
    /// <param name="filename"></param>
    public BuiltInConfigurationAttribute( string filename )
    {

      if ( string.IsNullOrEmpty( filename ) )
        throw new ArgumentNullException( "filename" );

      Filename = filename;
    }

    /// <summary>
    /// 创建 BuiltInConfigurationAttribute 对象
    /// </summary>
    /// <param name="filename">文件名</param>
    /// <param name="section">配置节</param>
    public BuiltInConfigurationAttribute( string filename, string section )
    {
      if ( string.IsNullOrEmpty( filename ) )
        throw new ArgumentNullException( "filename" );

      if ( string.IsNullOrEmpty( section ) )
        throw new ArgumentNullException( "section" );

      Filename = filename;
      Section = section;
    }


    /// <summary>
    /// 嵌入的资源文件名
    /// </summary>
    public string Filename { get; }


    /// <summary>
    /// 要应用到的配置节
    /// </summary>
    public string Section { get; }



    /// <summary>
    /// 加载配置数据
    /// </summary>
    /// <returns>配置数据</returns>
    internal ConfigurationSection LoadConfigurationData( Assembly assembly )
    {


      var section = Section;

      var stream = assembly.GetManifestResourceStream( Filename );
      if ( stream == null )
      {
        var filename = Filename + postfix;
        stream = assembly.GetManifestResourceStream( filename );
        if ( stream == null )
          return null;

        section = section ?? Filename;
      }


      using ( stream )
      {
        var data = (JObject) JObject.ReadFrom( new JsonTextReader( new StreamReader( stream ) ) );
        return new ConfigurationSection( assembly, section ?? "", data );
      }


    }


    private static readonly string postfix = ".configuration.json";


    internal static ConfigurationSection[] LoadDefaultConfigurationData( Assembly assembly )
    {
      var files = from name in assembly.GetManifestResourceNames()
                  where name.EndsWith( postfix )
                  orderby name
                  select name;


      return files.Select( filename =>
      {
        using ( var stream = assembly.GetManifestResourceStream( filename ) )
        {
          var section = filename.Remove( filename.Length - postfix.Length );
          var data = (JObject) JObject.ReadFrom( new JsonTextReader( new StreamReader( stream ) ) );
          return new ConfigurationSection( assembly, section, data );
        }
      } ).ToArray();
    }
  }

  internal class ConfigurationSection
  {

    public ConfigurationSection( Assembly assembly, string section, JObject data )
    {
      Assembly = assembly;
      Section = section;
      Data = data;
    }


    public Assembly Assembly { get; }

    public string Section { get; }

    public JObject Data { get; }
  }
}
