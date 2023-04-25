using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SchoolEventManager : MonoBehaviour
{
    public static SchoolEventManager Instance { get; private set; }

    public event Action OnEventCreate;
    public event Action OnEventChange;
    public ListToJson Events { get; private set; } = new ListToJson();
    private const string EVENT_SAVE_PATH = "/Event";
    private const string EVENT_NAME = "SchoolEvents.json";

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        LoadEvent();
    }

    //private void Start()
    //{
    //    CreatEvent(SchoolEventType.Error, 1, "123", "info", true);
    //}

    private void LoadEvent()
    {
        string filePath = Application.streamingAssetsPath + EVENT_SAVE_PATH;
        string fileFullPath = filePath + "/" + EVENT_NAME;

        CheckDirectory(filePath);

        if (File.Exists(fileFullPath))
        {
            using (StreamReader sr = new StreamReader(fileFullPath))
            {
                string temp = sr.ReadToEnd();
                if (temp is null || temp is "")
                    SaveEvent();
                else
                    Events = JsonUtility.FromJson<ListToJson>(temp);

                sr.Close();
                sr.Dispose();
            }
        }
        else
        {
            File.Create(fileFullPath);
            SaveEvent();
        }
    }

    public void SaveEvent()
    {
        string filePath = Application.streamingAssetsPath + EVENT_SAVE_PATH;
        string fileFullPath = filePath + "/" + EVENT_NAME;

        CheckDirectory(filePath);
        if (!File.Exists(fileFullPath)) File.Create(fileFullPath);

        using (StreamWriter sw = new StreamWriter(fileFullPath))
        {
            string temp = JsonUtility.ToJson(Events);
            sw.Write(temp);

            sw.Close();
            sw.Dispose();
        }

        OnEventChange?.Invoke();
    }

    public void CreatEvent(SchoolEventType type, string title, string info, bool isDebug = false)
    {
        SchoolEvent tempEvent = new SchoolEvent(type, title, info, isDebug);
        Events.List.Add(tempEvent);
        SaveEvent();

        EventPanel temp = ManagerOfUI.Instance.OpenUI<EventPanel>() as EventPanel;
        temp.Init(tempEvent);

        OnEventCreate?.Invoke();
    }

    private void CheckDirectory(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }
}

[Serializable]
public class ListToJson
{
    public List<SchoolEvent> List = new List<SchoolEvent>();
}

[Serializable]
public class SchoolEvent
{
    public bool IsDebug;
    public SchoolEventType EventType;
    public string Date;
    public string Title;
    public string Info;
    public SchoolEventState EventState;

    public bool isKnownOrHandled;

    private SchoolEvent() { }
    public SchoolEvent(SchoolEventType type, string title, string info, bool isDebug)
    {
        EventType = type;
        Title = title;
        Info = info;

        IsDebug = isDebug;

        Date = DateTime.Now.ToString("G");
        EventState = SchoolEventState.Todo;

        isKnownOrHandled = false;
    }

    public void SetEventState(SchoolEventState state)
    {
        EventState = state;
    }
}

public enum SchoolEventType
{
    Empty,
    Warning,
    Error
}

public enum SchoolEventState
{
    Empty = 0,
    Todo = 1,
    Known = 2,
    Handled = 3
}