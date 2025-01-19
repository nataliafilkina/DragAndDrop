using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class TouchHandler : MonoBehaviour
{
    private IDraggble _currentDragItem = null;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && _currentDragItem == null)
            {
                var ray = Camera.main.ScreenPointToRay(touch.position);

                var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                var hits = Physics2D.RaycastAll(touchPosition, Vector2.zero, 10f, LayerMask.GetMask("Item"));
                if (hits.Length > 0)
                {
                    var topObject = hits
                        .OrderByDescending(hit => hit.collider.GetComponent<SortingGroup>().sortingOrder).FirstOrDefault();
                    if (topObject.collider.TryGetComponent<IDraggble>(out var item))
                    {
                        _currentDragItem = item;
                        item.BeginDrag(touchPosition);
                    }
                }
            }
            if (touch.phase == TouchPhase.Ended && _currentDragItem != null)
            {
                _currentDragItem.Drop();
                _currentDragItem = null;
            }
        }
    }
}
