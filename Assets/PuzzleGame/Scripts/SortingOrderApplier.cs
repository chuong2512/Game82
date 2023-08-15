using UnityEngine;
using UnityEngine.UI;

public class SortingOrderApplier : MonoBehaviour
{
    private Canvas canvas;
    private GraphicRaycaster raycaster;
    
    void Awake()
    {
        canvas = GetComponent<Canvas>();

        raycaster = GetComponent<GraphicRaycaster>();
    }

    public void SetSortingOrder(int sortingOrder)
    {
        canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = sortingOrder;

        raycaster = gameObject.AddComponent<GraphicRaycaster>();
    }

    public void Hide()
    {
        Destroy(raycaster);
        Destroy(canvas);
    }
}
