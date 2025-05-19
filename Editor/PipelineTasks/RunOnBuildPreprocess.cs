using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Flexy.AssetRefs.Editor.PipelineTasks;

public class RunOnBuildPreprocess : IPipelineTask, IPreprocessBuildWithReport
{
	public Int32 callbackOrder { get; }
	public void OnPreprocessBuild( BuildReport report )
	{
		var scenesOld = EditorBuildSettings.scenes;
		FindAndRunAssetPipeline();
		
		var scenesNew = EditorBuildSettings.scenes;
		if (scenesNew.Length != scenesOld.Length) 
			throw new BuildFailedException("Scene list can not be changed from inside build");
		
		for (var i = 0; i < scenesNew.Length; i++)
		{
			if (scenesNew[i].path.Equals(scenesOld[i].path) && scenesNew[i].enabled == scenesOld[i].enabled)
				continue;
				
			throw new BuildFailedException("Scene list can not be changed from inside build");
		}
	}
	
	public static void FindAndRunAssetPipeline( )
	{
		var guids = AssetDatabase.FindAssets("t:Pipeline");
		foreach ( var guid in guids )
		{
			var pipeline = AssetDatabase.LoadAssetAtPath<Pipeline>( AssetDatabase.GUIDToAssetPath( guid ) );
			
			if( pipeline.EnabledTasks[0].Task is RunOnBuildPreprocess )
			{
				Debug.Log( $"[On Pre Build] Run Pipeline: {pipeline.name}" );
				pipeline.RunTasks( );
		
				AssetDatabase.SaveAssets( );
				AssetDatabase.Refresh( );
			}
		}
	}

	public void Run( Pipeline ppl, Context ctx ) { }
}