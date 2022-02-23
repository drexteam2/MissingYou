using UnityEngine;

[CreateAssetMenu(fileName = "InteractiveDoor", menuName = "Missing You/Create Interactive Door")]
public class InteractiveDoor : Interactive
{
    public GameObject doorPrefab;

    private void Awake()
    {
        interactionType = InteractionType.Door;
    }
}
