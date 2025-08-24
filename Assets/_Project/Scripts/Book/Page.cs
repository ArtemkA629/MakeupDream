using UnityEngine;

public class Page : MonoBehaviour
{
    [SerializeField] private Canvas _canvasComponent;
    
    [field: SerializeField] public Canvas RightPageContent { get; private set; }
    [field: SerializeField] public Canvas LeftPageContent { get; private set; }
    
    public int SortingOrder => _canvasComponent.sortingOrder;

    public void SetSortingOrder(int sortingOrder)
    {
        _canvasComponent.sortingOrder = sortingOrder;
    }
}
