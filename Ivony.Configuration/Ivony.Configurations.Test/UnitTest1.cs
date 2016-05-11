using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

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
  }
}
