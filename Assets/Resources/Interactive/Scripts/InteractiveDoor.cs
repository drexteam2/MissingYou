using UnityEngine;

[CreateAssetMenu(fileName = "InteractiveDoor", menuName = "Missing You/Create Interactive Door")]
public class InteractiveDoor : Interactive
{
    public GameObject doorPrefab;
    public string toScene;

    private void Awake()
    {
        interactionType = InteractionType.Door;
    }
}
