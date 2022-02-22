using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshPro))]
public class Typewriter : UIBehaviour
{
    public string text;
    public float typingSpeed;

    private TextMeshPro _tmp;

    protected override void Awake()
    {
        _tmp ??= GetComponent<TextMeshPro>();
    }

    public IEnumerator StartTyping(string targetText)
    {
        for (int letter = 1; letter < targetText.Length; letter++)
        {
            string temp = targetText.Substring(0, letter);
            _tmp.text = temp;
            yield return new WaitForSeconds(1.0f / typingSpeed);
        }
    }

    public void SkipTyping()
    {
        _tmp.text = text;
    }
}
