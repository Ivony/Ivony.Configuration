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

    public override JObject GetConfigurationData()
    {




      var list = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                 where assembly.IsDynamic == false
                 let attributes = assembly.GetCustomAttributes<BuiltInConfigurationAttribute>()
                 let result = attributes.Any()
                                ? from item in attributes select item.LoadConfigurationData( assembly )
                                : BuiltInConfigurationAttribute.LoadDefaultConfigurationData( assembly )

                 from dataItem in result
                 select dataItem;



      {

        var result = new JObject();
        var sectionSet = new Dictionary<string, Assembly>();


        foreach ( var item in list )
        {
          if ( item == null )
            continue;

          var data = new JObject();

          Assembly conflictAssembly;
          if ( sectionSet.TryGetValue( item.Section, out conflictAssembly ) )
            throw new Exception( string.Format( "Configuration section {0} confilict, it's registered by assembly \"{1}\" and \"{2}\"", item.Section, conflictAssembly.FullName, item.Assembly.FullName ) );
          sectionSet.Add( item.Section, item.Assembly );


          if ( string.IsNullOrEmpty( item.Section ) )
            data = item.Data;
          else
            data[item.Section] = item.Data;

          result.Merge( data );
        }

        return result;

      }
    }
  }
}