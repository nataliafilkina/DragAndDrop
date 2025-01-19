using UnityEngine;

public class PlaceItem
{
    #region Fields
    private Transform _transform;
    private Item _item;
    private Collider2D _collider;
    private float _colliderWidth;
    private string _surfaceLayer = "Surface";
    private string _itemLayer = "Item";
    private string _floorLayer = "Floor";
    #endregion

    public PlaceItem(Item item)
    {
        _item = item;
        _transform = item.transform;
        _collider = _transform.GetComponent<Collider2D>();
        _colliderWidth = _collider.bounds.size.x * _transform.localScale.x;
    }

    public bool Place()
    {
        bool isPlaced = TryPlaceInItem();

        if (!isPlaced)
            isPlaced = TryPlaceOnSurface();

        return isPlaced;
    }

    private bool TryPlaceInItem()
    {
        var collision = FindCollisionOnLayer(_itemLayer, out bool isShifted);
        if (collision != null && collision.TryGetComponent<ContainItem>(out var contain))
        {
            if (IsFitsIn(collision) && contain.Put(_item))
            {
                if (isShifted)
                    _transform.position = collision.ClosestPoint(_transform.position);
                return true;
            }
        }
        return false;
    }

    private bool TryPlaceOnSurface()
    {
        var collision = FindCollisionOnLayer(_surfaceLayer, out bool isShifted);
        if (collision != null)
        {
            _transform.SetParent(_transform.parent.parent);
            if (isShifted)
                _transform.position = collision.ClosestPoint(_transform.position);
            return true;
        }

        collision = FindCollisionOnLayer(_floorLayer, out isShifted);
        if (collision != null)
        {
            _transform.SetParent(_transform.parent.parent);
            return true;
        }
        return false;
    }

    private bool IsFitsIn(Collider2D collider)
    {
        return collider.bounds.size.x * _transform.localScale.x >= _colliderWidth;
    }

    private Collider2D FindCollisionOnLayer(string layer, out bool isShifted)
    {
        isShifted = false;
        var collider = GetCollisionOverlapPoint(_transform.position, layer);
        if (collider != null)
            return collider;

        collider = GetCollisionOverlapPoint(_collider.bounds.center, layer);
        isShifted = collider != null;
        return collider;
    }

    private Collider2D GetCollisionOverlapPoint(Vector3 position, string layer)
    {
        SetColliderEnabled(false);
        var collider = Physics2D.OverlapPoint(position, LayerMask.GetMask(layer));
        SetColliderEnabled(true);
        return collider;
    }

    private void SetColliderEnabled(bool isEnabled)
    {
        var childItems = _transform.GetComponentsInChildren<Collider2D>();
        foreach (var child in childItems)
            child.enabled = isEnabled;
    }
}
