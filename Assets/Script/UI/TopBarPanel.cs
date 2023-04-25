using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using QFramework;

public sealed class TopBarPanel : UI
{
    [SerializeField]
    private Button baseInfo;
    [SerializeField]
    private Button chooseBuilding;
    [SerializeField]
    private Button setting;
    [SerializeField]
    private Button exit;
    [SerializeField]
    private Text date;
    [SerializeField]
    private Text time;

    private IFormatProvider formatProvider = new System.Globalization.CultureInfo("zh-cn");

    private new void Awake()
    {
        base.Awake();

        baseInfo.onClick.AddListener(() =>
        {
            if (ManagerOfUI.Instance.FindUI<BaseInfoPanel>())
            {
                ManagerOfUI.Instance.CloseUI<BaseInfoPanel>();
            }
            else
            {
                ManagerOfUI.Instance.CloseAllUI();
                ManagerOfUI.Instance.OpenUI<BaseInfoPanel>();
            }
        });
        chooseBuilding.onClick.AddListener(() =>
        {
            if (ManagerOfUI.Instance.FindUI<SelectBuildingPanel>())
            {
                ManagerOfUI.Instance.CloseUI<SelectBuildingPanel>();
            }
            else
            {
                ManagerOfUI.Instance.CloseAllUI();
                ManagerOfUI.Instance.OpenUI<SelectBuildingPanel>();
            }
        });
        setting.onClick.AddListener(() =>
        {
            if (ManagerOfUI.Instance.FindUI<SettingPanel>())
            {
                ManagerOfUI.Instance.CloseUI<SettingPanel>();
            }
            else
            {
                ManagerOfUI.Instance.CloseAllUI();
                ManagerOfUI.Instance.OpenUI<SettingPanel>();
            }
        });
        exit.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    private void Update()
    {
        UpdateView();
    }

    public override void OnHide()
    {

    }

    public override void OnShow()
    {
        Sequence sequence = DOTween.Sequence();
        float originY = rectTransform.anchoredPosition.y;
        sequence.Append(rectTransform.DOAnchorPosY(originY + rectTransform.rect.height, 0f))
            .AppendInterval(0.5f)
            .Append(rectTransform.DOAnchorPosY(originY, 0.5f));
    }

    protected override void UpdateView()
    {
        DateTime dateTime = DateTime.Now;
        date.text = dateTime.ToString("yyyy年MM月dd日 dddd", formatProvider);
        time.text = dateTime.ToLongTimeString().ToString();
    }
}
