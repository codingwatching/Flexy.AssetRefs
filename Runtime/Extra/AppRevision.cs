namespace Flexy.AssetRefs.Extra;

public static class AppRevision
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static	void		Reset( )		
	{
		_cachedAppRev		= null;
		_cachedAppRevShort	= null;
	}

	private static	String?		_cachedAppRev;
	private static	String?		_cachedAppRevShort;
	
	public static	String		AppRev			
	{
		get
		{
			if (_cachedAppRev == null)
				ReadRevision();
				
			return _cachedAppRev ?? "";
		}
	}
	public static	String		AppRevShort		
	{
		get
		{
			if (_cachedAppRevShort == null)
				ReadRevision();
				
			return _cachedAppRevShort ?? "";
		}
	}
	
	public static	void		SetAppRevision	( String revision, String revisionShort )	
	{
		_cachedAppRev		= revision;
		_cachedAppRevShort	= revisionShort;
	}
	private static	void 		ReadRevision	( )											
	{
		_cachedAppRev		= Resources.Load<TextAsset>( "Fun.Flexy/BuildArtifacts/Revision" ) is { text: not null } o ? o.text : "";
		_cachedAppRevShort	= _cachedAppRev.Length > 7 ? _cachedAppRev[..7] : _cachedAppRev;
	}
}