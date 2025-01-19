using UnityEngine;

public class MovementItem
{
    #region Fields
    private Transform _transform;
    private float _speed;
    private LayerMask _surfaceLayer;
    public bool IsFalling { get; private set; }
    #endregion

    public MovementItem(Transform transform, float speed)
    {
        _transform = transform;
        _speed = speed;
        _surfaceLayer = LayerMask.GetMask(new string[] { "Surface", "Floor"});
    }

    public void Move(Vector3 TouchOffset)
    {
        var newPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) + TouchOffset;
        newPosition.z = 0;
        _transform.position = newPosition;
    }

    public bool Fall()
    {
        IsFalling = true;
        var newPosition = _transform.position;
        newPosition.y -= _speed * Time.deltaTime;
        _transform.position = newPosition;

        CheckGround();
        return IsFalling;
    }

    private void CheckGround()
    {
        if (Physics2D.OverlapPoint(_transform.position, _surfaceLayer) != null)
        {
            IsFalling = false;
        }
    }
}
