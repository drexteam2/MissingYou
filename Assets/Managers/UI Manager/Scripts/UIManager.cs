using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : UIBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private List<UIBehaviour> _uis = new List<UIBehaviour>();

    protected override void Awake()
    {
        _instance ??= this;
        DontDestroyOnLoad(_instance);
    }
}
