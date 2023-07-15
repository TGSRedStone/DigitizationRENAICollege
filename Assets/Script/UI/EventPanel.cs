using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPanel : UIBase
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private AudioSource audioSource;

    [Space]
    [SerializeField] private Image eventType;
    [SerializeField] private Text title;
    [SerializeField] private Text date;
    [SerializeField] private Text state;
    [SerializeField] private Text info;
    [SerializeField] private StaffUI[] staffs;
    [SerializeField] private Button handled;
    [SerializeField] private Button known;
    [SerializeField] private Button close;
    [SerializeField] private GameObject isDebug;

    private SchoolEvent schoolEvent;

    private StaffConfig staffConfig;
    private const string STAFF_CONFIG_PATH = "Configs/StaffConfig";

    private new void Awake()
    {
        staffConfig = Resources.Load<StaffConfig>(STAFF_CONFIG_PATH);
        audioSource.volume = SettingManager.Instance.Setting.Volume / 100f;

        handled.onClick.AddListener(() =>
        {
            schoolEvent.SetEventState(SchoolEventState.Handled);
            schoolEvent.isKnownOrHandled = true;
            SchoolEventManager.Instance.SaveEvent();
            RefreshView();
        });
        known.onClick.AddListener(() =>
        {
            schoolEvent.SetEventState(SchoolEventState.Known);
            schoolEvent.isKnownOrHandled = true;
            SchoolEventManager.Instance.SaveEvent();
            RefreshView();
        });
        close.onClick.AddListener(() =>
        {
            schoolEvent.isKnownOrHandled = true;
            SchoolEventManager.Instance.SaveEvent();
            UIManager.Instance.CloseUI<EventPanel>();
        });
    }

    public override void OnHide()
    {
        DOTween.To(alpha => { canvasGroup.alpha = alpha; },
            startValue: 1f,
            endValue: 0f,
            duration: 0.5f)
            .OnComplete(() =>
            {
                audioSource.Stop();
                Destroy(gameObject);
            });
    }

    public override void OnShow()
    {
        DOTween.To(alpha => { canvasGroup.alpha = alpha; },
            startValue: 0f,
            endValue: 1f,
            duration: 0.5f)
            .OnStart(() => { RefreshView(); })
            .OnComplete(() => { audioSource.Play(); });
    }

    protected override void UpdateView() { }

    public void Init(SchoolEvent schoolEvent)
    {
        this.schoolEvent = schoolEvent;
    }

    public void RefreshView()
    {
        audioSource.mute = schoolEvent.isKnownOrHandled;

        eventType.color = schoolEvent.EventType switch
        {
            SchoolEventType.Warning => Color.yellow,
            SchoolEventType.Error => Color.red,
            _ => Color.grey
        };
        title.text = schoolEvent.Title;
        date.text = schoolEvent.Date;
        string temp = schoolEvent.EventState switch
        {
            SchoolEventState.Todo => "未处理",
            SchoolEventState.Known => "已了解",
            SchoolEventState.Handled => "已处理",
            _ => ""
        };
        state.text = $"状态：{temp}";
        info.text = schoolEvent.Info;
        for (int i = 0; i < staffConfig.Staffs.Count; i++)
        {
            staffs[i].Avatar.sprite = staffConfig.Staffs[i].Avatar;
            staffs[i].Name.text = staffConfig.Staffs[i].Name;
            staffs[i].Office.text = staffConfig.Staffs[i].Office;
            staffs[i].PhoneNumber.text = $"电话：{staffConfig.Staffs[i].PhoneNumber}";
        }
        isDebug.SetActive(schoolEvent.IsDebug);

        switch (schoolEvent.EventState)
        {
            case SchoolEventState.Empty:
                break;
            case SchoolEventState.Todo:
                known.interactable = true;
                handled.interactable = true;
                break;
            case SchoolEventState.Known:
                known.interactable = false;
                handled.interactable = true;
                break;
            case SchoolEventState.Handled:
                known.interactable = false;
                handled.interactable = false;
                break;
            default:
                break;
        }
    }
}

[Serializable]
public class StaffUI
{
    public Image Avatar;
    public Text Name;
    public Text Office;
    public Text PhoneNumber;
}