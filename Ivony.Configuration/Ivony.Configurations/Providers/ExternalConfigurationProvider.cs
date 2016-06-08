using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Web.Hosting;
using System.IO;

namespace Ivony.Configurations
{
  internal class ExternalConfigurationProvider : ConfigurationProvider
  {
    public override JObject GetConfigurationData()
    {



      var currentDirectory = (string) null;
      if ( HostingEnvironment.IsHosted )
        currentDirectory = HostingEnvironment.ApplicationPhysicalPath;

      else
        currentDirectory = AppDomain.CurrentDomain.BaseDirectory;


      var result = new JObject();

      var setting = ConfigurationManager.AppSettings["configurations"] ?? "";
      foreach ( var file in setting.Split( new[] { ',' }, StringSplitOptions.RemoveEmptyEntries ) )
      {
        var path = Path.Combine( currentDirectory, file );
        result.Merge( JObject.Parse( File.ReadAllText( path ) ) );
      }

      return result;

    }
  }
}
