using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Telerik.QuickStart
{
	public class CodeFilesCollectionConverter : CollectionConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object sourceObj)
		{
			var strSourceObj = sourceObj as string;
			if (strSourceObj != null)
			{
				return (strSourceObj).Split(new char[] { ',' });
			}
			return base.ConvertFrom(context, culture, sourceObj);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string[]))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object destinationObj, Type destinationType)
		{
			var destAsStringArray = destinationObj as string[];
			if (destAsStringArray != null)
			{
				string returnValue = String.Join(",", destAsStringArray);
				if (destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo ctor = typeof(string).GetConstructor(new Type[] { typeof(string) });
					if (ctor != null)
					{
						return new InstanceDescriptor(ctor, new object[] { returnValue });
					}
				}
				else if (destinationType == typeof(string))
				{
					return returnValue;
				}
			}
			return base.ConvertTo(context, culture, destinationObj, destinationType);
		}
	}
}