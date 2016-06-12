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

      {
        var obj = (ConfigurationObject) ConfigurationObject.Create( JObject.Parse( "{ \"global\": { \"*\": { \"abc\": \"test\" } } }" ) );
        Assert.AreEqual( (string) obj["global"]["test"]["abc"], "test" );
      }

    }


    [TestMethod]
    public void NullableTest()
    {

      var obj = ConfigurationObject.Create( JObject.Parse( "{ \"test1\": false }" ) );

      Assert.AreEqual( (bool?) obj["test1"], false );
      Assert.AreEqual( (bool?) obj["test2"], null );

    }

    [TestMethod]
    public void ValueTypeTest()
    {

      var obj = ConfigurationObject.Create( JObject.Parse( "{ \"test1\": false }" ) );

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


    [TestMethod]
    public void InheritTest()
    {
      {

        var obj = (ConfigurationObject) ConfigurationObject.Create( JObject.Parse( "{ \"global\":{ \"test\": 1 } }" ) );
        Assert.AreEqual( (int) obj["global.A"]["test"], 1 );
      }

      {
        var obj = (ConfigurationObject) ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1 }, \"global.A\":{} }" ) );
        Assert.AreEqual( (int) obj["global.A"]["test"], 1 );
      }

      {
        var obj = (ConfigurationObject) ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1 }, \"global.A\":{ \"test\": 2} }" ) );
        Assert.AreEqual( (int) obj["global.A"]["test"], 2 );
      }

    }


    [TestMethod]
    public void InheritTest2()
    {
      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{ \"global\":{ \"test\": 1 } }" ) );
        Assert.AreEqual( (int) obj["global.A"]["test.A"], 1 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1 }, \"global.A\":{} }" ) );
        Assert.AreEqual( (int) obj["global.A"]["test.A"], 1 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1, \"test.A\": 2 }, \"global.A\":{} }" ) );
        Assert.AreEqual( (int) obj["global.A"]["test.A"], 2 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1 }, \"global.A\":{ \"test\": 2} }" ) );
        Assert.AreEqual( (int) obj["global.A"]["test.A"], 2 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1, \"test.A\": 3 }, \"global.A\":{ \"test\": 2} }" ) );
        Assert.AreEqual( (int) obj["global.A"]["test.A"], 2 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1, \"test.A\": 3 }, \"global.A\":{} }" ) );
        Assert.AreEqual( (int) obj["global.A"]["test.A"], 3 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1, \"test.A\": 3 }, \"global.A\":{ \"test\": 2, \"test.A\": 4 } }" ) );
        Assert.AreEqual( (int) obj["global.A"]["test.A"], 4 );
      }

    }


    [TestMethod]
    public void InheritTest3()
    {
      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1 }, \"global.*\":{} }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], 1 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1 }, \"global.*\":{ \"test.*\": 2 } }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], 2 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test.A\": 1 }, \"global.A\":{ \"test.*\": 3 } }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], 3 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test.A\": 1 }, \"global.*\":{ \"test.A\": 2 }, \"global.A\":{ \"test.*\": 3 } }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], 3 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test.A\": 1 }, \"global.*\":{ \"test.A\": 2 }, \"global.A\":{ \"test.A\": 3, \"*\": 4 } }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], 3 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test.A\": 1 }, \"global.*\":{ \"test.A\": 2 }, \"global.A\":{ \"*\": 4 } }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], 4 );
      }

      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"*\": 5 }, \"global.*\":{ \"test\": 2 }, \"global.A\":{} }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], 5 );
      }
    }



    [TestMethod]
    public void InheritTest4()
    {
      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1 }, \"global.\":{ \"test\": 2 }, \"global.A\":{} }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], 2 );
      }
      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1 }, \"global.\":{}, \"global.A\":{} }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], null );
      }
      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1 }, \"global.\":{}, \"global.*\":{\"test\": 3} }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], 3 );
      }
      {
        var obj = ConfigurationObject.Create( JObject.Parse( "{  \"global\":{ \"test\": 1 }, \"global.\":{ \"test\": 2 } }" ) );
        Assert.AreEqual( (int?) obj["global.A"]["test.A"], 1 );
      }
    }
  }
}
