using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour, IController
{
    //public abstract void OnEnter();
    //public abstract void OnPause();
    //public abstract void OnResume();
    //public abstract void OnExit();
    protected RectTransform rectTransform;

    protected void Awake()
    {
        rectTransform = transform as RectTransform;
    }

    public abstract void OnShow();
    public abstract void OnHide();

    protected abstract void UpdateView();

    public IArchitecture GetArchitecture()
    {
        return DigitalRENAI.Interface;
    }
}
