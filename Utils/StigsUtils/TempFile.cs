// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

namespace StigsUtils {
	/// <summary>
	/// File that is deleted when disposed.
	/// Defaults to create new file in Path.GetTempPath.
	/// </summary>
	public class TempFile : IDisposable
	{
		public FileStream FileStream { get; }
		public TempFile(string dir = "", string? name = null, string? extension = null, FileMode fileMode = FileMode.OpenOrCreate, FileAccess fileAccess = FileAccess.ReadWrite) {
			extension = extension ?? name?.Split('.').LastOrDefault();
			while (true) {
				var now = DateTime.UtcNow;
				TimeStamp = now.ToString(".yyyyMMdd-HHmmss.fff");
				Dir = Path.Combine(Path.GetTempPath(),dir);
				FilePath = Path.Combine(Dir, (name + TimeStamp + extension).Trim('.'));
				if (!File.Exists(FilePath)) break;
				SpinWait.SpinUntil(()=>DateTime.UtcNow-now >= TimeSpan.FromMilliseconds(1));
			}
			if(!Directory.Exists(Dir)) {
				Directory.CreateDirectory(Dir);
				DirWasCreated = true;
			}
			FileStream = File.Open(FilePath, fileMode, fileAccess);
		}
		public bool DirWasCreated { get; set; }
		public string FilePath { get; }
		public string TimeStamp { get; }
		public string Dir { get; }
		public void Dispose() {
			FileStream.Dispose();
			if(File.Exists(FilePath)) File.Delete(FilePath);	
			if(DirWasCreated && !Directory.GetFileSystemEntries(Dir).Any()) Directory.Delete(Dir,true); 
		}
		public override string ToString() => FilePath;
	}
}