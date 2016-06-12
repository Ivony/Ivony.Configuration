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
    ConfigurationObject _parent;


    internal ConfigurationObject( JObject data, ConfigurationObject parent )
    {
      _data = data;
      _parent = parent;
    }


    public override ConfigurationValue this[string name]
    {
      get { return GetValue( name ); }
    }



    private static Regex nameRegex = new Regex( @"^([\w-]+)([\.][\w-]+)*$", RegexOptions.Compiled );


    public ConfigurationValue GetValue( string name )
    {
      if ( name == null )
        throw new ArgumentNullException( "name" );

      if ( nameRegex.IsMatch( name ) == false )
        throw new ArgumentException( "must provide a valid name", "name" );


      {
        var value = GetValueCore( name );
        if ( value != null )
          return value;
      }


      {
        var parentName = GetParentName( name );
        while ( parentName != null )
        {
          var value = GetValueCore( parentName );
          if ( value != null )
            return value;

          parentName = GetParentName( parentName );
        }
      }


      if ( _parent != null )
        return _parent.GetValue( name );

      return null;
    }

    protected ConfigurationValue GetValueCore( string name )
    {
      JToken value;
      if ( _data.TryGetValue( name, out value ) )
        return CreateValue( name, value );

      if ( _data.TryGetValue( "*", out value ) )
        return CreateValue( name, value );

      else
        return null;
    }



    private string GetParentName( string name )
    {
      var index = name.LastIndexOf( '.' );
      if ( index <= 0 )
        return null;

      return name.Remove( index );
    }


    private ConfigurationValue CreateValue( string name, JToken value )
    {

      var obj = value as JObject;

      if ( obj != null )
      {
        var parentName = GetParentName( name );
        if ( parentName != null )
          return new ConfigurationObject( obj, GetValue( parentName ) as ConfigurationObject );

        else
          return new ConfigurationObject( obj, null );
      }



      return ConfigurationValue.Create( value as JValue );
    }





    public static ConfigurationObject Create( JObject dataObject )
    {
      return new ConfigurationObject( dataObject, null );
    }





    public override string ToString()
    {
      return _data.ToString();
    }



    IEnumerable<string> IReadOnlyDictionary<string, ConfigurationValue>.Keys
    {
      get
      {
        return _data.Properties().Select( item => item.Name );
      }
    }

    IEnumerable<ConfigurationValue> IReadOnlyDictionary<string, ConfigurationValue>.Values
    {
      get
      {
        return _data.Properties().Select( item => CreateValue( item.Name, item.Value ) );

      }
    }

    int IReadOnlyCollection<KeyValuePair<string, ConfigurationValue>>.Count
    {
      get
      {
        return _data.Properties().Count();
      }
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
        value = CreateValue( property.Name, property.Value );
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
      return _data.Properties().Select( item => new KeyValuePair<string, ConfigurationValue>( item.Name, CreateValue( item.Name, item.Value ) ) ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<string, ConfigurationValue>>) this).GetEnumerator();
    }




  }
}
