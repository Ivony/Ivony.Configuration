using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Ivony.Configurations
{
  internal class StringValue : ConfigurationValue
  {
    private string value;

    public StringValue(string value)
    {
      this.value = value;
    }


    protected class DynamicValueProxy : DynamicMetaObject
    {
      public DynamicValueProxy(Expression expression)
        : base(expression, BindingRestrictions.GetTypeRestriction(expression, typeof(ConfigurationValue))) { }
    }


    public override string ToString()
    {
      return value;
    }


    protected override bool TryConvertTo(Type type, out object value)
    {
      if (type == typeof(string))
      {
        value = this.value;
        return true;
      }

      else
      {
        value = null;
        return false;
      }
    }
  }
}