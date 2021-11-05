using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

[ShowOdinSerializedPropertiesInInspector]
public class ConfigManager : Singleton<ConfigManager>, ISerializationCallbackReceiver, ISupportsPrefabSerialization
{
    public GameConfig Game;
    public SoundConfig Audio;

    #region Odin

#if UNITY_EDITOR
    [Button]
    public void LoadConfigs()
    {
        // Levels.Clear();
        // var path = "Assets/Configs/Levels";
        // var fileEntries = Directory.GetFiles(path);
        // for (int i = 0; i < fileEntries.Length; i++)
        // {
        //     if (fileEntries[i].EndsWith(".asset"))
        //     {
        //         var item =
        //             AssetDatabase.LoadAssetAtPath<LevelConfig>(fileEntries[i].Replace("\\", "/"));
        //         if (item != null)
        //         {
        //             Levels.Add(item);
        //         }
        //     }
        // }
    }
#endif

    [SerializeField, HideInInspector]
    private SerializationData serializationData;

    SerializationData ISupportsPrefabSerialization.SerializationData
    {
        get => this.serializationData;
        set => this.serializationData = value;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
    }

    #endregion
}