using UnityEngine;

[CreateAssetMenu(fileName = "InteractiveNote", menuName = "Missing You/Create Interactive Note")]
public class InteractiveNote : Interactive
{
    public string interactText;

    private void Awake()
    {
        interactionType = InteractionType.Note;
    }
}
