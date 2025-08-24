using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EyeShadowConfig", fileName = "EyeShadowConfig")]
public class EyeShadowConfig : ScriptableObject
{
    [field: SerializeField] public Sprite EyeShadow { get; private set; }
    [field: SerializeField] public Sprite MakeupLeftEyeShadow { get; private set; }
    [field: SerializeField] public Sprite MakeupRightEyeShadow { get; private set; }
}