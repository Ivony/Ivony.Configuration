using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Ivony.Configurations
{
  /// <summary>
  /// 指定程序集内嵌配置文件位置的特性
  /// </summary>
  [AttributeUsage( AttributeTargets.Assembly, AllowMultiple = false, Inherited = false )]
  public sealed class ConfigurationFileAttribute : Attribute
  {
    /// <summary>
    /// 使用默认值创建 ConfigurationFileAttribute 对象
    /// </summary>
    public ConfigurationFileAttribute() : this( "" ) { }

    /// <summary>
    /// 创建 ConfigurationFileAttribute 对象
    /// </summary>
    /// <param name="section">指定内嵌配置文件的根配置节</param>
    public ConfigurationFileAttribute( string section ) : this( section, "default.configuration.json" ) { }

    /// <summary>
    /// 创建 ConfigurationFileAttribute 对象
    /// </summary>
    /// <param name="section">指定内嵌配置文件的根配置节</param>
    /// <param name="filename">配置数据资源文件名称路径</param>
    public ConfigurationFileAttribute( string section, string filename )
    {

      if ( section == null )
        throw new ArgumentNullException();

      if ( filename == null )
        throw new ArgumentNullException();

      Section = section.Split( new[] { '/' }, StringSplitOptions.RemoveEmptyEntries ).ToArray();
      Filename = filename;
    }


    /// <summary>
    /// 指定内嵌配置文件的根配置节
    /// </summary>
    public string[] Section { get; private set; }

    /// <summary>
    /// 指定配置数据资源文件名称路径
    /// </summary>
    public string Filename { get; private set; }

  }
}
