using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventTemplate : MonoBehaviour
{
    [SerializeField] private Image eventFlag;
    [SerializeField] private Text eventTitle;
    [SerializeField] private Text eventState;
    [SerializeField] private Text eventDate;
    [SerializeField] private GameObject isDebug;

    private SchoolEvent schoolEvent;

    public void Init(SchoolEvent schoolEvent)
    {
        this.schoolEvent = schoolEvent;

        Refresh();
    }

    private void Refresh()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            EventPanel eventPanel = UIManager.Instance.OpenUI<EventPanel>() as EventPanel;
            eventPanel.Init(schoolEvent);
        });

        eventFlag.color = schoolEvent.EventType switch
        {
            SchoolEventType.Warning => Color.yellow,
            SchoolEventType.Error => Color.red,
            _ => Color.black
        };
        eventTitle.text = schoolEvent.Title;
        string state = schoolEvent.EventState switch
        {
            SchoolEventState.Todo => "未处理",
            SchoolEventState.Known => "已了解",
            SchoolEventState.Handled => "已处理",
            _ => ""
        };
        eventState.text = $"状态：{state}";
        eventDate.text = schoolEvent.Date;
        isDebug.SetActive(schoolEvent.IsDebug);
    }
}
