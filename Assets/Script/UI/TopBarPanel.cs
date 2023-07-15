using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using QFramework;

public sealed class TopBarPanel : UIBase
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
            if (UIManager.Instance.FindUI<BaseInfoPanel>())
            {
                UIManager.Instance.CloseUI<BaseInfoPanel>();
            }
            else
            {
                UIManager.Instance.CloseAllUI();
                UIManager.Instance.OpenUI<BaseInfoPanel>();
            }
        });
        chooseBuilding.onClick.AddListener(() =>
        {
            if (UIManager.Instance.FindUI<SelectBuildingPanel>())
            {
                UIManager.Instance.CloseUI<SelectBuildingPanel>();
            }
            else
            {
                UIManager.Instance.CloseAllUI();
                UIManager.Instance.OpenUI<SelectBuildingPanel>();
            }
        });
        setting.onClick.AddListener(() =>
        {
            if (UIManager.Instance.FindUI<SettingPanel>())
            {
                UIManager.Instance.CloseUI<SettingPanel>();
            }
            else
            {
                UIManager.Instance.CloseAllUI();
                UIManager.Instance.OpenUI<SettingPanel>();
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
