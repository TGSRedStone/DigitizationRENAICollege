using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    private const string SETTING_SAVE_PATH = "/Event";
    private const string SETTING_NAME = "Setting.json";
    public SchoolSetting Setting { get; private set; } = new SchoolSetting();

    private void Awake()
    {
        if (Instance is null) Instance = this;

        LoadSetting();
    }

    public void LoadSetting()
    {
        string filePath = Application.streamingAssetsPath + SETTING_SAVE_PATH;
        string fileFullPath = filePath + "/" + SETTING_NAME;

        CheckDirectory(filePath);

        if (File.Exists(fileFullPath))
        {
            using (StreamReader sr = new StreamReader(fileFullPath))
            {
                string temp = sr.ReadToEnd();
                if (temp is null || temp is "")
                    SaveSetting();
                else
                    Setting = JsonUtility.FromJson<SchoolSetting>(temp);

                sr.Close();
                sr.Dispose();
            }
        }
        else
        {
            File.Create(fileFullPath);
            SaveSetting();
        }
    }

    public void SaveSetting()
    {
        string filePath = Application.streamingAssetsPath + SETTING_SAVE_PATH;
        string fileFullPath = filePath + "/" + SETTING_NAME;

        CheckDirectory(filePath);
        if (!File.Exists(fileFullPath)) File.Create(fileFullPath);

        using (StreamWriter sw = new StreamWriter(fileFullPath))
        {
            string temp = JsonUtility.ToJson(Setting);
            sw.Write(temp);

            sw.Close();
            sw.Dispose();
        }
    }

    private void CheckDirectory(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }
}
