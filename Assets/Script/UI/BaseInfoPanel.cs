using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public sealed class BaseInfoPanel : UI
{
    private enum TypeScreen
    {
        Empty,
        All,
        Warning,
        Error
    }

    private enum StateScreen
    {
        Empty,
        All,
        ToDo,
        Known,
        Hanlded
    }

    [Header("面板")]
    [SerializeField]
    private RectTransform eventPanel;
    [SerializeField]
    private RectTransform baseInfoPanel;

    #region 左面板
    [Header("左面板")]
    [SerializeField] private Text count;
    [SerializeField] private Dropdown eventType;
    [SerializeField] private Dropdown eventState;

    [SerializeField]
    private GameObject content;
    #endregion

    #region 右面板
    [Space]
    [Header("右面板")]
    [SerializeField]
    private PieChart studentChart;
    [SerializeField]
    private LineChart powerChart;
    [SerializeField]
    private LineChart waterChart;

    private Serie studentSerie;
    private Serie powerSerie;
    private Serie waterSerie;
    #endregion

    private BaseInfoModel baseInfoModel;

    private float tempTime = 0f;

    private TypeScreen typeScreen = TypeScreen.Empty;
    private StateScreen stateScreen = StateScreen.Empty;
    private GameObject eventTemplate;
    private const string EVENT_TEMPLATE_PATH = "Prefabs/UI/Event";

    private new void Awake()
    {
        base.Awake();
        baseInfoModel = this.GetModel<BaseInfoModel>();

        eventTemplate = Resources.Load(EVENT_TEMPLATE_PATH) as GameObject;

        #region 左面板
        eventType.onValueChanged.AddListener(index =>
        {
            SetTypeScreen(index);
        });
        eventState.onValueChanged.AddListener(index =>
        {
            SetStateScreen(index);
        });
        #endregion

        #region 右面板
        studentSerie = studentChart.GetSerie(0);
        powerChart.SetMaxCache(10);
        powerChart.ClearSerieData();
        powerSerie = powerChart.GetSerie(0);
        waterChart.SetMaxCache(10);
        waterChart.ClearSerieData();
        waterSerie = waterChart.GetSerie(0);

        studentChart.AnimationPause();
        powerChart.AnimationReset();
        powerChart.AnimationPause();
        waterChart.AnimationReset();
        waterChart.AnimationPause();
        #endregion

        SchoolEventManager.Instance.OnEventChange += Refresh;
    }

    private void OnDestroy()
    {
        SchoolEventManager.Instance.OnEventChange -= Refresh;
    }

    private void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > 1f)
        {
            UpdateView();

            tempTime = 0f;
        }
    }

    public override void OnHide()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(eventPanel.DOAnchorPosX(eventPanel.anchoredPosition.x - eventPanel.rect.width, 0.5f))
            .Join(baseInfoPanel.DOAnchorPosX(baseInfoPanel.anchoredPosition.x + baseInfoPanel.rect.width, 0.5f))
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    public override void OnShow()
    {
        Sequence sequence = DOTween.Sequence();
        float eventPanelOriginX = eventPanel.anchoredPosition.x;
        float baseInfoPanelOriginX = baseInfoPanel.anchoredPosition.x;
        sequence.Append(eventPanel.DOAnchorPosX(eventPanelOriginX - eventPanel.rect.width, 0f))
            .Join(baseInfoPanel.DOAnchorPosX(baseInfoPanelOriginX + baseInfoPanel.rect.width, 0f))
            .AppendInterval(0.5f)
            .Append(eventPanel.DOAnchorPosX(eventPanelOriginX, 0.5f))
            .Join(baseInfoPanel.DOAnchorPosX(baseInfoPanelOriginX, 0.5f))
            .OnComplete(() =>
            {
                #region 左面板初始操作
                SetTypeScreen((int)TypeScreen.All - 1);
                SetStateScreen((int)TypeScreen.All - 1);
                #endregion

                #region 右面板初始操作
                double[] tempPower = baseInfoModel.TotalPowerQueue.ToArray();
                double[] tempWater = baseInfoModel.TotalWaterQueue.ToArray();
                for (int i = 0; i < tempPower.Length; i++)
                {
                    powerSerie.AddData(tempPower[i]);
                }
                for (int i = 0; i < tempWater.Length; i++)
                {
                    waterSerie.AddData(tempWater[i]);
                }

                studentChart.AnimationResume();
                powerChart.AnimationResume();
                waterChart.AnimationResume();
                #endregion
            });
    }

    protected override void UpdateView()
    {
        studentSerie.UpdateData(0, 1, baseInfoModel.InSchool);
        studentSerie.UpdateData(1, 1, baseInfoModel.OutSchool);
        studentSerie.UpdateData(2, 1, baseInfoModel.LeaveOfAbsence);

        powerSerie.AddData(baseInfoModel.TotalPower);
        waterSerie.AddData(baseInfoModel.TotalWater);
    }

    private void Refresh()
    {
        for (int i = 0; i < content.transform.childCount; i++)
            Destroy(content.transform.GetChild(i).gameObject);

        ChangeNumber();
        var list = from e in SchoolEventManager.Instance.Events.List
                   where CheckEventType(e) && CheckEventState(e)
                   select e;
        bool CheckEventType(SchoolEvent e)
        {
            if      (typeScreen is TypeScreen.All)      return true;
            else if (typeScreen is TypeScreen.Warning)  return e.EventType is SchoolEventType.Warning;
            else if (typeScreen is TypeScreen.Error)    return e.EventType is SchoolEventType.Error;
            return false;
        }
        bool CheckEventState(SchoolEvent e)
        {
            if      (stateScreen is StateScreen.All)        return true;
            else if (stateScreen is StateScreen.ToDo)       return e.EventState is SchoolEventState.Todo;
            else if (stateScreen is StateScreen.Known)      return e.EventState is SchoolEventState.Known;
            else if (stateScreen is StateScreen.Hanlded)    return e.EventState is SchoolEventState.Handled;
            return false;
        }

        foreach (var e in list)
        {
            Instantiate(eventTemplate, content.transform)
                .GetComponent<EventTemplate>()
                .Init(e);
        }
    }

    private void SetTypeScreen(int index)
    {
        if (index == ((int)typeScreen - 1)) return;

        typeScreen = index switch
        {
            0 => TypeScreen.All,
            1 => TypeScreen.Warning,
            2 => TypeScreen.Error,
            _ => TypeScreen.Empty
        };

        Refresh();
    }

    private void SetStateScreen(int index)
    {
        if (index == ((int)stateScreen - 1)) return;

        stateScreen = index switch
        {
            0 => StateScreen.All,
            1 => StateScreen.ToDo,
            2 => StateScreen.Known,
            3 => StateScreen.Hanlded,
            _ => StateScreen.Empty
        };

        Refresh();
    }

    private void ChangeNumber()
    {
        int num = SchoolEventManager.Instance.Events.List.Count(e => e.EventState is SchoolEventState.Todo);
        count.text = $"{num} 条待处理事件";
    }
}
