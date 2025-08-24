using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LipsticksApplyingController
{
    private readonly IInputSystem _inputSystem;
    private readonly AnimationsApplyer _animationsApplyer;
    private readonly LipstickConfig[] _lipstickConfigs;
    private readonly RectTransform _face;
    private readonly float _lipstickApplyingDuration;
    
    private Image _currentLipstickMakeup;
    private Image _currentLipstick;
    private RectTransform _currentLipstickTip;
    private Vector3 _currentLipstickPositionAtStart;
    private bool _isLipstickSelected;
    private Coroutine _lipstickApplyingRoutine;

    public LipsticksApplyingController(IInputSystem inputSystem, AnimationsApplyer animationsApplyer, LipstickConfig[] lipstickConfigs,
        RectTransform face, Image lipstickMakeup, float lipstickApplyingDuration)
    {
        _inputSystem = inputSystem;
        _animationsApplyer = animationsApplyer;
        _lipstickConfigs = lipstickConfigs;
        _face = face;
        _currentLipstickMakeup = lipstickMakeup;
        _lipstickApplyingDuration = lipstickApplyingDuration;
    }
    
    public void Update()
    {
        if (_inputSystem.IsInputControllerHolding() == false)
        {
            if (_isLipstickSelected && _lipstickApplyingRoutine == null)
                ResetLipstick();

            return;
        }

        if (_lipstickApplyingRoutine != null)
            return;
        
        var inputPosition = _inputSystem.GetInputPosition();

        if (_isLipstickSelected)
        {
            _currentLipstick.transform.position = inputPosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(_currentLipstickTip, _currentLipstickMakeup.transform.position))
                _lipstickApplyingRoutine = _currentLipstick.StartCoroutine(ChangeLipstickMakeup());
        }
        else
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = inputPosition;
            System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            
            if (results.Count > 0 && results[0].gameObject.TryGetComponent(out Lipstick lipstick))
            {
                _currentLipstick = lipstick.GetComponent<Image>();
                _currentLipstickTip = _currentLipstick.transform.GetChild(0).GetComponent<RectTransform>();
                _currentLipstickPositionAtStart = _currentLipstick.transform.position;
                _isLipstickSelected = true;
            }
        }
    }

    private IEnumerator ChangeLipstickMakeup()
    {
        var newLipstickMakeup = Object.Instantiate(_currentLipstickMakeup, _face);
        var newLipstickMakeupSprite = newLipstickMakeup.sprite = GetMakeupLipstickSprite(_currentLipstick.sprite);
        _currentLipstick.StartCoroutine(_animationsApplyer.AnimateMakeupApplying(
            _currentLipstick.transform, 
            newLipstickMakeup.GetComponent<RectTransform>(), 
            _lipstickApplyingDuration,
            _currentLipstick.GetComponent<RectTransform>().sizeDelta.y / 2));
        
        yield return _currentLipstickMakeup.ChangeImageTo(newLipstickMakeup, newLipstickMakeupSprite,
            _lipstickApplyingDuration);
        
        Object.Destroy(_currentLipstickMakeup.gameObject);
        _currentLipstickMakeup = newLipstickMakeup;
        _lipstickApplyingRoutine = null;
        ResetLipstick();
    }
    
    private Sprite GetMakeupLipstickSprite(Sprite lipstickSprite)
    {
        foreach (var lipstickConfig in _lipstickConfigs)
        {
            if (lipstickConfig.Lipstick == lipstickSprite)
                return lipstickConfig.MakeupLipstick;
        }
        
        Debug.LogError("Cant find blush");
        return null;
    }

    private void ResetLipstick()
    {
        _currentLipstick.transform.position = _currentLipstickPositionAtStart;
        _currentLipstick = null;
        _currentLipstickTip = null;
        _currentLipstickPositionAtStart = default;
        _isLipstickSelected = false;
    }
}