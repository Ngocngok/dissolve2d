using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = ("Configs/Sounds"), fileName = "SoundConfig")]
public class SoundConfig : SerializedScriptableObject
{
    public float VolumeSFX = 1f;
    public float VolumeBGM = 0.5f;

    [SerializeField]
    [TableList(DrawScrollView = false, ShowPaging = true, NumberOfItemsPerPage = 25, ShowIndexLabels = true)] //(AlwaysExpanded = true)]
    private List<HCAudioClip> clips = null;

    public List<HCAudioClip> Clips => clips;

    private Dictionary<TYPE_SOUND, AudioClip> dictAudioClip;

    public void ResetCache()
    {
        if (!dictAudioClip.CheckIsNullOrEmpty())
            dictAudioClip.Clear();
    }

    public AudioClip GetAudio(TYPE_SOUND typeSound)
    {
        if (dictAudioClip == null)
        {
            dictAudioClip = new Dictionary<TYPE_SOUND, AudioClip>(Clips.Count);
            for (int i = 0; i < Clips.Count; i++)
            {
                dictAudioClip.Add(Clips[i].m_typeSound, Clips[i].Audioclip);
            }
        }

        if (dictAudioClip.ContainsKey(typeSound))
            return dictAudioClip[typeSound];
        else
        {
            return dictAudioClip[TYPE_SOUND.BUTTON];
        }
    }

    [Serializable]
    public class HCAudioClip
    {
        [SerializeField, TableColumnWidth(250, Resizable = false)]
        private AudioClip m_audioClip = null;

        [SerializeField, TableColumnWidth(150)]
        public TYPE_SOUND m_typeSound;

        public AudioClip Audioclip
        {
            get => m_audioClip;
            set => m_audioClip = value;
        }
    }

#if UNITY_EDITOR
    [FolderPath]
    public string path = "Assets/Sounds";

    [Button]
    public void GenerateSFX()
    {
        clips.Clear();
        string[] fileEntries = Directory.GetFiles(path);
        int index = 0;
        for (int i = 0; i < fileEntries.Length; i++)
        {
            if (!fileEntries[i].EndsWith(".meta"))
            {
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(fileEntries[i].Replace("\\", "/"));

                var hc = new HCAudioClip();
                hc.Audioclip = clip;
                hc.m_typeSound = (TYPE_SOUND) index;
                clips.Add(hc);

                index++;
            }
        }
    }
#endif
}