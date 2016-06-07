using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Ivony.Configurations
{
  [AttributeUsage( AttributeTargets.Assembly, AllowMultiple = false, Inherited = false )]
  public sealed class ConfigurationFileAttribute : Attribute
  {
    public ConfigurationFileAttribute() : this( "" ) { }
    public ConfigurationFileAttribute( string section ) : this( section, "default.configuration.json" ) { }


    public ConfigurationFileAttribute( string section, string filename )
    {

      if ( section == null )
        throw new ArgumentNullException();

      if ( filename == null )
        throw new ArgumentNullException();

      Section = section.Split( new[] { '/' }, StringSplitOptions.RemoveEmptyEntries ).ToArray();
      Filename = filename;
    }



    public string[] Section { get; private set; }

    public string Filename { get; private set; }

  }
}
