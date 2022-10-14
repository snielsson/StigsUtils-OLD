// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

namespace StigsUtils {
	/// <summary>
	/// Directory that is deleted when disposed.
	/// Defaults to created new directory in Path.GetTempPath.
	/// </summary>
	public class TempDirectory : IDisposable
	{
		public TempDirectory(string? name = null) {
			while (true) {
				var now = DateTime.UtcNow;
				TimeStamp = now.ToString(".yyyyMMdd-HHmmss.fff");
				Dir = Path.Combine(Path.GetTempPath(), (name + TimeStamp).Trim('.'));
				if (!Directory.Exists(Dir)) break;
				SpinWait.SpinUntil(()=>DateTime.UtcNow-now >= TimeSpan.FromMilliseconds(1));
			}
			if(!Directory.Exists(Dir)) {
				Directory.CreateDirectory(Dir);
			}
		}
		public string TimeStamp { get; }
		public string Dir { get; }
		public void Dispose() {
			if(Directory.Exists(Dir)) Directory.Delete(Dir,true); 
		}
		public override string ToString() => Dir;
	}
}