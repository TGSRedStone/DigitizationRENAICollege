using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour, IController
{
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
