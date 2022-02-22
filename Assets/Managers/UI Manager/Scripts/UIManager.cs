using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : UIBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private Dictionary<string, UIItem> _uis = new Dictionary<string, UIItem>();

    protected override void Awake()
    {
        _instance ??= this;
        DontDestroyOnLoad(_instance);

        foreach (Transform childUI in transform)
        {
            _uis.Add(childUI.name, childUI.GetComponent<UIItem>());
        }
    }

    public UIItem ShowUI(string uiName)
    {
        if (!_uis.ContainsKey(uiName))
        {
            Debug.LogError("Could not show; UI does not exist.");
            return null;
        }

        UIItem ui = _uis[uiName];
        ui.Show();

        return ui;
    }

    public UIItem HideUI(string uiName)
    {
        if (!_uis.ContainsKey(uiName))
        {
            Debug.LogError("Could not hide; UI does not exist.");
            return null;
        }

        UIItem ui = _uis[uiName];
        ui.Hide();

        return ui;
    }
}
