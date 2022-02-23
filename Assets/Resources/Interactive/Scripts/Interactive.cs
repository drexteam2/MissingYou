using UnityEngine;

public class Interactive : ScriptableObject
{
    public InteractionType interactionType;
}

public enum InteractionType
{
    Note,
    Door,
    Other,
}