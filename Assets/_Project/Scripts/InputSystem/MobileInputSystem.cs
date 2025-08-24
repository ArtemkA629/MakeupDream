using UnityEngine;

public class MobileInputSystem : IInputSystem
{
    public bool IsInputControllerHolding() => Input.touchCount > 0;
    public Vector3 GetInputPosition() => Input.GetTouch(0).position;
}