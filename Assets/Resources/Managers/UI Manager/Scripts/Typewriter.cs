using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Typewriter : UIItem
{
    public UnityEvent finishedTyping;

    public string text;
    public float typingSpeed;

    private string _targetText;

    private TextMeshProUGUI _tmp;

    protected override void Awake()
    {
        _tmp ??= GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator StartTyping(string targetText)
    {
        _targetText = targetText;
        for (int letter = 1; letter < _targetText.Length; letter++)
        {
            string textPart = _targetText.Substring(0, letter);
            _tmp.text = textPart;
            yield return new WaitForSeconds(1.0f / typingSpeed);
        }

        finishedTyping.Invoke();
    }

    public void SkipTyping()
    {
        _tmp.text = _targetText;
        finishedTyping.Invoke();
    }
}
