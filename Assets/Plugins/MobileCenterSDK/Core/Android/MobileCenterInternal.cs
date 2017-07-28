﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System;

namespace Microsoft.Azure.Mobile.Unity.Internal
{
    class MobileCenterInternal
    {
        private static AndroidJavaClass _mobileCenter = new AndroidJavaClass("com.microsoft.azure.mobile.MobileCenter");
        
        public static void mobile_center_unity_configure(string appSecret)
        {
            _mobileCenter.CallStatic("configure", GetAndroidApplication(), appSecret);
        }

        public static void mobile_center_unity_start(string appSecret, IntPtr[] servicesArray, int numServices)
        {
            IntPtr services = servicesArray[0];
            IntPtr stringClass = AndroidJNI.FindClass("java/lang/String");
            IntPtr mobileCenterRawClass = AndroidJNI.FindClass("com/microsoft/azure/mobile/MobileCenter");

            IntPtr methId = AndroidJNI.GetMethodID(stringClass, "<init>", "(Ljava/lang/String;)V");
            IntPtr rawJavaString = AndroidJNI.NewStringUTF(appSecret);
            //IntPtr rawJavaStringObject = AndroidJNI.NewObject(stringClass, methId, new jvalue[] { new jvalue() { l = strId } });
            IntPtr start_Method = AndroidJNI.GetStaticMethodID(mobileCenterRawClass, "start",
                                                               "(Landroid/app/Application;Ljava/lang/String;[Ljava/lang/Class;)V");
            AndroidJNI.CallStaticVoidMethod(mobileCenterRawClass, start_Method, new jvalue[]
            { 
                new jvalue { l = GetAndroidApplication().GetRawObject() }, 
                new jvalue { l = rawJavaString }, 
                new jvalue { l = services } 
            });
        }

        public static void mobile_center_unity_start_services(IntPtr[] servicesArray, int numServices)
        {
            IntPtr services = servicesArray[0];
            IntPtr stringClass = AndroidJNI.FindClass("java/lang/String");
            IntPtr mobileCenterRawClass = AndroidJNI.FindClass("com/microsoft/azure/mobile/MobileCenter");
            IntPtr methId = AndroidJNI.GetMethodID(stringClass, "<init>", "(Ljava/lang/String;)V");
            IntPtr start_Method = AndroidJNI.GetStaticMethodID(mobileCenterRawClass, "start", "([Ljava/lang/Class;)V");
            AndroidJNI.CallStaticVoidMethod(mobileCenterRawClass, start_Method, new jvalue[]
            {
                new jvalue { l = services }
            });        
        }

        public static void mobile_center_unity_set_log_level(int logLevel)
        {
            _mobileCenter.CallStatic("setLogLevel", logLevel);
        }

        public static int mobile_center_unity_get_log_level()
        {
            return _mobileCenter.CallStatic<int>("getLogLevel");
        }

        public static bool mobile_center_unity_is_configured()
        {
            return _mobileCenter.CallStatic<bool>("isConfigured");
        }

        public static void mobile_center_unity_set_log_url(string logUrl)
        {
            _mobileCenter.CallStatic("setLogUrl", logUrl);
        }


        public static MobileCenterTask SetEnabledAsync(bool enabled)
        {
            var future = _mobileCenter.CallStatic<AndroidJavaObject>("setEnabled", enabled);
            return new MobileCenterTask(future);
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            var future = _mobileCenter.CallStatic<AndroidJavaObject>("isEnabled");
            return new MobileCenterTask<bool>(future);
        }

        public static MobileCenterTask<string> GetInstallIdAsync()
        {
            AndroidJavaObject future = _mobileCenter.CallStatic<AndroidJavaObject>("getInstallId");
            var task = new MobileCenterTask<AndroidJavaObject>(future);
            task.ContinueWith(t => {
                t.Result;
            });
            return installId.Call<string>("toString");
        }

        public static void mobile_center_unity_set_custom_properties(AndroidJavaObject properties)
        {
            _mobileCenter.CallStatic("setCustomProperties", properties);
        }

        private static AndroidJavaObject GetAndroidApplication()
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            return activity.Call<AndroidJavaObject>("getApplication");
        }

        public static void mobile_center_unity_set_wrapper_sdk(string wrapperSdkVersion,
                                                               string wrapperSdkName, 
                                                               string wrapperRuntimeVersion, 
                                                               string liveUpdateReleaseLabel, 
                                                               string liveUpdateDeploymentKey, 
                                                               string liveUpdatePackageHash)
        {
            var wrapperSdkObject = new AndroidJavaObject("com.microsoft.azure.mobile.ingestion.models.WrapperSdk");
            wrapperSdkObject.Call("setWrapperSdkVersion", wrapperSdkVersion);
            wrapperSdkObject.Call("setWrapperSdkName", wrapperSdkName);
            wrapperSdkObject.Call("setWrapperRuntimeVersion", wrapperRuntimeVersion);
            wrapperSdkObject.Call("setLiveUpdateReleaseLabel", liveUpdateReleaseLabel);
            wrapperSdkObject.Call("setLiveUpdateDeploymentKey", liveUpdateDeploymentKey);
            wrapperSdkObject.Call("setLiveUpdatePackageHash", liveUpdatePackageHash);
            _mobileCenter.CallStatic("setWrapperSdk", wrapperSdkObject);
        }
    }
}
#endif
