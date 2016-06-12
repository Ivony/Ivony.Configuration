using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace Ivony.Configurations
{
  public class ConfigurationObject : ConfigurationValue, IReadOnlyDictionary<string, ConfigurationValue>
  {


    private JObject _data;



    internal ConfigurationObject( JObject data )
    {
      _data = data;
    }


    public override ConfigurationValue this[string name]
    {
      get { return ConfigurationValue.Create( GetValueCore( name ) ); }
    }



    private static Regex nameRegex = new Regex( @"^([a-zA-Z0-9]+)([\.][a-zA-Z0-9]+)*$", RegexOptions.Compiled );

    IEnumerable<string> IReadOnlyDictionary<string, ConfigurationValue>.Keys
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    IEnumerable<ConfigurationValue> IReadOnlyDictionary<string, ConfigurationValue>.Values
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    int IReadOnlyCollection<KeyValuePair<string, ConfigurationValue>>.Count
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    ConfigurationValue IReadOnlyDictionary<string, ConfigurationValue>.this[string key]
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public ConfigurationValue GetValue( string name )
    {
      if ( name == null )
        throw new ArgumentNullException( "name" );

      if ( nameRegex.IsMatch( name ) == false )
        throw new ArgumentException( "must provide a valid name", "name" );

      return ConfigurationValue.Create( GetValueCore( name ) );

    }

    protected JToken GetValueCore( string name )
    {
      JToken value;
      if ( _data.TryGetValue( name, out value ) )
        return value;

      value = FallbackGetValue( name );
      if ( value == null )
        return null;

      else
        return value;
    }

    protected JToken FallbackGetValue( string name )
    {

      JToken value;
      if ( _data.TryGetValue( "*", out value ) )
        return value;


      var parentName = GetParentName( name );
      if ( parentName == null )
        return null;

      if ( _data.TryGetValue( parentName + ".*", out value ) )
        return value;


      else
        return GetValueCore( parentName );
    }


    protected string GetParentName( string name )
    {
      var index = name.LastIndexOf( '.' );
      if ( index <= 0 )
        return null;

      return name.Remove( index );

    }

    bool IReadOnlyDictionary<string, ConfigurationValue>.ContainsKey( string key )
    {
      return _data.Property( key ) != null;
    }

    bool IReadOnlyDictionary<string, ConfigurationValue>.TryGetValue( string key, out ConfigurationValue value )
    {
      var property = _data.Property( key );
      if ( property != null )
      {
        value = ConfigurationValue.Create( property.Value );
        return true;
      }

      else
      {
        value = null;
        return false;
      }
    }

    IEnumerator<KeyValuePair<string, ConfigurationValue>> IEnumerable<KeyValuePair<string, ConfigurationValue>>.GetEnumerator()
    {
      return _data.Properties().Select( item => new KeyValuePair<string, ConfigurationValue>( item.Name, ConfigurationValue.Create( item.Value ) ) ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<string, ConfigurationValue>>) this).GetEnumerator();
    }
  }
}
