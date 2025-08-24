using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetectionInstaller : MonoBehaviour
{
    [SerializeField] private InputActionReference _touchContact;
    [SerializeField] private InputActionReference _touchPosition;
    [SerializeField] private InputActionReference _touchDelta;
    [SerializeField] private float _swipeThreshold = 100f;

    private SwipeDetection _swipeDetection;

    private void OnDisable()
    {
        _swipeDetection.Dispose();
    }

    public void Install()
    {
        _swipeDetection = new(_touchContact, _touchPosition, _swipeThreshold);
        _swipeDetection.Init();
    }
}