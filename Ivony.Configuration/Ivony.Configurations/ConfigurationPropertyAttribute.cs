using System;

namespace Ivony.Configurations
{

  /// <summary>
  /// 将属性绑定到配置项
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
  public class ConfigurationPropertyAttribute : Attribute
  {

    /// <summary>
    /// 创建 ConfigurationPropertyAttribute 对象
    /// </summary>
    /// <param name="name">绑定的配置项名称</param>
    public ConfigurationPropertyAttribute(string name)
    {
      Name = name;
    }

    /// <summary>
    /// 绑定的配置项名称
    /// </summary>
    public string Name { get; set; }
  }
}