// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS

using UnityEngine;
using UnityEditor;
using System.IO;

public class MobileCenterSettingsMaker
{
    public MobileCenterSettingsMaker(string pathToBuiltProject)
    {
        _pathToLoaderFile = pathToBuiltProject + LoaderPathSuffix;
        _loaderFileText = File.ReadAllText(_pathToLoaderFile);
    }

    public MobileCenterSettingsMaker()
    {
    }

    private static string LoaderPathSuffix = "/Libraries/Plugins/iOS/Core/MobileCenterStarter.mm";
    private static string AppSecretSearchText = "mobile-center-app-secret";
    private static string LogUrlSearchText = "custom-log-url";
    private static string LogLevelSearchText = "/*LOG_LEVEL*/";
    private static string LogUrlToken = "MOBILE_CENTER_UNITY_USE_CUSTOM_LOG_URL";
    private static string UsePushToken = "MOBILE_CENTER_UNITY_USE_PUSH";

    private string _classListText = "";

    private string _loaderFileText;
    private string _pathToLoaderFile;

    public void SetLogLevel(int logLevel)
    {
        _loaderFileText = _loaderFileText.Replace(LogLevelSearchText, logLevel.ToString());
    }

    public void SetLogUrl(string logUrl)
    {
        AddToken(LogUrlToken);
        _loaderFileText = _loaderFileText.Replace(LogUrlSearchText, logUrl);
    }

    public void SetAppSecret(string appSecret)
    {
        _loaderFileText = _loaderFileText.Replace(AppSecretSearchText, appSecret);
    }

    public void StartPushClass()
    {
        AddToken(UsePushToken);
    }

    public void CommitSettings()
    {
        File.WriteAllText(_pathToLoaderFile, _loaderFileText);
    }

    private void AddToken(string token)
    {
        var tokenText = "#define " + token + "\n";
        _loaderFileText = tokenText + _loaderFileText;
    }
}

#endif
