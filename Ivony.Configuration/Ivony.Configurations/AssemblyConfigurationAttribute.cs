using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Configurations
{

  /// <summary>
  /// 定义可以获取内置配置数据的特性
  /// </summary>
  [AttributeUsage( AttributeTargets.Assembly, AllowMultiple = false, Inherited = false )]
  public abstract class AssemblyConfigurationAttribute : Attribute
  {

    /// <summary>
    /// 获取程序集的配置
    /// </summary>
    /// <returns></returns>
    public abstract JObject GetAssemblyConfiguration( Assembly assembly );

  }
}
