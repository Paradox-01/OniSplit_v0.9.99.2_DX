using System;
using System.Collections.Generic;
using System.IO;

namespace Oni.Objects
{
	internal class ObjectLoadContext
	{
		private readonly Func<TemplateTag, string, ObjectLoadContext, InstanceDescriptor> getDescriptor;

		private readonly TextWriter info;

		private readonly Dictionary<string, ObjectClass> classCache;

		private string basePath;

		private string filePath;

		public string BasePath
		{
			get
			{
				return basePath;
			}
			set
			{
				basePath = value;
			}
		}

		public string FilePath
		{
			get
			{
				return filePath;
			}
			set
			{
				filePath = value;
			}
		}

		public ObjectLoadContext(Func<TemplateTag, string, ObjectLoadContext, InstanceDescriptor> getDescriptor, TextWriter info)
		{
			this.getDescriptor = getDescriptor;
			this.info = info;
			classCache = new Dictionary<string, ObjectClass>(StringComparer.Ordinal);
		}

		public T GetClass<T>(TemplateTag tag, string name, Func<InstanceDescriptor, T> reader) where T : ObjectClass, new()
		{
			string key = tag.ToString() + name;
			ObjectClass value;
			if (!classCache.TryGetValue(key, out value))
			{
				InstanceDescriptor instanceDescriptor = getDescriptor(tag, name, this);
				if (instanceDescriptor != null)
				{
					info.WriteLine("Using {0} object class '{1}' from '{2}'", tag, instanceDescriptor.Name, instanceDescriptor.FilePath);
					value = reader(instanceDescriptor);
					value.Name = instanceDescriptor.Name;
				}
				classCache.Add(key, value);
			}
			return (T)value;
		}
	}
}
