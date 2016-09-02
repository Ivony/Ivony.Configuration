using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Configurations
{

  /// <summary>
  /// 定义配置数据提供程序
  /// </summary>
  public abstract class ConfigurationProvider
  {

    /// <summary>
    /// 获取配置数据
    /// </summary>
    /// <returns></returns>
    public abstract JObject GetConfigurationData();

  }
}
