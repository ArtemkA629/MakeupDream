using UnityEngine;

public interface IInputSystem
{
    bool IsInputControllerHolding();
    Vector3 GetInputPosition();
}