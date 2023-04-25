using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

public class ManagerOfUI : MonoBehaviour, IController
{
    private static ManagerOfUI instance;
    public static ManagerOfUI Instance { get { return instance; } }

    private Transform canvasTrans;

    //private Stack<UI> uiStack = new Stack<UI>();
    private HashSet<UI> uiSet = new HashSet<UI>();

    private const string UI_RESOURCE_PATH = "Prefabs/UI/";
    private readonly string mainPanelName = "TopBarPanel";

    private Character character;
    private const string CHARACTER_NAME = "Character";

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }

        Init();

        character = GameObject.Find(CHARACTER_NAME).GetComponent<Character>();
    }

    /*
    /// <summary>
    /// 隐藏当前UI，显示下一个UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void PushUI<T>() where T : UI
    {
        if (uiStack.Count > 0)
        {
            if (uiStack.Peek() is T) return;
            uiStack.Peek().OnPause();
        }

        if (uiStack.Peek() is T) return;
        var temp = GameObject.Instantiate(Resources.Load(uiResourcePath + nameof(T)));
        UI tempUI = (temp as GameObject).GetComponent<UI>();
        uiStack.Push(tempUI);
        tempUI.OnEnter();
    }

    /// <summary>
    /// 退出当前UI，恢复上一个UI
    /// </summary>
    public void PopUI()
    {
        if (uiStack.Count > 0)
        {
            var tempUI = uiStack.Pop();
            tempUI.OnExit();
            Destroy(tempUI.gameObject);
        }
        if (uiStack.Count > 0)
        {
            uiStack.Peek().OnResume();
        }
    }

    /// <summary>
    /// 重置UI栈，只剩栈底UI
    /// </summary>
    public void ResetUI()
    {
        while (uiStack.Count > 1)
        {
            var tempUI = uiStack.Pop();
            tempUI.OnExit();
            Destroy(tempUI.gameObject);
        }
        uiStack.Peek().OnResume();
    }
    */

    public void Init()
    {
        canvasTrans = this.transform;

        Instantiate<GameObject>(Resources.Load<GameObject>(UI_RESOURCE_PATH + mainPanelName), canvasTrans)
            .GetComponent<UI>()
            .OnShow();
    }

    public UI OpenUI<T>() where T : UI
    {
        foreach (UI u in uiSet)
        {
            if (u is T) return u;
        }
        GameObject gameObject = Resources.Load<GameObject>(UI_RESOURCE_PATH + typeof(T).Name);
        UI temp = Instantiate<GameObject>(gameObject, canvasTrans).GetComponent<UI>();
        uiSet.Add(temp);
        temp.OnShow();
        return temp;
    }

    public void CloseUI<T>() where T : UI
    {
        foreach (UI u in uiSet)
        {
            if(u is T)
            {
                u.OnHide();
                uiSet.Remove(u);
                break;
            }
        }
    }

    public void CloseAllUI()
    {
        foreach (UI u in uiSet)
        {
            u.OnHide();
        }
        uiSet.Clear();
    }

    public bool FindUI<T>() where T : UI
    {
        foreach(UI u in uiSet)
        {
            if (u is T) return true;
        }
        return false;
    }

    public void ControlCamera(Vector3 newBodyPos, Quaternion newBodyRota,  Quaternion newHeadRota, float newDistance, float duration, bool AutoControl)
    {
        character.AdjustBodyPosition(newBodyPos, duration, AutoControl);
        character.AdjustBodyRotation(newBodyRota, duration);
        character.AdjustHeadRotation(newHeadRota, duration);
        character.AdjustCameraDistance(newDistance, duration);
    }

    public IArchitecture GetArchitecture()
    {
        return DigitalRENAI.Interface;
    }
}
