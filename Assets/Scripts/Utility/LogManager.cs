using System;
using System.Collections.Generic;

using UnityEngine;

class LogManager
{
    public enum ELogLevel
    {
        eLogLevelAll = 0x0,		// All
        eLogLevelDebug = 0x100,	// Debug
        eLogLevelInfo = 0x200,	// Info
        eLogLevelWarn = 0x300,	// Warning
        eLogLevelError = 0x400,	// Error
        eLogLevelFatal = 0x500,	// Fatal
        eLogLevelOff = 0x600		// Off
    };

    public static void SetLogLevel(ELogLevel e)
    {
        _eLogLevel = e;
    }

    //--------------------------------------------------------
    public static void Log_Fatal(string x)
    {
        if (_eLogLevel <= ELogLevel.eLogLevelFatal)
            Debug.LogError("[Fatal]" + x);
    }

    public static void Log_Error(string x)
    {
        if (_eLogLevel <= ELogLevel.eLogLevelError)
            Debug.LogError("[Error]" + x);
    }

    public static void Log_Warn(string x)
    {
        if (_eLogLevel <= ELogLevel.eLogLevelWarn)
            Debug.LogWarning("[Warn ]" + x);
    }

    public static void Log_Info(string x)
    {
        if (_eLogLevel <= ELogLevel.eLogLevelInfo)
            Debug.Log("[Info ]" + x);
    }

    public static void Log_Debug(string x)
    {
        if (_eLogLevel <= ELogLevel.eLogLevelDebug)
            Debug.Log("[Debug]" + x);
    }

    //--------------------------------------------------------
    public static void Log_Fatal(string name, string x)
    {
        if ( IsNamedLogEnabled(name, ELogLevel.eLogLevelFatal) )
            Debug.LogError("[" + name + "][Fatal] " + x);
    }

    public static void Log_Error(string name, string x)
    {
        if (IsNamedLogEnabled(name, ELogLevel.eLogLevelError))
            Debug.LogError("[" + name + "][Error] " + x);
    }

    public static void Log_Warn(string name, string x)
    {
        if (IsNamedLogEnabled(name, ELogLevel.eLogLevelWarn))
            Debug.LogError("[" + name + "][Warn ] " + x);
    }

    public static void Log_Info(string name, string x)
    {
        if (IsNamedLogEnabled(name, ELogLevel.eLogLevelInfo))
            Debug.LogError("[" + name + "][Info ] " + x);
    }

    public static void Log_Debug(string name, string x)
    {
        if (IsNamedLogEnabled(name, ELogLevel.eLogLevelDebug))
            Debug.LogError("[" + name + "][Debug] " + x);
    }

    //--------------------------------------------------------
    public static bool RegisterNamedLog(string strName)
    {
        if (!_NameLogs.ContainsKey(strName))
        {
            _NameLogs.Add(strName, ELogLevel.eLogLevelDebug);
        }

        return false;
    }

    public static void SetNamedLogLevel(string strName, ELogLevel e)
    {
        if (_NameLogs.ContainsKey(strName))
        {
            _NameLogs[strName] = e;
        }
    }

    public static bool IsNamedLogEnabled(string strName, ELogLevel e)
    {
        if (_NameLogs.ContainsKey(strName) && _NameLogs[strName] <= e)
        {
            return true;
        }

        return false;
    }

    private static ELogLevel _eLogLevel = ELogLevel.eLogLevelDebug;
    private static Dictionary<string, ELogLevel> _NameLogs = new Dictionary<string, ELogLevel>();
};
