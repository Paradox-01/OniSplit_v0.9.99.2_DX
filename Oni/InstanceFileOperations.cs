using System;
using System.Collections.Generic;
using System.IO;

namespace Oni
{
	internal sealed class InstanceFileOperations
	{
		private InstanceFileManager fileManager;

		private string destinationDir;

		private readonly Dictionary<string, string> fileNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private Dictionary<string, string> referencedFiles;

		private readonly Dictionary<string, string> instances = new Dictionary<string, string>(StringComparer.Ordinal);

		public void Copy(InstanceFileManager fileManager, List<string> sourceFiles, string destinationDir)
		{
			Initialize(fileManager, sourceFiles, destinationDir);
			foreach (KeyValuePair<string, string> referencedFile in referencedFiles)
			{
				if (File.Exists(referencedFile.Value))
				{
					if (!Utils.AreFilesEqual(referencedFile.Key, referencedFile.Value))
					{
						Console.WriteLine("File {0} already exists at destination and it is different. File not copied.", referencedFile.Value);
					}
				}
				else
				{
					File.Copy(referencedFile.Key, referencedFile.Value);
				}
			}
		}

		public void Move(InstanceFileManager fileManager, List<string> sourceFilePaths, string outputDirPath)
		{
			Initialize(fileManager, sourceFilePaths, outputDirPath);
			foreach (KeyValuePair<string, string> referencedFile in referencedFiles)
			{
				if (File.Exists(referencedFile.Value))
				{
					if (Utils.AreFilesEqual(referencedFile.Key, referencedFile.Value))
					{
						File.Delete(referencedFile.Key);
					}
					else
					{
						Console.WriteLine("File {0} already exists at destination and it is different. Source file not moved.", referencedFile.Value);
					}
				}
				else
				{
					File.Move(referencedFile.Key, referencedFile.Value);
				}
			}
		}

		public void MoveOverwrite(InstanceFileManager fileManager, List<string> sourceFilePaths, string outputDirPath)
		{
			Initialize(fileManager, sourceFilePaths, outputDirPath);
			foreach (KeyValuePair<string, string> referencedFile in referencedFiles)
			{
				if (File.Exists(referencedFile.Value))
				{
					File.Delete(referencedFile.Value);
				}
				File.Move(referencedFile.Key, referencedFile.Value);
			}
		}

		public void MoveDelete(InstanceFileManager fileManager, List<string> sourceFilePaths, string outputDirPath)
		{
			Initialize(fileManager, sourceFilePaths, outputDirPath);
			foreach (KeyValuePair<string, string> referencedFile in referencedFiles)
			{
				if (File.Exists(referencedFile.Value))
				{
					File.Delete(referencedFile.Key);
				}
				else
				{
					File.Move(referencedFile.Key, referencedFile.Value);
				}
			}
		}

		public void GetDependencies(InstanceFileManager fileManager, List<string> sourceFilePaths)
		{
			Initialize(fileManager, sourceFilePaths, null);
			foreach (string key in referencedFiles.Keys)
			{
				Console.WriteLine(key);
			}
		}

		private void Initialize(InstanceFileManager fileManager, List<string> inputFiles, string destinationDir)
		{
			this.fileManager = fileManager;
			this.destinationDir = destinationDir;
			referencedFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			if (destinationDir != null)
			{
				if (Directory.Exists(destinationDir))
				{
					string[] files = Directory.GetFiles(destinationDir, "*.oni");
					foreach (string text in files)
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
						string key = Importer.DecodeFileName(text);
						fileNames[fileNameWithoutExtension] = fileNameWithoutExtension;
						instances[key] = text;
					}
				}
				else
				{
					Directory.CreateDirectory(destinationDir);
				}
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
			string text2 = null;
			foreach (string inputFile in inputFiles)
			{
				string directoryName = Path.GetDirectoryName(inputFile);
				if (directoryName != text2)
				{
					text2 = directoryName;
					dictionary.Clear();
					string[] files2 = Directory.GetFiles(directoryName, "*.oni");
					foreach (string text3 in files2)
					{
						dictionary[Importer.DecodeFileName(text3)] = text3;
					}
				}
				GetReferencedFiles(inputFile, dictionary);
			}
		}

		private void GetReferencedFiles(string sourceFile, Dictionary<string, string> sourceFiles)
		{
			AddReferencedFile(sourceFile);
			InstanceFile instanceFile = fileManager.OpenFile(sourceFile);
			foreach (InstanceDescriptor placeholder in instanceFile.GetPlaceholders())
			{
				string value;
				if (sourceFiles.TryGetValue(placeholder.FullName, out value) && !referencedFiles.ContainsKey(value))
				{
					GetReferencedFiles(value, sourceFiles);
				}
			}
		}

		private void AddReferencedFile(string filePath)
		{
			if (!referencedFiles.ContainsKey(filePath))
			{
				string text = Importer.DecodeFileName(filePath);
				string value;
				if (!instances.TryGetValue(text, out value) && destinationDir != null)
				{
					value = Path.Combine(destinationDir, Importer.EncodeFileName(text, fileNames) + ".oni");
				}
				referencedFiles.Add(filePath, value);
			}
		}
	}
}
