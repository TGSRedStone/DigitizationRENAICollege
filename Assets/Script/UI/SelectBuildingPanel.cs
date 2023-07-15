using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using XCharts.Runtime;

public class SelectBuildingPanel : UIBase
{
    [Header("面板")]
    [SerializeField]
    private RectTransform buildingListPanelRectTrans;
    [SerializeField]
    private RectTransform infoPanelRectTrans;

    [Space]
    [Header("右面板组件")]
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text temperature;
    [SerializeField]
    private Text humidity;
    [SerializeField]
    private Image smokeLight;
    [SerializeField]
    private LineChart powerChart;
    [SerializeField]
    private LineChart waterChart;

    [Space]
    [Header("左面板组件")]
    [SerializeField]
    private List<Button> buttonList;

    private BuildingModel model;

    private int buildingIndex;
    private float tempTime;

    private float preTemperature = 0f;
    private float preHumidity = 0f;

    private new void Awake()
    {
        model = this.GetModel<BuildingModel>();

        powerChart.SetMaxCache(10);
        waterChart.SetMaxCache(10);

        for (int i = 0; i < buttonList.Count; i++)
        {
            int temp = i;
            buttonList[i].onClick.AddListener(() =>
            {
                ChangeBuilding(temp);
            });
        }

        powerChart.AnimationFadeIn(false);
        powerChart.AnimationPause();
        waterChart.AnimationFadeIn(false);
        waterChart.AnimationPause();
    }

    private void Start()
    {
        buildingIndex = -1;

        ChangeBuilding(0);
    }

    private void Update()
    {
        tempTime += Time.deltaTime;
        if(tempTime > 1f)
        {
            UpdateView();

            tempTime = 0f;
        }
    }

    public override void OnHide()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(buildingListPanelRectTrans.DOAnchorPosX(buildingListPanelRectTrans.anchoredPosition.x - buildingListPanelRectTrans.rect.width, 0.5f))
            .Join(infoPanelRectTrans.DOAnchorPosX(infoPanelRectTrans.anchoredPosition.x + infoPanelRectTrans.rect.width, 0.5f))
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    public override void OnShow()
    {
        Sequence sequence = DOTween.Sequence();
        float buildingPanelOriginX = buildingListPanelRectTrans.anchoredPosition.x;
        float infoPanelOriginX = infoPanelRectTrans.anchoredPosition.x;
        sequence.Append(buildingListPanelRectTrans.DOAnchorPosX(buildingPanelOriginX - buildingListPanelRectTrans.rect.width, 0f))
            .Join(infoPanelRectTrans.DOAnchorPosX(infoPanelOriginX + infoPanelRectTrans.rect.width, 0f))
            .AppendInterval(0.5f)
            .Append(buildingListPanelRectTrans.DOAnchorPosX(buildingPanelOriginX, 0.5f))
            .Join(infoPanelRectTrans.DOAnchorPosX(infoPanelOriginX, 0.5f))
            .OnComplete(() =>
            {
                powerChart.AnimationResume();
                waterChart.AnimationResume();
            });
    }

    protected override void UpdateView()
    {
        temperature.text = $"当前温度：{model.BuildingList[buildingIndex].Temperature.ToString("f1")}℃";
        humidity.text = $"当前湿度：{model.BuildingList[buildingIndex].Humidity.ToString("#0.0")}%RH";

        smokeLight.color = model.BuildingList[buildingIndex].IsSmoke ? Color.red : Color.green;

        powerChart.AddData(0, model.BuildingList[buildingIndex].Power);
        waterChart.AddData(0, model.BuildingList[buildingIndex].Water);
    }

    private void ChangeBuilding(int index)
    {
        UIManager.Instance.ControlCamera(
            model.BuildingList[index].BuildingPos,
            Quaternion.Euler(Vector3.zero),
            Quaternion.Euler(Vector3.right * 45f + (model.BuildingList[index].Direction == Direction.South ? Vector3.zero : (Vector3.up * 180f))),
            model.BuildingList[index].CameraDistance,
            0.5f, true);

        if (index == buildingIndex) return;

        buildingIndex = index;
        title.text = buttonList[index].name;

        DOTween.To(value => { temperature.text = $"当前温度：{value.ToString("f1")}℃"; },
            preTemperature,
            model.BuildingList[index].Temperature,
            0.25f)
            .OnComplete(() =>
            {
                preTemperature = model.BuildingList[index].Temperature;
            });
        DOTween.To(value => { humidity.text = $"当前湿度：{value.ToString("#0.0")}%RH"; },
            preHumidity,
            model.BuildingList[index].Humidity,
            0.25f)
            .OnComplete(() =>
            {
                preHumidity = model.BuildingList[index].Humidity;
            });

        powerChart.ClearSerieData();
        waterChart.ClearSerieData();
        double[] tempPower = model.BuildingList[index].PowerQueue.ToArray();
        double[] tempWater = model.BuildingList[index].WaterQueue.ToArray();
        for (int i = 0; i < tempPower.Length; i++)
        {
            powerChart.AddData(0, tempPower[i]);
        }
        for (int i = 0; i < tempWater.Length; i++)
        {
            waterChart.AddData(0, tempWater[i]);
        }
    }
}
