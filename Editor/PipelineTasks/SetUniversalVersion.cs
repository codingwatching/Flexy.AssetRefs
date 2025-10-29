namespace Flexy.AssetRefs.Editor.PipelineTasks;

public class SetUniversalVersion : IPipelineTask
{
	[SerializeField]	Int32		_n;
	[SerializeField]	ETimeSource	_timeSource;
	[SerializeField]	EMajorType	_major;
	[SerializeField]	EMinorType	_minor;
	[SerializeField]	EBuildType	_build;

	public void Run( Pipeline ppln, Context ctx )
	{
		var time = _timeSource switch
		{
			ETimeSource.UTC		=> DateTime.UtcNow, 
			ETimeSource.Local	=> DateTime.Now,
		};
		
		var major = _major switch
		{
			EMajorType.YearMinusN	=> time.Year - _n,
			EMajorType.N			=> _n,
		};
		
		var minor = "";
		switch (_minor)
		{
			case EMinorType.MonthAndDay: minor = $"{time.Month:00}{time.Day:00}"; break;
			case EMinorType.SeasonAndDay:
				var season	= time.Month switch { 1 => 1, 2 => 1, 3 => 2, 4 => 2, 5 => 2, 6 => 3, 7 => 3, 8 => 3, 9 => 4, 10 => 4, 11 => 4, 12 => 5 };
				var day		= Math.Min(time.Day, 30) + (time.Month < 3 ? time.Month-1 : (time.Month-3) % 3) * 30;
				minor = $"{season:0}{day:00}";
				break;
		}

		var build = _build switch
		{
			EBuildType.Hours					=> $"{time.Hour + (time.Day == 31 ? 50 : 0):00}",
			EBuildType.HoursAnd10th				=> $"{time.Hour + (time.Day == 31 ? 50 : 0):00}{Mathf.RoundToInt(time.Minute / 6f):0}",
			EBuildType.QwartersOfHours			=> $"{time.Hour*4 + time.Minute/15:00}",
			EBuildType.QwartersOfHoursAnd10th	=> $"{time.Hour*4 + time.Minute/15:00}{Mathf.RoundToInt(time.Minute % 15 / 1.5f)}",
		};
		
		var versionString = $"{major}.{minor}.{build}";

		Debug.Log( $"[SetUniversalVersion] - Setting universal version {versionString}" );

		PlayerSettings.bundleVersion				= versionString;
		
		PlayerSettings.Android.bundleVersionCode	= Int32.Parse(versionString.Replace(".", ""));
		PlayerSettings.macOS.buildNumber			= versionString;
		PlayerSettings.tvOS.buildNumber				= versionString;
		PlayerSettings.iOS.buildNumber				= versionString;
	}

	public enum ETimeSource : byte
	{
		UTC,
		Local,
	}
	public enum EMajorType : byte
	{
		YearMinusN,
		N,
	}
	public enum EMinorType : byte
	{
		MonthAndDay,
		SeasonAndDay,
	}
	public enum EBuildType : byte
	{
		Hours,
		HoursAnd10th,
		QwartersOfHours,
		QwartersOfHoursAnd10th,
	}
}

