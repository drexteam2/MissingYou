using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Fader : UIItem
{
    public UnityEvent fadedIn;
    public UnityEvent fadedOut;

    private Animator _anim;

    protected override void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public override IEnumerator Show()
    {
        _anim.Play("Fade In");
        yield return new WaitForSeconds(_anim.runtimeAnimatorController.animationClips
            .First(clip => clip.name == "Fade In").length);
        fadedIn.Invoke();
    }

    public override IEnumerator Hide()
    {
        _anim.Play("Fade Out");
        yield return new WaitForSeconds(_anim.runtimeAnimatorController.animationClips
            .First(clip => clip.name == "Fade Out").length);
        fadedOut.Invoke();
    }
}
