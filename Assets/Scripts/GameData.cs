using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GameData
{
    public UserData User = new UserData();
    public SettingData Setting = new SettingData();

    [Serializable]
    public class UserData
    {
        //Progress Data
        public int Money = 0;
        public int Level = 1;
        public string Name = "Player";

        //Purchase Data
        public bool PurchasedNoAds = false;

        //Other Data
        public int SessionPlayed = 0;
        public DateTime LastTimeLogOut = DateTime.Now;
    }

    [Serializable]
    public class SettingData
    {
        public int Sound = 1;
        public int Haptic = 1;
        public int HighPerformance = 1;

        public bool EnablePN = false;
        public bool RequestedPN = false;
        public Dictionary<PushNotificationType, int> AndroidPNIndexes = new Dictionary<PushNotificationType, int>();
    }
}

public static class Database
{
    public static void SaveData()
    {
        string dataString = JsonConvert.SerializeObject(GameManager.Instance.Data);
        PlayerPrefs.SetString("GameData", dataString);
        PlayerPrefs.Save();
    }

    public static GameData LoadData()
    {
        if (!PlayerPrefs.HasKey("GameData"))
            return null;

        return JsonConvert.DeserializeObject<GameData>(PlayerPrefs.GetString("GameData"));
    }
}