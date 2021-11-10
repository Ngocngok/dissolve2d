using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


public class HCPanelCreator : EditorWindow
{
    private const string templatePath = "Assets/HyperCatSDK/Scripts/Templates/Template1.txt";
    private const string enumPath = "Assets/Scripts/UI/PanelEnums.cs";

    private const string hcPrefix = "hcEditor_";
    private const string scriptPath = "Assets/Scripts/UI/Panels/";

    private bool isPopup = false;
    private string className = string.Empty;

    #region UI Tools

    [MenuItem("HyperCat Toolkit/UI/New Screen")]
    public static void CreateNewScreen()
    {
        GetWindow<HCPanelCreator>("UI Panel Creator");
    }

    #endregion

    void OnGUI()
    {
        className = EditorGUILayout.TextField("Class name", className);

        isPopup = EditorGUILayout.Toggle("Is Popup", isPopup);

        if (GUILayout.Button("Create"))
        {
            if (isPopup)
            {
                if (className.Contains("Screen"))
                    className = className.Replace("Screen", "Popup");
                else
                    className = className.Insert(0, "Popup");
            }
            else
            {
                if (className.Contains("Popup"))
                    className = className.Replace("Popup", "Screen");
                else
                    className = className.Insert(0, "Screen");
            }

            AddNewEnum();
            ExportEntity();

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            Close();
        }
    }

    void AddNewEnum()
    {
        var enumScript = File.ReadAllText(enumPath);
        if (enumScript.Contains(className))
            return;

        var index = enumScript.IndexOf("HC_Place_Holder");

        var newEnumScript = enumScript.Insert(index, className + ",\n    ");

        File.WriteAllText(enumPath, newEnumScript);
    }

    void ExportEntity()
    {
        var templateFilePath = templatePath;
        var scriptTemplate = File.ReadAllText(templateFilePath);

        var script = scriptTemplate.Replace("HC_Class_Name", className);

        Directory.CreateDirectory(scriptPath);
        File.WriteAllText(scriptPath + className + ".cs", script);
    }
}