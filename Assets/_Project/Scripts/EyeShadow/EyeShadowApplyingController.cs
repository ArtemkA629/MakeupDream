using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EyeShadowApplyingController
{
    private static readonly Vector3 BrushToBlushOffset = new Vector3(180f, 130f, 0f);
    private static readonly float ZBrushRotationOnAnimating = 110f;
    private static readonly float YAnimatedMovingHeight = 20f;
    
    private readonly IInputSystem _inputSystem;
    private readonly AnimationsApplyer _animationsApplyer;
    private readonly EyeShadowConfig[] _eyeShadowConfigs;
    private readonly Button[] _eyeShadowButtonComponents;
    private readonly Transform _brush;
    private readonly Canvas _brushCanvas;
    private readonly Transform _brushTip;
    private readonly RectTransform _eyeShadows;
    private readonly float _eyeShadowApplyingDuration;
    private readonly Vector3 _brushStartPosition;
    
    private bool _isEyeShadowSelected;
    private Image _currentMakeupLeftEyeShadow;
    private Image _currentMakeupRightEyeShadow;
    private Sprite _currentEyeShadowSprite;
    private Coroutine _eyeShadowApplyingRoutine;
    private int _brushSortingOrder;
    
    public EyeShadowApplyingController(IInputSystem inputSystem, AnimationsApplyer animationsApplyer, EyeShadowConfig[] eyeShadowConfigs, Button[] eyeShadowButtonComponents, 
        Transform brush, RectTransform eyeShadows, Image currentMakeupLeftEyeShadow, Image currentMakeupRightEyeShadow, float eyeShadowApplyingDuration)
    {
        _inputSystem = inputSystem;
        _animationsApplyer = animationsApplyer;
        _eyeShadowConfigs = eyeShadowConfigs;
        _eyeShadowButtonComponents = eyeShadowButtonComponents;
        _currentMakeupLeftEyeShadow = currentMakeupLeftEyeShadow;
        _currentMakeupRightEyeShadow = currentMakeupRightEyeShadow;
        _brush = brush;
        _brushCanvas = _brush.GetComponent<Canvas>();
        _brushTip = _brush.GetChild(0);
        _eyeShadows = eyeShadows;
        _eyeShadowApplyingDuration = eyeShadowApplyingDuration;
        _brushStartPosition = _brush.position;
    }
    
    public void Init()
    {
        foreach (var button in _eyeShadowButtonComponents)
            button.onClick.AddListener(() => button.StartCoroutine(ApplyEyeShadowInSet(button)));
    }

    public void Update()
    {
        if ((_isEyeShadowSelected && _inputSystem.IsInputControllerHolding()) == false)
            return;

        _brush.position = _inputSystem.GetInputPosition();
        
        var brushTipTouchedFace = RectTransformUtility.RectangleContainsScreenPoint(_eyeShadows, _brushTip.position);

        if (brushTipTouchedFace && _eyeShadowApplyingRoutine == null)
            _eyeShadowApplyingRoutine = _eyeShadowButtonComponents[0]?.StartCoroutine(ApplyEyeShadowInFace());
    }
    
    private IEnumerator ApplyEyeShadowInSet(Button button)
    {
        if (_isEyeShadowSelected)
            yield break;
        
        _currentEyeShadowSprite = button.GetComponent<Image>().sprite;
        _brushSortingOrder = _brushCanvas.sortingOrder;
        _brushCanvas.sortingOrder = Mathf.Max(button.GetComponentInParent<Canvas>().sortingOrder + 1, _brush.GetComponentInParent<Canvas>().sortingOrder + 1); 
        yield return _animationsApplyer.AnimateBrushTakesMakeup(_brush, button.transform.position + BrushToBlushOffset,
            _eyeShadowApplyingDuration, ZBrushRotationOnAnimating, YAnimatedMovingHeight);
        _isEyeShadowSelected = true;
    }

    private IEnumerator ApplyEyeShadowInFace()
    {
        var eyeShadowsSprites = GetMakeupEyeShadowSprites(_currentEyeShadowSprite);
        _eyeShadowButtonComponents[0]?.StartCoroutine(_animationsApplyer.AnimateMakeupApplying(_brush, _eyeShadows, _eyeShadowApplyingDuration));
        ChangeEyeShadow(_currentMakeupLeftEyeShadow, eyeShadowsSprites[0], out Image newLeftEyeShadow);
        ChangeEyeShadow(_currentMakeupRightEyeShadow, eyeShadowsSprites[1], out Image newRightEyeShadow);
        
        yield return new WaitForSeconds(_eyeShadowApplyingDuration);
        
        Object.Destroy(_currentMakeupLeftEyeShadow.gameObject);
        Object.Destroy(_currentMakeupRightEyeShadow.gameObject);

        _currentMakeupLeftEyeShadow = newLeftEyeShadow;
        _currentMakeupRightEyeShadow = newRightEyeShadow;
        
        _eyeShadowApplyingRoutine = null;
        _isEyeShadowSelected = false;
        _brush.position = _brushStartPosition;
        _brushCanvas.sortingOrder = _brushSortingOrder;
    }

    private void ChangeEyeShadow(Image makeupEyeShadow, Sprite newEyeShadowSprite, out Image newEyeShadow)
    {
        newEyeShadow = Object.Instantiate(makeupEyeShadow, _eyeShadows);
        _eyeShadowButtonComponents[0]?.StartCoroutine(makeupEyeShadow.ChangeImageTo(newEyeShadow, newEyeShadowSprite, _eyeShadowApplyingDuration));
    }
    
    private Sprite[] GetMakeupEyeShadowSprites(Sprite eyeShadowSprite)
    {
        foreach (var eyeShadowConfig in _eyeShadowConfigs)
        {
            if (eyeShadowConfig.EyeShadow == eyeShadowSprite)
                return new[] { eyeShadowConfig.MakeupLeftEyeShadow, eyeShadowConfig.MakeupRightEyeShadow };
        }
        
        Debug.LogError("Cant find blush");
        return null;
    }
}