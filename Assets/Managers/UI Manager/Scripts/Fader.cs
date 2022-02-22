using UnityEngine;

public class Fader : UIItem
{
    private Animator _anim;

    protected override void Awake()
    {
        _anim ??= GetComponent<Animator>();
    }

    public override void Show()
    {
        base.Show();
        _anim.Play("Fade In");
    }

    public override void Hide()
    {
        _anim.Play("Fade Out");
    }
}
