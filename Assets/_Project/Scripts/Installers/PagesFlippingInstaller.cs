using System;
using UnityEngine;

public class PagesFlippingInstaller : MonoBehaviour
{
    [SerializeField] private Page[] _pages;
    [SerializeField] private float _flippingDuration;
    
    private PagesFlipping _pagesFlipping;

    private void OnDisable()
    {
        _pagesFlipping.Dispose();
    }

    public void Install()
    {
        _pagesFlipping = new(_flippingDuration, _pages);
        _pagesFlipping.Init();
    }
}