using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : UIBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private Dictionary<string, UIItem> _activeUIs = new Dictionary<string, UIItem>();

    private Dictionary<string, GameObject> _uis = new Dictionary<string, GameObject>();

    protected override void Awake()
    {
        _instance ??= this;
        DontDestroyOnLoad(_instance.gameObject);

        _uis["Fader"] = Resources.Load<GameObject>("Managers/UI Manager/Prefabs/Fader");
        _uis["Health Bar"] = Resources.Load<GameObject>("Managers/UI Manager/Prefabs/Health Bar");
        _uis["Note Background"] = Resources.Load<GameObject>("Managers/UI Manager/Prefabs/Note Background");
    }

    public UIItem InstanceUI(string uiName)
    {
        if (!_uis.ContainsKey(uiName))
        {
            Debug.LogError($"Could not instance UI {uiName} that is not in UI Dictionary.");
            return null;
        }

        if (_activeUIs.ContainsKey(uiName))
        {
            Debug.Log($"UI {uiName} already exists, fetching that.");
            return _activeUIs[uiName];
        }

        Debug.Log($"Instantiating new UI {uiName}.");
        GameObject ui = Instantiate(_uis[uiName], transform);
        _activeUIs.Add(uiName, ui.GetComponent<UIItem>());

        return ui.GetComponent<UIItem>();
    }

    public UIItem GetUI(string uiName)
    {
        if (_activeUIs.ContainsKey(uiName))
        {
            Debug.Log($"Fetching existing UI {uiName}.");
            return _activeUIs[uiName];
        }
        
        if (_uis.ContainsKey(uiName))
        {
            Debug.Log($"Could not get UI {uiName} that doesn't exist yet, but it is in UI Dictionary, instantiating a new one.");
            return InstanceUI(uiName);
        }

        Debug.Log($"Could not get UI {uiName} that is not in UI Dictionary.");
        return null;
    }

    public void ShowUI(string uiName)
    {
        UIItem ui = GetUI(uiName);
        if (ui == null)
        {
            Debug.LogError($"Could not show {uiName}; UI does not exist.");
            return;
        }

        Debug.Log($"Showing UI {uiName}.");
        StartCoroutine(ui.Show());
    }

    public void HideUI(string uiName)
    {
        UIItem ui = GetUI(uiName);
        if (ui == null)
        {
            Debug.LogError($"Could not hide {uiName}; UI does not exist.");
            return;
        }

        Debug.Log($"Hiding UI {uiName}.");
        StartCoroutine(ui.Hide());
    }

    public void FreeUI(string uiName)
    {
        if (!_activeUIs.ContainsKey(uiName))
        {
            Debug.LogError($"Could not free {uiName}; It does not exist.");
            return;
        }

        Debug.Log($"Freeing UI {uiName}.");
        _uis.Remove(uiName);
        Destroy(_activeUIs[uiName]);
    }
}
