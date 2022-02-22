using HutongGames.PlayMaker.Actions;
using UnityEngine.EventSystems;

public class UIItem : UIBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
