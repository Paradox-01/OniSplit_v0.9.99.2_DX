using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Oni
{
	internal abstract class Exporter
	{
		private readonly InstanceFileManager fileManager;

		private readonly string outputDirPath;

		private readonly Dictionary<string, string> fileNames;

		private Regex nameFilter;

		public InstanceFileManager InstanceFileManager
		{
			get
			{
				return fileManager;
			}
		}

		public string OutputDirPath
		{
			get
			{
				return outputDirPath;
			}
		}

		public Regex NameFilter
		{
			get
			{
				return nameFilter;
			}
			set
			{
				nameFilter = value;
			}
		}

		protected Exporter(InstanceFileManager fileManager, string outputDirPath)
		{
			this.fileManager = fileManager;
			this.outputDirPath = outputDirPath;
			fileNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}

		public void ExportFiles(IEnumerable<string> sourceFilePaths)
		{
			Directory.CreateDirectory(outputDirPath);
			foreach (string sourceFilePath in sourceFilePaths)
			{
				ExportFile(sourceFilePath);
			}
			Flush();
		}

		protected virtual void ExportFile(string sourceFilePath)
		{
			Console.WriteLine(sourceFilePath);
			InstanceFile file = fileManager.OpenFile(sourceFilePath);
			List<InstanceDescriptor> list = GetSupportedDescriptors(file);
			if (nameFilter != null)
			{
				list = list.FindAll((InstanceDescriptor x) => x.HasName && nameFilter.IsMatch(x.FullName));
			}
			foreach (InstanceDescriptor item in list)
			{
				ExportInstance(item);
			}
		}

		protected abstract void ExportInstance(InstanceDescriptor descriptor);

		protected virtual void Flush()
		{
		}

		protected virtual List<InstanceDescriptor> GetSupportedDescriptors(InstanceFile file)
		{
			return file.GetNamedDescriptors();
		}

		protected string CreateFileName(InstanceDescriptor descriptor, string fileExtension)
		{
			string text = Importer.EncodeFileName(descriptor.FullName, fileNames);
			return Path.Combine(outputDirPath, text + fileExtension);
		}
	}
}
