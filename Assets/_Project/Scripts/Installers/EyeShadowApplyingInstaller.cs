using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EyeShadowApplyingInstaller : MonoBehaviour
{
    [SerializeField] private EyeShadowConfig[] _eyeShadowConfigs;
    [SerializeField] private Button[] _eyeShadowButtonComponents;
    [SerializeField] private Image _currentMakeupLeftEyeShadow;
    [SerializeField] private Image _currentMakeupRightEyeShadow;
    [SerializeField] private Transform _brush;
    [SerializeField] private RectTransform _eyeShadows;
    [SerializeField] private float _eyeShadowApplyingDuration;
    
    private EyeShadowApplyingController _eyeShadowApplyingController;

    private void Update()
    {
        _eyeShadowApplyingController.Update();
    }
    
    public void Install(IInputSystem inputSystem, AnimationsApplyer animationsApplyer)
    {
        _eyeShadowApplyingController = new(inputSystem, animationsApplyer, _eyeShadowConfigs, _eyeShadowButtonComponents, _brush, _eyeShadows, 
            _currentMakeupLeftEyeShadow, _currentMakeupRightEyeShadow, _eyeShadowApplyingDuration);
        _eyeShadowApplyingController.Init();
    }
}