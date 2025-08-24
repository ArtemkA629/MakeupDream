using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : IDisposable
{
    private readonly InputActionReference _touchContact;
    private readonly InputActionReference _touchPosition;
    private readonly float _swipeThreshold;

    private Vector2 _startTouchPosition;
    private bool _isSwiping = false;

    public SwipeDetection(InputActionReference touchContact, InputActionReference touchPosition, float swipeThreshold)
    {
        _touchContact = touchContact;
        _touchPosition = touchPosition;
        _swipeThreshold = swipeThreshold;
    }
    
    public void Init()
    {
        _touchContact.action.Enable();
        _touchPosition.action.Enable();
        
        _touchContact.action.started += StartTouch;
        _touchContact.action.canceled += EndTouch;
    }

    public void Dispose()
    {
        _touchContact.action.started -= StartTouch;
        _touchContact.action.canceled -= EndTouch;
        
        _touchContact.action.Disable();
        _touchPosition.action.Disable();
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        _startTouchPosition = _touchPosition.action.ReadValue<Vector2>();
        _isSwiping = true;
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (_isSwiping == false) 
            return;
        
        Vector2 endTouchPosition = _touchPosition.action.ReadValue<Vector2>();
        Vector2 swipeDelta = endTouchPosition - _startTouchPosition;

        if (Mathf.Abs(swipeDelta.x) > _swipeThreshold)
            EventBus.Publish(new SwipeTakenSignal(swipeDelta.x));
        
        _isSwiping = false;
    }
}