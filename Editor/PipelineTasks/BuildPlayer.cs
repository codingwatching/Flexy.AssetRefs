using UnityEditor.Build.Reporting;

namespace Flexy.AssetRefs.Editor.PipelineTasks;

public class BuildPlayer : IPipelineTask
{
	[SerializeField] private String _outputDirectory	= "Builds";
	[SerializeField] private String _buildName			= "MyGame";

	public void Run( Pipeline ppln, Context ctx )
	{
		var fullBuildName	= $"{_buildName} {Application.version}";
		var outoutExtension = "";
		var outputDirectory = _outputDirectory;
        
#if UNITY_STANDALONE_WIN
		outoutExtension = ".exe";
		outputDirectory = Path.Combine(outputDirectory, fullBuildName);
		fullBuildName	= _buildName;
#elif UNITY_STANDALONE_OSX
		outoutExtension = ".app";
		outputDirectory = Path.Combine(outputDirectory, fullBuildName);
		fullBuildName	= _buildName;
#elif UNITY_STANDALONE_LINUX
		outoutExtension = "";
		outputDirectory = Path.Combine(outputDirectory, fullBuildName);
		fullBuildName	= _buildName;
#elif UNITY_IOS
		outoutExtension = ".ipa"; 
#elif UNITY_ANDROID
		outoutExtension = ".apk";
#elif UNITY_WEBGL
		outoutExtension = "";
		outputDirectory = Path.Combine(outputDirectory, fullBuildName);
		fullBuildName	= _buildName;
#endif
        
		var outputPath		= Path.Combine(outputDirectory, fullBuildName);
        outputPath += outoutExtension;
        
		if (!Directory.Exists(outputDirectory))
			Directory.CreateDirectory(outputDirectory);
        
		// Get scenes to build (all enabled scenes in build settings)
		var scenes = GetEnabledScenes();
        
		var buildPlayerOptions = new BuildPlayerOptions
		{
			scenes = scenes,
			locationPathName = outputPath,
			target = EditorUserBuildSettings.activeBuildTarget,
			options = BuildOptions.None
		};
        
		var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		var summary = report.summary;
        
		if (summary.result == BuildResult.Succeeded)
		{
			Debug.Log($"Build succeeded: {summary.totalSize} bytes");
			Debug.Log($"Build location: {outputPath}");
			
			if (!Application.isBatchMode)
				Application.OpenURL( _outputDirectory );
		}
		else if (summary.result == BuildResult.Failed)
		{
			Debug.LogError("Build failed");
		}
	}
	
	private String[] GetEnabledScenes()
	{
		var scenes = EditorBuildSettings.scenes;
		var enabledScenes = new List<String>();
        
		foreach (var scene in scenes)
		{
			if (scene.enabled)
			{
				enabledScenes.Add(scene.path);
			}
		}
        
		return enabledScenes.ToArray();
	}
}