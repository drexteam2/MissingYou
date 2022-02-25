using System.Collections;
using System.Linq;
using UnityEngine;

public class NoteBackground : UIItem
{
    private Animator _anim;

    protected override void Awake()
    {
        _anim ??= GetComponent<Animator>();
    }

    public override IEnumerator Show()
    {
        yield return base.Show();
        _anim.Play("Show Text");
    }

    public override IEnumerator Hide()
    {
        _anim.Play("Hide Text");
        //Debug.Log("Show text? " + _anim.GetCurrentAnimatorStateInfo(0).IsName("Show Text"));
        yield return new WaitForSeconds(_anim.runtimeAnimatorController.animationClips
            .First(clip => clip.name == "Hide Text").length);
        yield return base.Hide();
    }
}
