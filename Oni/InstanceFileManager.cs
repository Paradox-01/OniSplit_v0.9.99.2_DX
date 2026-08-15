using System;
using System.Collections.Generic;
using System.IO;
using Oni.Collections;

namespace Oni
{
	internal sealed class InstanceFileManager
	{
		private readonly List<string> searchPaths = new List<string>();

		private readonly Dictionary<string, InstanceFile> loadedFiles = new Dictionary<string, InstanceFile>(StringComparer.OrdinalIgnoreCase);

		private Dictionary<string, string> files;

		public InstanceFile OpenFile(string filePath)
		{
			InstanceFile value;
			if (!loadedFiles.TryGetValue(filePath, out value))
			{
				try
				{
					value = InstanceFile.Read(this, filePath);
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("Error opening file {0}: {1}", filePath, ex.Message);
					throw;
				}
				loadedFiles.Add(filePath, value);
			}
			return value;
		}

		public List<InstanceFile> OpenDirectories(string[] dirPaths)
		{
			List<InstanceFile> list = new List<InstanceFile>();
			Set<string> set = new Set<string>(StringComparer.Ordinal);
			Array.Reverse(dirPaths);
			foreach (string dirPath in dirPaths)
			{
				List<string> list2 = FindFiles(dirPath);
				foreach (string item in list2)
				{
					if (!set.Contains(Path.GetFileName(item)))
					{
						list.Add(OpenFile(item));
					}
				}
				foreach (string item2 in list2)
				{
					set.Add(Path.GetFileName(item2));
				}
			}
			return list;
		}

		public List<InstanceFile> OpenDirectory(string dirPath)
		{
			List<InstanceFile> list = new List<InstanceFile>();
			foreach (string item in FindFiles(dirPath))
			{
				list.Add(OpenFile(item));
			}
			return list;
		}

		public void AddSearchPath(string path)
		{
			if (Directory.Exists(path))
			{
				searchPaths.Add(path);
			}
		}

		public InstanceFile FindInstance(string instanceName)
		{
			if (files == null)
			{
				files = new Dictionary<string, string>(StringComparer.Ordinal);
				foreach (string searchPath in searchPaths)
				{
					foreach (string item in FindFiles(searchPath))
					{
						string key = Importer.DecodeFileName(item);
						if (!files.ContainsKey(key))
						{
							files[key] = item;
						}
					}
				}
			}
			string value;
			if (!files.TryGetValue(instanceName, out value))
			{
				return null;
			}
			InstanceFile instanceFile = OpenFile(value);
			if (instanceFile == null)
			{
				return null;
			}
			InstanceDescriptor instanceDescriptor = instanceFile.Descriptors[0];
			if (instanceFile.Header.Version != 1448227634)
			{
				return null;
			}
			if (instanceDescriptor == null || !instanceDescriptor.HasName || instanceDescriptor.FullName != instanceName)
			{
				return null;
			}
			return instanceFile;
		}

		public InstanceFile FindInstance(string instanceName, InstanceFile baseFile)
		{
			if (files == null)
			{
				files = new Dictionary<string, string>(StringComparer.Ordinal);
				foreach (string item in FindFiles(Path.GetDirectoryName(baseFile.FilePath)))
				{
					files[Importer.DecodeFileName(item)] = item;
				}
				foreach (string searchPath in searchPaths)
				{
					foreach (string item2 in FindFiles(searchPath))
					{
						string key = Importer.DecodeFileName(item2);
						if (!files.ContainsKey(key))
						{
							files[key] = item2;
						}
					}
				}
			}
			string value;
			if (!files.TryGetValue(instanceName, out value))
			{
				if (instanceName.Length > 4)
				{
					instanceName = instanceName.Substring(4);
				}
				if (!files.TryGetValue(instanceName, out value))
				{
					string text = Path.Combine(Path.GetDirectoryName(baseFile.FilePath), "level0_Final.dat");
					if (!File.Exists(text))
					{
						return null;
					}
					return OpenFile(text);
				}
			}
			InstanceFile instanceFile = OpenFile(value);
			if (instanceFile == null || instanceFile == baseFile)
			{
				return null;
			}
			InstanceDescriptor instanceDescriptor = instanceFile.Descriptors[0];
			if (instanceFile.Header.Version == 1448227634)
			{
				return instanceFile;
			}
			if (instanceDescriptor == null || !instanceDescriptor.HasName || instanceDescriptor.FullName != instanceName)
			{
				return null;
			}
			return instanceFile;
		}

		private static List<string> FindFiles(string dirPath)
		{
			List<string> result = new List<string>();
			if (Directory.Exists(dirPath))
			{
				FindFilesRecursive(dirPath, result);
			}
			return result;
		}

		private static void FindFilesRecursive(string dirPath, List<string> files)
		{
			files.AddRange(Directory.GetFiles(dirPath, "*.oni"));
			string[] directories = Directory.GetDirectories(dirPath);
			foreach (string text in directories)
			{
				string fileName = Path.GetFileName(text);
				if (!string.Equals(fileName, "_noimport", StringComparison.OrdinalIgnoreCase) && !string.Equals(fileName, "noimport", StringComparison.OrdinalIgnoreCase))
				{
					FindFilesRecursive(text, files);
				}
			}
		}
	}
}
