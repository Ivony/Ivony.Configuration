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
      _data = (JObject) data.DeepClone();
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




      ConfigurationValue value = null;


      var dataName = name;
      while ( dataName != null )
      {
        value = GetValueCore( dataName );
        if ( value != null )
          return value;

        dataName = GetParentName( dataName );
      }


      if ( _parent != null )
        return _parent.GetValue( name );

      return null;
    }


    protected ConfigurationValue GetValueCore( string name )
    {

      var parentName = GetParentName( (string) name );
      var value = _data[name] ?? (parentName == null ? _data["*"] : _data[parentName + ".*"]);

      return GetValueCore( value, parentName );
    }



    private ConfigurationValue GetValueCore( JToken value, string parentName )
    {
      var _value = value as JValue;
      if ( _value != null )
        return Create( _value );


      var obj = value as JObject;
      if ( obj != null )
      {
        ConfigurationObject parent = GetParent( parentName );

        return new ConfigurationObject( obj, parent );
      }

      return null;
    }



    private ConfigurationObject GetParent( string parentName )
    {
      var parentObject = (parentName == null ? _data["."] : _data[parentName + "."]) as JObject;
      if ( parentObject == null )
        return parentName == null ? null : GetValue( parentName ) as ConfigurationObject;

      parentName = GetParentName( parentName );
      return new ConfigurationObject( parentObject, parentName == null ? null : GetValue( parentName ) as ConfigurationObject );
    }



    private string GetParentName( string name )
    {
      if ( name == null )
        return null;

      var index = name.LastIndexOf( '.' );
      if ( index <= 0 )
        return null;

      return name.Remove( index );
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
        return _data.Properties().Select( item => GetValueCore( item.Value, GetParentName( item.Name ) ) );

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
        value = GetValueCore( property.Value, GetParentName( property.Name ) );
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
      return _data.Properties().Select( item => new KeyValuePair<string, ConfigurationValue>( item.Name, GetValueCore( item.Value, GetParentName( item.Name ) ) ) ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<string, ConfigurationValue>>) this).GetEnumerator();
    }




  }
}
