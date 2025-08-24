using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/LipstickConfig", fileName = "LipstickConfig")]
public class LipstickConfig : ScriptableObject
{
    [field: SerializeField] public Sprite Lipstick { get; private set; }
    [field: SerializeField] public Sprite MakeupLipstick { get; private set; }
}