using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Material _fillMat;
    private RectTransform _rect;

    private void Awake()
    {
        _fillMat = transform.Find("Fill").GetComponent<Image>().material;
        _rect ??= GetComponent<RectTransform>();

        PlayerController.Instance.healthChanged.AddListener(HealthChanged);
    }

    private void HealthChanged(int maxHealth, int currentHealth)
    {
        _fillMat.SetFloat("_Progress", (float)currentHealth / maxHealth);
        _rect.sizeDelta = new Vector2(maxHealth * 5, _rect.sizeDelta.y);
    }
}
