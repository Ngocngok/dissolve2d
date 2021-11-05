using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager>
{
    [SerializeField]
    public Canvas Root;

    [SerializeField]
    Camera MainCamera;

    protected override void Awake()
    {
        DestroyChildren(LayerScreen.transform);
        DestroyChildren(LayerPopup.transform);
        DestroyChildren(LayerNotify.transform);

        base.Awake();
    }

    public void ReloadCamera()
    {
        if (MainCamera == null)
            MainCamera = Camera.main;

        Root.worldCamera = MainCamera;
    }

    [SerializeField]
    public Canvas LayerUI;

    [SerializeField]
    public Canvas LayerScreen;

    [SerializeField]
    public Canvas LayerPopup;

    [SerializeField]
    public Canvas LayerNotify;

    [SerializeField]
    public Canvas LayerLoading;

    [SerializeField]
    PanelInstance Prefabs;

    private Dictionary<UI_PANEL, UIPanel> initiedPanels = new Dictionary<UI_PANEL, UIPanel>();

    private List<UIPanel> showingPopups = new List<UIPanel>();
    private List<UIPanel> showingNotifications = new List<UIPanel>();

    private Queue<Action> QueuePopup = new Queue<Action>();

    public void Init()
    {
        DestroyChildren(LayerScreen.transform);
        DestroyChildren(LayerPopup.transform);
        DestroyChildren(LayerNotify.transform);
        ReloadCamera();

        EventGlobalManager.Instance.OnStartLoadScene.AddListener(StartLoading);
        EventGlobalManager.Instance.OnFinishLoadScene.AddListener(ReloadCamera);
    }

    public UIPanel NewPanel(UI_PANEL id)
    {
        PANEL_TYPE type = id.ToString().GetPanelType();

        UIPanel newPanel = null;
        if (initiedPanels.ContainsKey(id))
            newPanel = initiedPanels[id];
        else
        {
            newPanel = Instantiate(GetPrefab(id), GetRootByType(type).transform);
            initiedPanels.Add(id, newPanel);
        }

        if (type == PANEL_TYPE.POPUP)
        {
            if (showingPopups.Contains(newPanel))
                showingPopups.Remove(newPanel);
            
                showingPopups.Add(newPanel);
        }
        else if (type == PANEL_TYPE.NOTIFICATION)
        {
            if (!showingNotifications.Contains(newPanel))
                showingNotifications.Add(newPanel);
        }
        else
        {
            if (GetCurrentScreen() != null && GetCurrentScreen().gameObject.activeSelf)
                GetCurrentScreen().Close(false);

            if (ScreenStack.Contains(newPanel))
                ScreenStack = MakeElementToTopStack(newPanel, ScreenStack);
            else
                ScreenStack.Push(newPanel);
        }

        newPanel.transform.SetAsLastSibling();
        newPanel.gameObject.SetActive(true);

        return newPanel;
    }

    Stack<UIPanel> ScreenStack = new Stack<UIPanel>();

    public UIPanel GetCurrentScreen()
    {
        if (ScreenStack.Count == 0)
            return null;

        return ScreenStack.Peek();
    }

    public void GoBackLastScreen()
    {
        ScreenStack.Pop().Close(false);

        if (GetCurrentScreen() == null || GetCurrentScreen().GetID() == UI_PANEL.MainScreen)
        {
            MainScreen.Show();
        }
        else
        {
            UIPanel newPanel = NewPanel(GetCurrentScreen().GetID());
            newPanel.OnAppear();
        }
    }

    public void ClearGUI()
    {
        foreach (var pair in initiedPanels)
        {
            pair.Value.CloseImmediately();
        }
    }

    public void Dismiss(UIPanel panel)
    {
        showingPopups.Remove(panel);
        showingNotifications.Remove(panel);

        if (GetCurrentScreen() == panel && GetCurrentScreen().GetID() != UI_PANEL.MainScreen)
            MainScreen.Show();
    }

    public void DismissTopPopup()
    {
        UIPanel topPanel = GetTopPopup();
        if (topPanel == null)
            return;

        topPanel.Close();
        Dismiss(topPanel);
    }

    public void DismissPanelByID(UI_PANEL id)
    {
        UIPanel panel = GetPanel(id);
        if (panel == null)
            return;

        panel.Close();
        Dismiss(panel);
    }

    #region Loading
    public UILoading LoadingUI;

    public void StartLoading()
    {
        LoadingUI.gameObject.SetActive(true);
    }

    public void FinishLoading()
    {
        LoadingUI.gameObject.SetActive(false);
    }

    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape))
    //     {
    //
    //         UIPanel topPanel = GetTopPopup();
    //         if (topPanel == null)
    //             topPanel = GetCurrentScreen();
    //
    //         if (topPanel == null)
    //             return;
    //
    //         topPanel.Close();
    //     }
    // }

    public void CheckPopupQueue()
    {
        if (showingPopups.Count == 0 && QueuePopup.Count > 0)
        {
            if (QueuePopup.Peek() != null)
                QueuePopup.Dequeue().Invoke();
        }
    }

    public void AddPopupQueue(Action action)
    {
        QueuePopup.Enqueue(action);

        CheckPopupQueue();
    }
    #endregion

    #region Utilities
    public UIPanel GetTopPopup()
    {
        if (showingPopups.Count == 0)
            return null;

        return showingPopups.GetLast();
    }

    UIPanel GetPrefab(UI_PANEL id)
    {
        if (Prefabs == null)
            return null;

        return Prefabs.Instances.FindLast(e => e.GetID().Equals(id));
    }

    Canvas GetRootByType(PANEL_TYPE type)
    {
        switch (type)
        {
            case PANEL_TYPE.UI:
                return LayerUI;
            case PANEL_TYPE.SCREEN:
                return LayerScreen;
            case PANEL_TYPE.POPUP:
                return LayerPopup;
            case PANEL_TYPE.NOTIFICATION:
                return LayerNotify;
            case PANEL_TYPE.LOADING:
                return LayerLoading;
        }

        return null;
    }

    public bool HasElementAtMousePos()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        bool result = raysastResults.Any(x => x.gameObject.layer == LayerMask.NameToLayer("UI"));
        return result;
    }

    public static IEnumerator TextAnim(UILabel label, long oldValue, long newValue)
    {
        long currentValue = oldValue;
        float speed = (newValue - oldValue) / 0.3f * Time.fixedDeltaTime / 2;
        if (Mathf.Abs(speed) > 1)
        {
            float currentProgress = 0f;
            while (currentProgress < 0.3f)
            {
                yield return new WaitForFixedUpdate();
                currentProgress += Time.fixedDeltaTime;
                currentValue += (long) speed;
                label.text = currentValue.ToFormatString();
            }
        }

        label.text = newValue.ToFormatString();
    }

    public static IEnumerator ProgressAnim(Image bar, float oldValue, float newValue)
    {
        float currentValue = oldValue;
        float speed = (newValue - oldValue) * Time.fixedDeltaTime;

        float currentProgress = 0f;
        while (currentProgress < 1f)
        {
            yield return new WaitForFixedUpdate();
            currentProgress += Time.fixedDeltaTime;
            currentValue += speed;
            bar.fillAmount = currentValue;
        }

        bar.fillAmount = newValue;
    }

    public void DestroyChildren(Transform transform)
    {
        int totalChild = transform.childCount;

        if (totalChild == 0)
            return;

        for (int i = totalChild - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public UIPanel GetPanel(UI_PANEL type)
    {
        if (initiedPanels.ContainsKey(type))
        {
            return initiedPanels[type];
        }

        return null;
    }

    public Stack<UIPanel> MakeElementToTopStack(UIPanel objectTop, Stack<UIPanel> stack)
    {
        UIPanel[] extraPanel = stack.ToArray();
        for (int i = 0; i < extraPanel.Length; i++)
        {
            if (extraPanel[i] == objectTop)
            {
                for (int ii = i; ii > 0; ii--)
                {
                    extraPanel[ii] = extraPanel[ii - 1];
                }

                extraPanel[0] = objectTop;
            }
        }

        Array.Reverse(extraPanel);
        return new Stack<UIPanel>(extraPanel);
    }

    public void ShowGUI()
    {
        LayerPopup.enabled = true;
        LayerScreen.enabled = true;
    }

    public void HideGUI()
    {
        LayerPopup.enabled = false;
        LayerScreen.enabled = false;
    }

    public RectTransform RootRect;
    #endregion
}