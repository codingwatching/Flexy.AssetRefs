namespace Flexy.AssetRefs.Editor.PipelineTasks;

public class ReadAppRevisionFromGit : IPipelineTask
{
	public void Run( Pipeline ppln, Context ctx )
	{
		var buildArtifactsDir	= "Assets/Resources/Fun.Flexy/BuildArtifacts";
		var revisionPath		= buildArtifactsDir + "/Revision.txt";
		var revision			= GetGitRevision() ?? "";
		
		if (!Directory.Exists(buildArtifactsDir))
			Directory.CreateDirectory(buildArtifactsDir);
		
		File.WriteAllText(revisionPath, revision);
		Directory.GetParent(Application.dataPath);

		Debug.Log( $"[ReadAppRevisionFromGit] - Got revision {revision}" );
		AssetDatabase.ImportAsset(revisionPath, ImportAssetOptions.ForceSynchronousImport);
	}
	
	public static String? GetGitRevision( )
	{
		DirectoryInfo? gitDir = null;
		try
		{
			var currentDir = Directory.GetParent(Application.dataPath);
        
			// Search for .git directory up to 3 levels up
			for (var i = 0; i < 3; i++)
			{
				if (currentDir == null)
					break;
                
				var gitPath = Path.Combine(currentDir.FullName, ".git");
				if (Directory.Exists(gitPath))
				{
					gitDir = new DirectoryInfo(gitPath);
					break;
				}
            
				currentDir = currentDir.Parent;
			}
        
			if (gitDir == null)
			{
				Debug.LogWarning("Git repository not found");
				return null;
			}
        
			// Read HEAD file
			var headPath = Path.Combine(gitDir.FullName, "HEAD");
			if (!File.Exists(headPath))
			{
				Debug.LogWarning("HEAD file not found");
				return null;
			}
        
			var headContent = File.ReadAllText(headPath).Trim();
        
			// HEAD contains the revision directly (detached HEAD state)
			if (!headContent.StartsWith("ref: ")) 
				return headContent;

			// HEAD contains a reference. Extract the reference path by removing "ref: " prefix
			var refFilePath = Path.Combine(gitDir.FullName, headContent[5..]);
            
			if (File.Exists(refFilePath))
				return File.ReadAllText(refFilePath).Trim();

			Debug.LogWarning($"Reference file not found: {refFilePath}");
			return null;
		}
		catch (Exception ex)
		{
			Debug.LogError($"Error reading git revision: {ex.Message}");
			return null;
		}
	}
}