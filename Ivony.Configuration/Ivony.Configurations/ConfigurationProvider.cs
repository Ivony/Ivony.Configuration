using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Configurations
{
  public abstract class ConfigurationProvider
  {

    public abstract JObject GetConfigurationData();

  }
}
