using System;
using System.Collections;
using UnityEngine;

public class PagesFlipping : IDisposable
{
    private const float FinalPageXScale1 = 1f;
    private const float FinalPageXScale2 = -1f;
    
    private readonly float _flippingDuration;
    private readonly Page[] _pages;

    private float _currentRoutineTime = 0f;
    private int _currentSheet = 1;
    private Coroutine _currentFlippingRoutine;
    
    public PagesFlipping(float flippingDuration, Page[] pages)
    {
        _flippingDuration = flippingDuration;
        _pages = pages;
    }

    public void Init()
    {
        EventBus.Subscribe<SwipeTakenSignal>(OnSwipeTaken, this);
    }
    
    public void Flip(int direction)
    {
        _currentFlippingRoutine ??= _pages[0]?.StartCoroutine(FlipRoutine(direction));
    }

    public void Dispose()
    {
        EventBus.Unsubscribe<SwipeTakenSignal>(OnSwipeTaken);
    }
    
    private void OnSwipeTaken(SwipeTakenSignal signal)
    {
        if (signal.XDelta > 0f)
            Flip(1);
        else
            Flip(-1);
    }
    
    private IEnumerator FlipRoutine(int direction)
    {
        if (_currentSheet - direction == 0 || _currentSheet - direction == _pages.Length)
            yield break;
        
        var currentPage = direction == 1 ? _pages[_currentSheet - 1] : _pages[_currentSheet];
        var finalPageXScale = direction == 1 ? FinalPageXScale1 : FinalPageXScale2;
        var startScale = currentPage.transform.localScale;
        var finalScale = new Vector3(finalPageXScale, startScale.y, startScale.z);
        bool changedSortingOrders = false;
        
        while (_currentRoutineTime < _flippingDuration)
        {
            _currentRoutineTime += Time.deltaTime;
            float t = _currentRoutineTime / _flippingDuration;
            currentPage.transform.localScale = Vector3.Lerp(startScale, finalScale, t);

            if (Mathf.Abs(_currentRoutineTime - _flippingDuration) < _flippingDuration / 2 && changedSortingOrders == false)
            {
                if (direction == 1)
                {
                    currentPage.SetSortingOrder(_pages[_currentSheet].SortingOrder + 3);
                    MovePageRight(currentPage);
                }
                else
                {
                    currentPage.SetSortingOrder(_pages[_currentSheet-1].SortingOrder + 3);
                    MovePageLeft(currentPage);
                }

                changedSortingOrders = true;
            }
            
            yield return null;
        }

        currentPage.transform.localScale = finalScale;
        _currentRoutineTime = 0f;
        _currentSheet -= direction;
        _currentFlippingRoutine = null;
    }

    private void MovePageLeft(Page currentPage)
    {
        if (currentPage.RightPageContent)
            currentPage.RightPageContent.sortingOrder = currentPage.SortingOrder - 1;
        
        if (currentPage.LeftPageContent)
            currentPage.LeftPageContent.sortingOrder = currentPage.SortingOrder + 1;
    }

    private void MovePageRight(Page currentPage)
    {
        if (currentPage.RightPageContent)
            currentPage.RightPageContent.sortingOrder = currentPage.SortingOrder + 1;
        
        if (currentPage.LeftPageContent)
            currentPage.LeftPageContent.sortingOrder = currentPage.SortingOrder - 1;
    }
}