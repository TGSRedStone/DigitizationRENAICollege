using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public abstract class BuildingsUIBase : MonoBehaviour
{
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
    private Vector3 LiftRectVect;
    private Vector3 RigthRectVect;
    private Vector3 DownRectVect;
    [Tooltip("动画的移动时间")]
    public float MoveTime = 0.5f;
    private void Awake()
    {
        // SetUIRectTransorm();
    }
    private void Start()
    {


    }
    private void OnEnable()
    {
        StartToCamera();
    }

    //public virtual void SetUIRectTransorm()
    //{
    //    if (LiftUI == null)
    //    {
    //        LiftUI = this.transform.GetChild(0).GetComponent<RectTransform>();
    //    }
    //    if (RightUI == null)
    //    {
    //        RightUI = this.transform.GetChild(1).GetComponent<RectTransform>();
    //    }
    //    if (DownUI == null)
    //    {
    //        RightUI = this.transform.GetChild(2).GetComponent<RectTransform>();
    //    }
    //    if (LiftUIOut == null)
    //    {
    //        LiftUI = this.transform.GetChild(3).GetComponent<RectTransform>();
    //    }
    //    if (RightUIOut == null)
    //    {
    //        RightUI = this.transform.GetChild(4).GetComponent<RectTransform>();
    //    }
    //    if (DownUIOut == null)
    //    {
    //        RightUI = this.transform.GetChild(5).GetComponent<RectTransform>();
    //    }
    //}
    private void Update()
    {

    }
    public  void OutToCamera()
    {
        LiftUI.DOPlayBackwards();
        RightUI.DOPlayBackwards();
        DownUI.DOPlayBackwards();

    }
    public  void StartToCamera()
    {
        LiftUI.DOLocalMove(LiftUIIn.localPosition, MoveTime);
        RightUI.DOLocalMove(RightUIIn.localPosition, MoveTime);
        DownUI.DOLocalMove(DownUIIn.localPosition, MoveTime);
      
    }
}
