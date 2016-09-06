using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

namespace Ivony.Configurations
{
  internal sealed class BuiltInConfigurationProvider : ConfigurationProvider
  {
    private static readonly string postfix = ".configuration.json";

    public override JObject GetConfigurationData()
    {




      var files = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany( item => item.GetManifestResourceNames().Select( name => new { Assembly = item, Filename = name } ) )
        .Where( item => item.Filename.EndsWith( postfix ) )
        .OrderBy( item => item.Filename )
        .ToArray();



      var result = new JObject();
      var sectionSet = new Dictionary<string, Assembly>();


      foreach ( var item in files )
      {
        using ( var stream = item.Assembly.GetManifestResourceStream( item.Filename ) )
        {
          var data = new JObject();
          var section = item.Filename.Remove( item.Filename.Length - postfix.Length );

          Assembly conflictAssembly;
          if ( sectionSet.TryGetValue( section, out conflictAssembly ) )
            throw new Exception( string.Format( "Configuration section {0} confilict, it's registered by assembly \"{1}\" and \"{2}\"", section, conflictAssembly.FullName, item.Assembly.FullName ) );
          sectionSet.Add( section, item.Assembly );



          foreach ( var key in section.Split( '.' ) )
            data.Add( key, data = new JObject() );

          data.Merge( JObject.ReadFrom( new JsonTextReader( new StreamReader( stream ) ) ) );

          result.Merge( data.Root );
        }
      }

      return result;
    }
  }
}
