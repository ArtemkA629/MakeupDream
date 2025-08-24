using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreamApplyingController
{
    private readonly IInputSystem _inputSystem;
    private readonly Button _creamButton;
    private readonly Image _acne;
    private readonly float _acneCleaningDuration;
    
    private bool _isCreamSelected;

    public CreamApplyingController(IInputSystem inputSystem, Button creamButton, Image acne, float acneCleaningDuration)
    {
        _inputSystem = inputSystem;
        _creamButton = creamButton;
        _acne = acne;
        _acneCleaningDuration = acneCleaningDuration;
    }
    
    public void Init()
    {
        _creamButton.onClick.AddListener(ApplyCream);
    }

    public void Update()
    {
        if ((_isCreamSelected && _inputSystem.IsInputControllerHolding()) == false)
            return;

        if (Mathf.Approximately(_acne.color.a, 0f))
            return;
        
        var zoneTouched = RectTransformUtility.RectangleContainsScreenPoint(
            _acne.GetComponent<RectTransform>(), 
            _inputSystem.GetInputPosition());

        if (zoneTouched)
            _creamButton.StartCoroutine(CleanAcne());
    }
    
    private void ApplyCream()
    {
        if (_isCreamSelected)
            return;
        
        _isCreamSelected = true;
    }

    private IEnumerator CleanAcne()
    {
        if (Mathf.Approximately(_acne.color.a, 0f))
        {
            _isCreamSelected = false;
            yield break;
        }
        
        _acne.CrossFadeAlpha(0f, _acneCleaningDuration, false);
        yield return new WaitForSeconds(_acneCleaningDuration);
        _isCreamSelected = false;
    }
}
