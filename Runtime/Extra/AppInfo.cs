namespace Flexy.AssetRefs.Extra;

public static class AppInfo
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static	void		Reset	( )		
	{
		_cachedAppRev		= null;
		_cachedAppRevShort	= null;
		_cachedVersionTag	= null;
		_cachedBranch		= null;
	}

	private static	String?		_cachedAppRev;
	private static	String?		_cachedAppRevShort;
	private static	String?		_cachedVersionTag;
	private static	String?		_cachedBranch;
	
	private static	String?		_cachedVersionTagAndVersion;
	private static	String?		_cachedRevisionAndBranch;
	
	public static	String		AppRev			
	{
		get
		{
			if (_cachedAppRev == null)
				ReadAppInfo();
				
			return _cachedAppRev ?? "";
		}
	}
	public static	String		AppRevShort		
	{
		get
		{
			if (_cachedAppRevShort == null)
				ReadAppInfo();
				
			return _cachedAppRevShort ?? "";
		}
	}
	public static	String		AppVersionTag	
	{
		get
		{
			if (_cachedVersionTag == null)
				ReadAppInfo();
				
			return _cachedVersionTag ?? "";
		}
	}
	public static	String		AppBranch		
	{
		get
		{
			if (_cachedBranch == null)
				ReadAppInfo();
				
			return _cachedBranch ?? "";
		}
	}
	
	public static	String		AppVersionTagAndVersion		
	{
		get
		{
			if (_cachedVersionTagAndVersion == null)
				ReadAppInfo();
				
			return _cachedVersionTagAndVersion ?? "";
		}
	}
	public static	String		RevisionAndBranch			
	{
		get
		{
			if (_cachedRevisionAndBranch == null)
				ReadAppInfo();
				
			return _cachedRevisionAndBranch ?? "";
		}
	}
	
	public static	void		SetAppRevision	( String revision, String revisionShort )	
	{
		_cachedAppRev		= revision;
		_cachedAppRevShort	= revisionShort;
	}
	public static	void		SetTag			( String tag )								
	{
		_cachedVersionTag		= tag;
	}
	public static	void		SetBranch		( String branch )							
	{
		_cachedBranch	= branch;
	}
	
	private static	void 		ReadAppInfo	( )		
	{
		_cachedAppRev		= Resources.Load<TextAsset>( "Fun.Flexy/BuildArtifacts/Revision" ) is { text: not null } o1 ? o1.text : "";
		_cachedVersionTag	= Resources.Load<TextAsset>( "Fun.Flexy/BuildArtifacts/VersionTag" ) is { text: not null } o2 ? o2.text : "";
		_cachedBranch		= Resources.Load<TextAsset>( "Fun.Flexy/BuildArtifacts/Branch" ) is { text: not null } o3 ? o3.text : "";
		
		_cachedAppRevShort	= _cachedAppRev.Length > 7 ? _cachedAppRev[..7] : _cachedAppRev;
		_cachedVersionTagAndVersion = $"{_cachedVersionTag} {Application.version}";
		_cachedRevisionAndBranch = $"{_cachedAppRevShort} {_cachedBranch}";
	}
}