using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : UIBase
{
    [SerializeField] private CanvasGroup canvasGroup;

    [Space]
    [Header("设置")]
    [SerializeField] private InputField powerWarning;
    [SerializeField] private InputField powerError;
    [SerializeField] private InputField waterWarning;
    [SerializeField] private InputField waterError;
    [SerializeField] private Slider volume;

    [Space]
    [Header("数据模拟")]
    [SerializeField] private Toggle isAnalog;
    [SerializeField] private InputField minScale;
    [SerializeField] private InputField maxScale;

    [Space]
    [Header("调试")]
    [SerializeField] private Dropdown building;
    [SerializeField] private Dropdown dataType;
    [SerializeField] private InputField value;
    [SerializeField] private InputField count;
    [SerializeField] private Button generate;
    [SerializeField] private Text info;

    private BuildingModel buildingModel;

    private int buildingID = 0;
    private DataType type = DataType.Power;

    private new void Awake()
    {
        base.Awake();

        buildingModel = this.GetModel<BuildingModel>();

        powerWarning.onEndEdit.AddListener(value =>
        {
            SettingManager.Instance.Setting.PowerWarning = float.Parse(value);
            SettingManager.Instance.SaveSetting();
        });
        powerError.onEndEdit.AddListener(value =>
        {
            SettingManager.Instance.Setting.PowerError = float.Parse(value);
            SettingManager.Instance.SaveSetting();
        });
        waterWarning.onEndEdit.AddListener(value =>
        {
            SettingManager.Instance.Setting.WaterWarning = float.Parse(value);
            SettingManager.Instance.SaveSetting();
        });
        waterError.onEndEdit.AddListener(value =>
        {
            SettingManager.Instance.Setting.WaterError = float.Parse(value);
            SettingManager.Instance.SaveSetting();
        });
        volume.onValueChanged.AddListener(value =>
        {
            SettingManager.Instance.Setting.Volume = (int)value;
            SettingManager.Instance.SaveSetting();
        });

        isAnalog.onValueChanged.AddListener(value =>
        {
            SettingManager.Instance.Setting.IsAnalog = value;
            SettingManager.Instance.SaveSetting();
            RefreshInfo();
        });
        minScale.onEndEdit.AddListener(value =>
        {
            SettingManager.Instance.Setting.MinScale = float.Parse(value);
            SettingManager.Instance.SaveSetting();
        });
        maxScale.onEndEdit.AddListener(value =>
        {
            SettingManager.Instance.Setting.MaxScale = float.Parse(value);
            SettingManager.Instance.SaveSetting();
        });

        building.ClearOptions();
        List<string> list = new List<string>();
        foreach (Building building in buildingModel.BuildingList)
            list.Add(building.Name);
        building.AddOptions(list);
        building.onValueChanged.AddListener(index =>
        {
            if (index == buildingID) return;
            buildingID = index;
            RefreshInfo();
        });
        dataType.onValueChanged.AddListener(index =>
        {
            if (index == (int)type - 1) return;
            type = (DataType)(index + 1);
            Refresh();
            RefreshInfo();
        });
        value.onEndEdit.AddListener((s) => { RefreshInfo(); });
        count.onEndEdit.AddListener((s) => { RefreshInfo(); });
        generate.onClick.AddListener(() =>
        {
            if (!SettingManager.Instance.Setting.IsAnalog)
            {
                RefreshInfo("<Color=red>未开启数据模拟，无法生成Debug</Color>");
                return;
            }

            bool checkValue;
            int tempValue = 0;
            if (type is DataType.Smoke)
            {
                checkValue = true;
            }
            else
            {
                int.TryParse(value.text, out tempValue);
                checkValue = !(value.text is null || value.text is "");
            }
            bool checkCount;
            int tempCount;
            int.TryParse(count.text, out tempCount);
            checkCount = !(count.text is null || count.text is "" || tempCount is 0);

            if (checkValue && checkCount)
            {
                SchoolDebugBase temp;
                if (type is DataType.Smoke)
                    temp = new SchoolDebugBool(building.value, type, true, tempCount);
                else
                    temp =  new SchoolDebugNumber(building.value, type, tempValue, tempCount);
                if (DataGenerationManager.Instance.CreateDebug(temp))
                    RefreshInfo("<Color=lime>Debug创建成功</Color>");
                else
                    RefreshInfo("<Color=red>创建失败，该建筑已有同类型Debug正在进行，请稍后重试</Color>");
            }
            else
            {
                RefreshInfo("<Color=yellow>请确保数据设置正确</Color>");
            }
        });
    }

    public override void OnHide()
    {
        DOTween.To(value => { canvasGroup.alpha = value; },
            startValue: 1f,
            endValue: 0f,
            duration: 0.5f)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    public override void OnShow()
    {
        DOTween.To(value => { canvasGroup.alpha = value; },
            startValue: 0f,
            endValue: 1f,
            duration: 0.5f)
            .OnStart(() =>
            {
                RefreshInfo();
            })
            .OnComplete(() =>
            {
                powerWarning.text = SettingManager.Instance.Setting.PowerWarning.ToString();
                powerError.text = SettingManager.Instance.Setting.PowerError.ToString();
                waterWarning.text = SettingManager.Instance.Setting.WaterWarning.ToString();
                waterError.text = SettingManager.Instance.Setting.WaterError.ToString();
                volume.value = SettingManager.Instance.Setting.Volume;

                isAnalog.isOn = SettingManager.Instance.Setting.IsAnalog;
                minScale.text = SettingManager.Instance.Setting.MinScale.ToString();
                maxScale.text = SettingManager.Instance.Setting.MaxScale.ToString();
            });
    }

    protected override void UpdateView() { }

    private void Refresh()
    {
        if (type is DataType.Smoke)
            value.interactable = false;
        else
            value.interactable = true;
    }

    private void RefreshInfo(string s = "")
    {
        info.text = s;
    }
}

[Serializable]
public class SchoolSetting
{
    public float PowerWarning = 1000f;
    public float PowerError = 2000f;
    public float WaterWarning = 30f;
    public float WaterError = 50f;
    public int Volume = 50;

    public bool IsAnalog = false;
    public float MinScale = 0.5f;
    public float MaxScale = 0.9f;
}