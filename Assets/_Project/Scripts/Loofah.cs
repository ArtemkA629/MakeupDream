using UnityEngine;
using UnityEngine.UI;

public class Loofah : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _acne;
    [SerializeField] private Transform _face;

    private void Awake()
    {
        _button.onClick.AddListener(ResetFaceMakeup);
    }

    private void ResetFaceMakeup()
    {
        _acne.CrossFadeAlpha(1f, 0f, false);
        var _faceImages = _face.GetComponentsInChildren<Image>();

        foreach (var faceImage in _faceImages)
        {
            if (faceImage == _acne)
                continue;

            faceImage.sprite = null;
            faceImage.CrossFadeAlpha(0f, 0f, false);
        }
    }
}