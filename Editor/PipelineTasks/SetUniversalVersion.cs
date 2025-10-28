namespace Flexy.AssetRefs.Editor.PipelineTasks;

public class SetUniversalVersion : IPipelineTask
{
	public void Run( Pipeline ppln, Context ctx )
	{
		var utcTime = DateTime.UtcNow;
		var versionString = $"{utcTime.Year-2000}.{utcTime.Month:00}{utcTime.Day:00}.{utcTime.Hour:00}{Mathf.RoundToInt(utcTime.Minute / 6f)}";

		Debug.Log( $"[SetUniversalVersion] - Setting universal version {versionString}" );

		PlayerSettings.bundleVersion				= versionString;
		
		PlayerSettings.Android.bundleVersionCode	= Int32.Parse(versionString.Replace(".", ""));
		PlayerSettings.macOS.buildNumber			= versionString;
		PlayerSettings.tvOS.buildNumber				= versionString;
		PlayerSettings.iOS.buildNumber				= versionString;
	}
}