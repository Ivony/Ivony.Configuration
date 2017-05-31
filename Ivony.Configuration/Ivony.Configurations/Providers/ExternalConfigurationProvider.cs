using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Web.Hosting;
using System.IO;
using System.Net;

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

      var setting = System.Configuration.ConfigurationManager.AppSettings["configurations"] ?? "";


      var files = new List<string>( setting.Split( new[] { ',' }, StringSplitOptions.RemoveEmptyEntries ) );

      foreach ( var file in files )
      {

        Uri url;
        if ( Uri.TryCreate( file, UriKind.Absolute, out url ) )
        {
          result.Merge( Load( url ) );
        }
        else
        {
          var path = Path.Combine( currentDirectory, file );
          result.Merge( Load( path ) );
        }
      }

      return result;

    }

    private object Load( Uri url )
    {
      return JObject.Parse( new WebClient().DownloadString( url ) );
    }

    private object Load( string path )
    {
      if ( File.Exists( path ) )
        return JObject.Parse( File.ReadAllText( path ) );

      else
        return new JObject();
    }
  }
}
