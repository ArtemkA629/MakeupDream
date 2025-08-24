using UnityEngine;

public class ComputerInputSystem : IInputSystem
{
    public bool IsInputControllerHolding() => Input.GetMouseButton(0);

    public Vector3 GetInputPosition() => Input.mousePosition;
}