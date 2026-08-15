using System;
using System.Collections.Generic;
using Oni.Collections;

namespace Oni
{
	internal sealed class DatPacker
	{
		private readonly List<string> inputPaths = new List<string>();

		private string targetFilePath;

		private bool targetBigEndian;

		private long targetTemplateChecksum;

		public List<string> InputPaths
		{
			get
			{
				return inputPaths;
			}
		}

		public string TargetFilePath
		{
			get
			{
				return targetFilePath;
			}
			set
			{
				targetFilePath = value;
			}
		}

		public bool TargetBigEndian
		{
			get
			{
				return targetBigEndian;
			}
			set
			{
				targetBigEndian = value;
			}
		}

		public long TargetTemplateChecksum
		{
			get
			{
				return targetTemplateChecksum;
			}
			set
			{
				targetTemplateChecksum = value;
			}
		}

		public void Pack(InstanceFileManager fileManager, IEnumerable<string> filePaths)
		{
			List<InstanceFile> list = new List<InstanceFile>();
			Set<string> set = new Set<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string filePath in filePaths)
			{
				if (set.Add(filePath))
				{
					list.Add(fileManager.OpenFile(filePath));
				}
			}
			list.Reverse();
			List<InstanceDescriptor> importedDescriptors = GetImportedDescriptors(list);
			if (importedDescriptors.Count > 0)
			{
				InstanceFileWriter instanceFileWriter = InstanceFileWriter.CreateV31(targetTemplateChecksum, targetBigEndian);
				instanceFileWriter.AddDescriptors(importedDescriptors, true);
				Console.WriteLine("Writing {0}", targetFilePath);
				instanceFileWriter.Write(targetFilePath);
			}
		}

		public void Import(InstanceFileManager fileManager, string[] inputDirPaths)
		{
			Console.WriteLine("Reading files from {0}", string.Join(";", inputDirPaths));
			List<InstanceFile> inputFiles = fileManager.OpenDirectories(inputDirPaths);
			List<InstanceDescriptor> importedDescriptors = GetImportedDescriptors(inputFiles);
			if (importedDescriptors.Count > 0)
			{
				InstanceFileWriter instanceFileWriter = InstanceFileWriter.CreateV31(targetTemplateChecksum, targetBigEndian);
				instanceFileWriter.AddDescriptors(importedDescriptors, true);
				Console.WriteLine("Writing {0}", targetFilePath);
				instanceFileWriter.Write(targetFilePath);
			}
		}

		private static List<InstanceDescriptor> GetImportedDescriptors(List<InstanceFile> inputFiles)
		{
			Set<string> set = new Set<string>(StringComparer.Ordinal);
			Set<InstanceDescriptor> set2 = new Set<InstanceDescriptor>();
			foreach (InstanceFile inputFile in inputFiles)
			{
				foreach (InstanceDescriptor namedDescriptor in inputFile.GetNamedDescriptors())
				{
					if (set.Contains(namedDescriptor.FullName))
					{
						set2.Add(namedDescriptor);
					}
					else
					{
						set.Add(namedDescriptor.FullName);
					}
				}
			}
			inputFiles.Sort((InstanceFile x, InstanceFile y) => string.Compare(x.Descriptors[0].FullName, y.Descriptors[0].FullName, StringComparison.Ordinal));
			List<InstanceDescriptor> list = new List<InstanceDescriptor>(4096);
			foreach (InstanceFile inputFile2 in inputFiles)
			{
				foreach (InstanceDescriptor descriptor in inputFile2.Descriptors)
				{
					if (set2.Contains(descriptor))
					{
						continue;
					}
					if (descriptor.HasName)
					{
						if (descriptor.IsPlaceholder && set.Contains(descriptor.FullName))
						{
							continue;
						}
						set.Add(descriptor.FullName);
					}
					list.Add(descriptor);
				}
			}
			list.Sort((InstanceDescriptor x, InstanceDescriptor y) => x.Template.IsLeaf.CompareTo(y.Template.IsLeaf));
			return list;
		}
	}
}
