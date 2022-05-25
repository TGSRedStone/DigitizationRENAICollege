using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public  class UIBase : MonoBehaviour
{
    [Header("当前UI处于哪个按钮上")]
    public int UINum;
    //判断是否底部UI可以隐藏
    public bool downUICanOut;
    [Header("---------------第 一 界 面 UI----------------")]
    [SerializeField]
    protected RectTransform LiftUI;
    [SerializeField]
    protected RectTransform RightUI;
    [SerializeField]
    protected RectTransform DownUI;
    [SerializeField]
    protected RectTransform LiftUIIn;
    [SerializeField]
    protected RectTransform RightUIIn;
    [SerializeField]
    protected RectTransform DownUIIn;
    [Tooltip("动画的移动时间")]
    public float MoveTime = 0.5f;
    [Space(20f)]
    [Header("---------------第 二 界 面 UI----------------")]
    [SerializeField]
    protected RectTransform LiftUI_2;
    [SerializeField]
    protected RectTransform RightUI_2;
    [Space(20f)]
    [Header("---------------第 三 界 面 UI----------------")]
    [SerializeField]
    protected RectTransform LiftUI_3;
    [SerializeField]
    protected RectTransform RightUI_3;
    [Header("---------------底 部 界 面 UI----------------")]
    [SerializeField]
    protected Button L_B1;
    [SerializeField]
    protected Button L_B2;
    [SerializeField]
    protected Button L_B3;
    //public float[] SliderCountData;
    public enum BulidingType {
        LivingArea,
        TeachingArea,
        LaboratoryBuilding,
        FunctionalArea,
    }
   protected void MoveOutDownUI() {
        if(downUICanOut)
        DownUI.DOPlayBackwards();
    }
    private void OnEnable()
    {
        downUICanOut =false;
    }
    public virtual void OutToCamera_1()
    {
        LiftUI.DOPlayBackwards();
        RightUI.DOPlayBackwards();
        //DownUI.DOPlayBackwards();
        MoveOutDownUI();
    }
 
    
    public virtual void StartToCamera_1()
    {
        L_B1.GetComponent<Button>().interactable = false;
        L_B2.GetComponent<Button>().interactable = true;
        L_B3.GetComponent<Button>().interactable = true;
        UINum = 0;
        Tweener Ltweener = LiftUI.DOLocalMove(LiftUIIn.localPosition, MoveTime);
        Tweener Rtweener = RightUI.DOLocalMove(RightUIIn.localPosition, MoveTime);
        Tweener Dtweener = DownUI.DOLocalMove(DownUIIn.localPosition, MoveTime);
        Ltweener.SetAutoKill(false);
        Rtweener.SetAutoKill(false);
        Dtweener.SetAutoKill(false);

    }
    public virtual void OutToCamera_2()
    {
        Debug.Log("2号菜单开始撤出Camera");
        LiftUI_2.DOPlayBackwards();
        RightUI_2.DOPlayBackwards();
         MoveOutDownUI ();
        Debug.Log("2号菜单结束撤出Camera");
    }

    public virtual void StartToCamera_2()
    {
        L_B2.GetComponent<Button>().interactable = false;
        L_B1.GetComponent<Button>().interactable = true;
        L_B3.GetComponent<Button>().interactable = true;
        UINum = 1;
        Tweener Ltweener2 = LiftUI_2.DOLocalMove(LiftUIIn.localPosition, MoveTime);
        Tweener Rtweener2 = RightUI_2.DOLocalMove(RightUIIn.localPosition, MoveTime);
        Ltweener2.SetAutoKill(false);
        Rtweener2.SetAutoKill(false);


    }
  
    public virtual void OutToCamera_3()
    {
        LiftUI_3.DOPlayBackwards();
        RightUI_3.DOPlayBackwards();
        MoveOutDownUI();
    }

    public virtual void StartToCamera_3()
    {
        L_B3.GetComponent<Button>().interactable = false;
        L_B1.GetComponent<Button>().interactable = true;
        L_B2.GetComponent<Button>().interactable = true;
        UINum = 2;
        Tweener Ltweener3 = LiftUI_3.DOLocalMove(LiftUIIn.localPosition, MoveTime);
        Tweener Rtweener3 = RightUI_3.DOLocalMove(RightUIIn.localPosition, MoveTime);
        Ltweener3.SetAutoKill(false);
        Rtweener3.SetAutoKill(false);
    }

    public virtual void SwitchUI()
    {
        Debug.Log("当前UINum的值是" + UINum);
        if (UINum == 0) OutToCamera_1();
        if (UINum == 1) OutToCamera_2();
        if (UINum == 2) OutToCamera_3();

    }


}

/// <summary>
/// 实现画图表数据点图表
/// </summary>
public interface ChartLinInterFace
{
    void DrawChartline();
}

