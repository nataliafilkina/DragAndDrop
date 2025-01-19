using UnityEngine;

public class ContainItem : Item
{
    [SerializeField]
    private int _maxItems = 5;

    private int _currentAmount;
    private float _shiftChild = 0.01f;

    protected override void Awake()
    {
        base.Awake();
        _currentAmount = transform.childCount - 2;
    }

    private void Start()
    {
        var items = GetComponentsInChildren<Item>();
        foreach(var item in items)
        {
            item.OnStartDrag += Pop;
        }
    }

    public bool Put(Item item)
    {
        if (_currentAmount >= _maxItems)
            return false;

        item.transform.SetParent(transform);
        var newItemPosition = transform.position;
        newItemPosition.y += _shiftChild;
        item.transform.position = newItemPosition;
        item.OnStartDrag += Pop;
        _currentAmount++;

        return true;
    }

    private void Pop(Item item)
    {
        _currentAmount--;
        item.OnStartDrag -= Pop;
    }
}
