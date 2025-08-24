using UnityEngine;
using UnityEngine.UI;

public class LipstickApplyingInstaller : MonoBehaviour
{
    [SerializeField] private LipstickConfig[] _lipstickConfigs;
    [SerializeField] private RectTransform _face;
    [SerializeField] private Image _lipstickMakeup;
    [SerializeField] private float _lipstickApplyingDuration;

    private LipsticksApplyingController _lipsticksApplyingController;
    
    private void Update()
    {
        _lipsticksApplyingController.Update();
    }
    
    public void Install(IInputSystem inputSystem, AnimationsApplyer animationsApplyer)
    {
        _lipsticksApplyingController = new(inputSystem, animationsApplyer, _lipstickConfigs, _face, _lipstickMakeup, _lipstickApplyingDuration);
    }
}