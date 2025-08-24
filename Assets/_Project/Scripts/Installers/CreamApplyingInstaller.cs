using System;
using UnityEngine;
using UnityEngine.UI;

public class CreamApplyingInstaller : MonoBehaviour
{
    [SerializeField] private Button _creamButton;
    [SerializeField] private Image _acne;
    [SerializeField] private float _acneCleaningDuration;
    
    private CreamApplyingController _creamApplyingController;
    
    private void Update()
    {
        _creamApplyingController.Update();
    }
    
    public void Install(IInputSystem inputSystem)
    {
        _creamApplyingController = new(inputSystem, _creamButton, _acne, _acneCleaningDuration);
        _creamApplyingController.Init();
    }
}