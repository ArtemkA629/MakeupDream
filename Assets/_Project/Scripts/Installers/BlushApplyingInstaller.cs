using UnityEngine;
using UnityEngine.UI;

public class BlushApplyingInstaller : MonoBehaviour
{
    [SerializeField] private BlushConfig[] _blushConfigs;
    [SerializeField] private Button[] _blushButtonComponents;
    [SerializeField] private Image _currentMakeupBlush;
    [SerializeField] private Transform _brush;
    [SerializeField] private RectTransform _face;
    [SerializeField] private float _blushApplyingDuration;
    
    private BlushApplyingController _blushApplyingController;

    private void Update()
    {
        _blushApplyingController.Update();
    }
    
    public void Install(IInputSystem inputSystem, AnimationsApplyer animationsApplyer)
    {
        _blushApplyingController = new(inputSystem, animationsApplyer, _blushConfigs, _blushButtonComponents, _brush, _face, _currentMakeupBlush, _blushApplyingDuration);
        _blushApplyingController.Init();
    }
}