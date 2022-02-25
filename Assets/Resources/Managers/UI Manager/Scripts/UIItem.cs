using System.Collections;
using UnityEngine.EventSystems;

public class UIItem : UIBehaviour
{
    public virtual IEnumerator Show()
    {
        gameObject.SetActive(true);
        yield return null;
    }

    public virtual IEnumerator Hide()
    {
        gameObject.SetActive(false);
        yield return null;
    }
}
