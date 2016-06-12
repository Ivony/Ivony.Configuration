using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Ivony.Configurations;

//[assembly: ConfigurationFile()]
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

      {
        var obj = (ConfigurationObject) ConfigurationObject.Create( JObject.Parse( "{ \"*\": \"abc\" }" ) );
        Assert.AreEqual( (string) obj["test"], "abc" );
      }

      {
        var obj = (ConfigurationObject) ConfigurationObject.Create( JObject.Parse( "{ \"*\": { \"abc\": \"test\" } }" ) );
        Assert.AreEqual( (string) obj["test"]["abc"], "test" );
      }

    }


    [TestMethod]
    public void NullableTest()
    {

      var obj = (ConfigurationObject) ConfigurationObject.Create( JObject.Parse( "{ \"test1\": false }" ) );

      Assert.AreEqual( (bool?) obj["test1"], false );
      Assert.AreEqual( (bool?) obj["test2"], null );

    }

    [TestMethod]
    public void ValueTypeTest()
    {

      var obj = (ConfigurationObject) ConfigurationObject.Create( JObject.Parse( "{ \"test1\": false }" ) );

      Assert.AreEqual( (bool) obj["test1"], false );
      try
      {
        var a = (bool) obj["test2"];
      }
      catch ( InvalidCastException e )
      {
        return;
      }
      Assert.Fail();
    }


    [TestMethod]
    public void ConfigurationProviders()
    {
      var configuration = Configuration.GetConfigurationData();

      Assert.AreEqual( configuration["Test"].ToString(), "test" );
    }


    [TestMethod]
    public void Dynamic()
    {
      var obj = (dynamic) ConfigurationObject.Create( JObject.Parse( "{ \"*\": \"abc\" }" ) );

      Assert.AreEqual( (string) obj.test, "abc" );
      Assert.AreEqual( (string) obj["test"], "abc" );
    }

  }
}
