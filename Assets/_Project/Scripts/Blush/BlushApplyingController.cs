using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlushApplyingController
{
    private static readonly Vector3 BrushToBlushOffset = new Vector3(180f, 130f, 0f);
    private static readonly float ZBrushRotationOnAnimating = 110f;
    private static readonly float YAnimatedMovingHeight = 20f;
    
    private readonly IInputSystem _inputSystem;
    private readonly AnimationsApplyer _animationsApplyer;
    private readonly BlushConfig[] _blushConfigs;
    private readonly Button[] _blushButtonComponents;
    private readonly Transform _brush;
    private readonly Canvas _brushCanvas;
    private readonly Transform _brushTip;
    private readonly RectTransform _face;
    private readonly float _blushApplyingDuration;
    private readonly Vector3 _brushStartPosition;
    
    private bool _isBlushSelected;
    private Image _currentMakeupBlush;
    private Sprite _currentBlushSprite;
    private Coroutine _blushApplyingRoutine;
    private int _brushSortingOrder;
    
    public BlushApplyingController(IInputSystem inputSystem, AnimationsApplyer animationsApplyer, BlushConfig[] blushConfigs, Button[] blushButtonComponents, 
        Transform brush, RectTransform face, Image currentMakeupBlush, float blushApplyingDuration)
    {
        _inputSystem = inputSystem;
        _animationsApplyer = animationsApplyer;
        _blushConfigs = blushConfigs;
        _blushButtonComponents = blushButtonComponents;
        _currentMakeupBlush = currentMakeupBlush;
        _brush = brush;
        _brushCanvas = _brush.GetComponent<Canvas>();
        _brushTip = _brush.GetChild(0);
        _face = face;
        _blushApplyingDuration = blushApplyingDuration;
        _brushStartPosition = _brush.position;
    }
    
    public void Init()
    {
        foreach (var button in _blushButtonComponents)
            button.onClick.AddListener(() => _blushButtonComponents[0]?.StartCoroutine(ApplyBlushInSet(button)));
    }

    public void Update()
    {
        if ((_isBlushSelected && _inputSystem.IsInputControllerHolding()) == false)
            return;

        _brush.position = _inputSystem.GetInputPosition();
        
        var brushTipTouchedFace = RectTransformUtility.RectangleContainsScreenPoint(_face, _brushTip.position);

        if (brushTipTouchedFace)
            _blushApplyingRoutine ??= _blushButtonComponents[0]?.StartCoroutine(ApplyBlushInFace());
    }
    
    private IEnumerator ApplyBlushInSet(Button button)
    {
        if (_isBlushSelected)
            yield break;

        _currentBlushSprite = button.GetComponent<Image>().sprite;
        _brushSortingOrder = _brushCanvas.sortingOrder;
        _brushCanvas.sortingOrder = Mathf.Max(button.GetComponentInParent<Canvas>().sortingOrder + 1, _brush.GetComponentInParent<Canvas>().sortingOrder + 1); 
        yield return _animationsApplyer.AnimateBrushTakesMakeup(_brush, button.transform.position + BrushToBlushOffset,
            _blushApplyingDuration, ZBrushRotationOnAnimating, YAnimatedMovingHeight);
        
        _isBlushSelected = true;
    }

    private IEnumerator ApplyBlushInFace()
    {
        var newBlush = Object.Instantiate(_currentMakeupBlush, _face);
        var newBlushSprite = GetMakeupBlushSprite(_currentBlushSprite);
        _blushButtonComponents[0]?.StartCoroutine(_animationsApplyer.AnimateMakeupApplying(_brush, newBlush.GetComponent<RectTransform>(),
            _blushApplyingDuration));
        yield return _currentMakeupBlush.ChangeImageTo(newBlush, newBlushSprite, _blushApplyingDuration);
        
        Object.Destroy(_currentMakeupBlush.gameObject);
        _currentMakeupBlush = newBlush;
        _blushApplyingRoutine = null;
        _isBlushSelected = false;
        _brush.position = _brushStartPosition;
        _brushCanvas.sortingOrder = _brushSortingOrder;
    }
    
    private Sprite GetMakeupBlushSprite(Sprite blushSprite)
    {
        foreach (var blushConfig in _blushConfigs)
        {
            if (blushConfig.Blush == blushSprite)
                return blushConfig.MakeupBlush;
        }
        
        Debug.LogError("Cant find blush");
        return null;
    }
}