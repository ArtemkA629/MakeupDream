using UnityEngine;
using UnityEngine.Serialization;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private CreamApplyingInstaller _creamApplyingInstaller;
    [SerializeField] private BlushApplyingInstaller _blushApplyingInstaller;
    [SerializeField] private LipstickApplyingInstaller _lipstickApplyingInstaller;
    [SerializeField] private EyeShadowApplyingInstaller _eyeShadowApplyingInstaller;
    [SerializeField] private PagesFlippingInstaller _pagesFlippingInstaller;
    [SerializeField] private SwipeDetectionInstaller _swipeDetectionInstaller;

    private void Awake()
    {
        IInputSystem inputSystem = null;
        var animationsApplyer = new AnimationsApplyer();

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                inputSystem = new MobileInputSystem();
                break;
            case RuntimePlatform.WindowsEditor:
                inputSystem = new ComputerInputSystem();
                break;
        }
        
        _creamApplyingInstaller.Install(inputSystem);
        _blushApplyingInstaller.Install(inputSystem, animationsApplyer);
        _lipstickApplyingInstaller.Install(inputSystem, animationsApplyer);
        _eyeShadowApplyingInstaller.Install(inputSystem, animationsApplyer);
        _pagesFlippingInstaller.Install();
        _swipeDetectionInstaller.Install();
    }
}