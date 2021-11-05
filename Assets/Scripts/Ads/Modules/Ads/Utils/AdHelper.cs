//using BestHTTP;
//using G2.Sdk.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RocketTeam.Sdk.Services.Ads
{
    public class AdHelper
    {
        private const string pathLogShow = "ads/show/";
        private const string pathLogClick = "ads/click/";
        private const string pathLogClose = "ads/close/";
        private static AndroidJavaClass androidJavaClass;

        private const string LIST_BUNDLE_ID_OF_RECEIVED_REWARD_KEY = "G2.Sdk.Services.Ads_list_bundle_id_of_received_reward";
        private const string LIST_AD_IMPRESSIONS_KEY = "G2.Sdk.Services.Ads_ad_impression";
        private const int MAX_IMPRESSIONS = 3;

        #region OTHER
//        public static string GetDeviceId()
//        {
//            string deviceId = string.Empty;

//#if UNITY_EDITOR
//            deviceId = SystemInfo.deviceUniqueIdentifier;
//#elif UNITY_ANDROID
//            AndroidJavaClass androidUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//            AndroidJavaObject unityPlayerActivity = androidUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//            AndroidJavaObject unityPlayerResolver = unityPlayerActivity.Call<AndroidJavaObject>("getContentResolver");
//            AndroidJavaClass androidSettingsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
//            deviceId = androidSettingsSecure.CallStatic<string>("getString", unityPlayerResolver, "android_id");

////#elif UNITY_IOS
////            string keyChainJsonData = KeyChain.BindGetKeyChainUser();
////            KeyChainData keyChain = JsonConvert.DeserializeObject<KeyChainData>(keyChainJsonData);
////            deviceId = keyChain.uuid;

////            LogConsole(keyChainJsonData);
////            LogConsole(keyChain.ToString());

////            if (string.IsNullOrEmpty(deviceId))
////            {
////                deviceId = SystemInfo.deviceUniqueIdentifier;
////                KeyChain.BindSetKeyChainUser("com.vtcgame.zombie.io.undead.zone", deviceId);
////            }

//#else 
//            deviceId = SystemInfo.deviceUniqueIdentifier;
//#endif

//            return deviceId;
//        }

//        public static int GetScreenRatio()
//        {
//            float ratio = (float)Screen.width / (float)Screen.height;
//            if (ratio >= 1.7)
//            {
//                return (int)AdEnums.ScreenRatio.RATIO_169;
//            }
//            else if (ratio < 1.5)
//            {
//                return (int)AdEnums.ScreenRatio.RATIO_43;
//            }
//            else
//            {
//                return (int)AdEnums.ScreenRatio.UNKNOW;
//            }
//        }

//        public static int GetOrientation()
//        {
//            int orientation = (int)AdEnums.Orientation.UNKNOWN;
//            if (Screen.width >= Screen.height)
//            {
//                orientation = (int)AdEnums.Orientation.LANDSCAPE;
//            }
//            else
//            {
//                orientation = (int)AdEnums.Orientation.PORTRAIT;
//            }
//            return orientation;
//        }

//        public static int GetOS()
//        {
//#if UNITY_IOS
//            return (int)AdEnums.OS.IOS;
//#else
//            return (int)AdEnums.OS.ANDROID;
//#endif
//        }

//        public static string CreateMoneyText(int money) {
//            if (money < 1000) {
//                return "+"+money+" ";
//            }
//            if (money < 1000000) {
//                int tmp = money / 1000;
//                return "+"+tmp + "K ";
//            }
//            int tmp2 = money / 1000000;
//            return "+" + tmp2 + "M ";
//        }
        #endregion

        #region AD_LOG
        public static void SendLogClickAd(string getId, int isReceivedReward)
        {
            //try
            //{
            //    if (string.IsNullOrEmpty(getId))
            //    {
            //        DebugCustom.Log("getId IsNullOrEmpty");
            //        return;
            //    }
            //    HTTPRequest request;
            //    if (SdkManager.Instance.PrepareRequest(pathLogClick, HTTPMethods.Post, null, out request)) {
            //        request.AddField("getId", getId);
            //        request.AddField("isReceivedReward", isReceivedReward.ToString()); // 1 = received, 0 = not received

            //        request.Send();
            //    }
            //}
            //catch (Exception ex) {
            //    DebugCustom.Log(ex.ToString());
            //}
        }

        public static void SendLogCloseAd(string groupId)
        {            
            //try
            //{
            //    if (string.IsNullOrEmpty(groupId))
            //    {
            //        DebugCustom.Log("groupId IsNullOrEmpty");
            //        return;
            //    }

            //    HTTPRequest request;
            //    if (SdkManager.Instance.PrepareRequest(pathLogClose, HTTPMethods.Post, null, out request)) {
            //        request.AddField("groupId", groupId);

            //        request.Send();
            //    }                
            //}
            //catch (Exception ex)
            //{
            //    DebugCustom.Log(ex.ToString());
            //}

        }

        public static void SendLogShowAd(string getId, string groupId)
        {            
            //try
            //{
            //    if (string.IsNullOrEmpty(getId) && string.IsNullOrEmpty(groupId))
            //    {
            //        DebugCustom.Log("getId or groupId IsNullOrEmpty");
            //        return;
            //    }
            //    HTTPRequest request;
            //    if (SdkManager.Instance.PrepareRequest(pathLogShow, HTTPMethods.Post, null, out request))
            //    {
            //        if (!string.IsNullOrEmpty(getId))
            //        {
            //            request.AddField("getId", getId);
            //        }
            //        else {
            //            request.AddField("groupId", groupId);
            //        }
            //        request.Send();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DebugCustom.Log(ex.ToString());
            //}

        }
        #endregion

        #region CHECK_OTHER_APP

        public static bool CheckInstall(string bundleID)
        {
            if (!string.IsNullOrEmpty(bundleID))
            {
#if UNITY_ANDROID
                try
                {
                    using (AndroidJavaObject packageManager = GetUnityCurrentActivity().Call<AndroidJavaObject>("getPackageManager"))
                    {
                    
                        using (AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleID))
                        {
                            if (launchIntent != null)
                            {
                                return true;
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }
#else
                return false;
#endif
            }
            return false;
        }

        public static void OpenGame(string bundleID, string urlStore)
        {
            if (!string.IsNullOrEmpty(bundleID))
            {
#if UNITY_ANDROID
                try
                {
                    using (AndroidJavaObject packageManager = GetUnityCurrentActivity().Call<AndroidJavaObject>("getPackageManager"))
                    {
                    
                        using (AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleID))
                        {
                            GetUnityCurrentActivity().Call("startActivity", launchIntent);
                            return;
                        }
                    
                    }
                }
                catch
                {
                    if (!urlStore.Contains("http"))
                    {
                        Application.OpenURL(string.Format("market://details?id={0}", urlStore));
                    }
                    else
                    {
                        Application.OpenURL(urlStore);
                    }
                }
#else
                if (!urlStore.Contains("http"))
                {
                    Application.OpenURL(string.Format("itms-apps://itunes.apple.com/app/id{0}", urlStore));
                }
                else
                {
                    Application.OpenURL(urlStore);
                }
#endif
            }
        }

        private static AndroidJavaObject GetUnityCurrentActivity()
        {
            if (androidJavaClass == null) {
                androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            }
            AndroidJavaObject ca = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");

            return ca;
        }
        #endregion

        #region REWARD
        public static bool IsReceivedReward(string bundleId)
        {
            string data = PlayerPrefs.GetString(LIST_BUNDLE_ID_OF_RECEIVED_REWARD_KEY, null);
            if (!string.IsNullOrEmpty(data)) {
                string[] listBundleId = data.Split(',');
                for (int i = 0; i < listBundleId.Length; i ++) {
                    if (listBundleId[i].Equals(bundleId)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void AddNewBundleReceivedReward(string bundleId) {
            string data = PlayerPrefs.GetString(LIST_BUNDLE_ID_OF_RECEIVED_REWARD_KEY, null);
            if (!string.IsNullOrEmpty(data))
            {
                string newData = data + "," + bundleId;
                PlayerPrefs.SetString(LIST_BUNDLE_ID_OF_RECEIVED_REWARD_KEY, newData);
            }
            else {
                PlayerPrefs.SetString(LIST_BUNDLE_ID_OF_RECEIVED_REWARD_KEY, bundleId);
            }
        }
        #endregion

        #region BLACKLIST
        public static bool IsInBlacklist(string bundleId, string url)
        {
            string data = PlayerPrefs.GetString(LIST_AD_IMPRESSIONS_KEY, null);
            if (!string.IsNullOrEmpty(data))
            {
                Dictionary<string, BlacklistData> dict = JsonConvert.DeserializeObject<Dictionary<string, BlacklistData>>(data);
                if (dict.ContainsKey(bundleId) && 
                    dict[bundleId].url.Equals(url))
                {
                    if (dict[bundleId].impressions >= 3)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void AddToListAdImpressions(string bundleId, string url)
        {
            string data = PlayerPrefs.GetString(LIST_AD_IMPRESSIONS_KEY, null);
            if (!string.IsNullOrEmpty(data))
            {
                Dictionary<string, BlacklistData> dict = JsonConvert.DeserializeObject<Dictionary<string, BlacklistData>>(data);
                if (dict.ContainsKey(bundleId))
                {
                    if (dict[bundleId].url.Equals(url))
                    {
                        int tmp = dict[bundleId].impressions + 1;
                        dict[bundleId] = new BlacklistData(url, tmp);
                    }
                    else {

                        dict[bundleId] = new BlacklistData(url, 1);
                    }
                }
                else {
                    dict.Add(bundleId, new BlacklistData(url, 1));
                }
                string newData = JsonConvert.SerializeObject(dict);
                PlayerPrefs.SetString(LIST_AD_IMPRESSIONS_KEY, newData);
            }
            else
            {
                Dictionary<string, BlacklistData> dict = new Dictionary<string, BlacklistData>();
                dict.Add(bundleId, new BlacklistData(url, 1));
                string newData = JsonConvert.SerializeObject(dict);
                PlayerPrefs.SetString(LIST_AD_IMPRESSIONS_KEY, newData);
            }
        }
        #endregion
    }
}