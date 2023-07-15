using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

public class UIManager : MonoBehaviour, IController
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    private Transform canvasTrans;

    private HashSet<UIBase> uiSet = new HashSet<UIBase>();

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

    public void Init()
    {
        canvasTrans = this.transform;

        Instantiate<GameObject>(Resources.Load<GameObject>(UI_RESOURCE_PATH + mainPanelName), canvasTrans)
            .GetComponent<UIBase>()
            .OnShow();
    }

    public UIBase OpenUI<T>() where T : UIBase
    {
        foreach (UIBase u in uiSet)
        {
            if (u is T) return u;
        }
        GameObject gameObject = Resources.Load<GameObject>(UI_RESOURCE_PATH + typeof(T).Name);
        UIBase temp = Instantiate<GameObject>(gameObject, canvasTrans).GetComponent<UIBase>();
        uiSet.Add(temp);
        temp.OnShow();
        return temp;
    }

    public void CloseUI<T>() where T : UIBase
    {
        foreach (UIBase u in uiSet)
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
        foreach (UIBase u in uiSet)
        {
            u.OnHide();
        }
        uiSet.Clear();
    }

    public bool FindUI<T>() where T : UIBase
    {
        foreach(UIBase u in uiSet)
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
