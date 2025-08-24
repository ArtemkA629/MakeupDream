using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BlushConfig", fileName = "BlushConfig")]
public class BlushConfig : ScriptableObject
{
    [field: SerializeField] public Sprite Blush { get; private set; }
    [field: SerializeField] public Sprite MakeupBlush { get; private set; }
}