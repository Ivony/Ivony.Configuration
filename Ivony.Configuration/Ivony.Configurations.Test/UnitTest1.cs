using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Ivony.Configurations;

[assembly: ConfigurationFile()]
namespace Ivony.Configurations.Test
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void BlankObjectTest()
    {

      var obj = (ConfigurationObject) ConfigurationObject.Create( JObject.Parse( "{}" ) );


    }
    [TestMethod]
    public void WildcardTest()
    {

      var obj = (ConfigurationObject) ConfigurationObject.Create( JObject.Parse( "{ \"*\": \"abc\" }" ) );

      Assert.AreEqual( obj["test"].ToString(), "abc" );

    }


    [TestMethod]
    public void ConfigurationProviders()
    {
      var configuration = Configuration.GetConfigurationData();

      Assert.AreEqual( configuration["Test"].ToString(), "test" );
    }

  }
}
