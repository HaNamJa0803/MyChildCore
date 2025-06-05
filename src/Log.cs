using System.Diagnostics;
using StardewModdingAPI;

namespace SpaceShared;

internal class Log
{
	public static IMonitor Monitor;

	public static bool IsVerbose => Monitor.IsVerbose;

	[DebuggerHidden]
	[Conditional("DEBUG")]
	public static void DebugOnlyLog(string str)
	{
		Monitor.Log(str, (LogLevel)1);
	}

	[DebuggerHidden]
	[Conditional("DEBUG")]
	public static void DebugOnlyLog(string str, bool pred)
	{
		if (pred)
		{
			Monitor.Log(str, (LogLevel)1);
		}
	}

	[DebuggerHidden]
	public static void Verbose(string str)
	{
		Monitor.VerboseLog(str);
	}

	public static void Trace(string str)
	{
		Monitor.Log(str, (LogLevel)0);
	}

	public static void Debug(string str)
	{
		Monitor.Log(str, (LogLevel)1);
	}

	public static void Info(string str)
	{
		Monitor.Log(str, (LogLevel)2);
	}

	public static void Warn(string str)
	{
		Monitor.Log(str, (LogLevel)3);
	}

	public static void Error(string str)
	{
		Monitor.Log(str, (LogLevel)4);
	}
}
