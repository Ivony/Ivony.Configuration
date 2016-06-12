using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Configurations
{
  public static class Configuration
  {



    private static object _sync = new object();
    private static bool _initialized = false;


    public static void Initialize()
    {
      if ( _initialized )
        return;

      lock ( _sync )
      {
        if ( _initialized )
          return;

        InitializeProviders();
      }
    }

    private static void InitializeProviders()
    {
      providers = new ConfigurationProvider[] { new BuiltInConfigurationProvider(), new ExternalConfigurationProvider() };

    }


    private static ConfigurationProvider[] providers;

    public static ConfigurationObject GetConfigurationData()
    {

      Initialize();

      var result = new JObject();
      foreach ( var item in providers )
      {
        result.Merge( item.GetConfigurationData() );
      }

      return ConfigurationObject.Create( result );
    }
  }
}
