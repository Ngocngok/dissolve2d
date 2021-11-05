using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/Manager/AudioClipManager"), fileName = "AudioClipManager")]
public class AudioClipManager : SerializedScriptableObject
{
    [SerializeField]
    [TableList(DrawScrollView = false, ShowPaging = true, NumberOfItemsPerPage = 25, ShowIndexLabels = true)] //(AlwaysExpanded = true)]
    private List<UniAudioClip> m_list = null;

    public List<UniAudioClip> ListBGM;
    public List<UniAudioClip> List
    {
        get { return m_list; }
    }

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
            dictAudioClip = new Dictionary<TYPE_SOUND, AudioClip>(List.Count);
            for (int i = 0; i < List.Count; i++)
            {
                dictAudioClip.Add(List[i].m_typeSound, List[i].Audioclip);
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
    public class UniAudioClip
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
    public string path;
    [Button]
    public void GenerateSFX()
    {
        m_list.Clear();
        string[] fileEntries = Directory.GetFiles(path);
        int index = 0;
        for (int i = 0; i < fileEntries.Length; i++)
        {
            if (!fileEntries[i].EndsWith(".meta"))
            {
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(fileEntries[i].Replace("\\", "/"));

                UniAudioClip uni = new UniAudioClip();
                uni.Audioclip = clip;
                uni.m_typeSound = (TYPE_SOUND) index;
                m_list.Add(uni);

                index++;
            }
        }
    }
#endif
}