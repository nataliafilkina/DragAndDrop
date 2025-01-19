using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Item : MonoBehaviour, IDraggble
{
    public event Action<Item> OnStartDrag;

    #region Fields
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _scaleFactor = 1.2f;
    [SerializeField]
    private Transform _draggbleItem;

    private bool _isDragging = false;
    private Vector3 _touchOffset;
    private DragAndDropAnimation _animation;
    private SortingGroup _sortingGroup;

    private MovementItem _movement;
    private PlaceItem _place;
    #endregion

    protected virtual void Awake()
    {
        _animation = new DragAndDropAnimation(transform, _scaleFactor);
        _sortingGroup = GetComponent<SortingGroup>();

        _movement = new MovementItem(transform, _speed);
        _place = new PlaceItem(this);

        SetOrderLayer();
    }

    private void Update()
    {
        if (_isDragging)
        {
            _movement.Move(_touchOffset);
        }
        if(_movement.IsFalling) 
        {
            if (!_movement.Fall())
            {
                transform.SetParent(_draggbleItem.parent);
                SetOrderLayer();
            }
        }
    }

    public void BeginDrag(Vector3 touchPosition)
    {
        _touchOffset = transform.position - touchPosition;
        _isDragging = true;
        _animation.Drag();
        transform.SetParent(_draggbleItem);
        OnStartDrag?.Invoke(this);
    }

    public void Drop()
    {
        _isDragging = false;
        _animation.Drop();
        if (_place.Place())
            SetOrderLayer();
        else
            _movement.Fall();

    }

    private void SetOrderLayer()
    {
        var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        var screenHeight = Screen.height;
        var normalizedY = screenPosition.y / screenHeight;
        var sortingOrder = Mathf.RoundToInt((1 - normalizedY) * 1000);
        _sortingGroup.sortingOrder = sortingOrder;
    }
}
