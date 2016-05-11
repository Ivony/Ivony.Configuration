using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Ivony.Configurations
{
  public sealed class AssemblyConfigurationProvider : ConfigurationProvider
  {
    public override JObject GetConfigurationData()
    {
      AppDomain.CurrentDomain.GetAssemblies().SelectMany( item => item.GetCustomAttributes( typeof( ConfigurationFileAttribute ), false ) ).ToArray();

      throw new NotImplementedException();
    }
  }
}
