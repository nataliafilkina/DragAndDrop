using UnityEngine;

public class Pan : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sceneSprite;
    private Vector3 touchStartPosition;
    private bool _isPanning = false;
    private float _minX;
    private float _maxX;

    private void Start()
    {
        var halfCameraWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;

        _minX = _sceneSprite.bounds.min.x + halfCameraWidth;
        _maxX = _sceneSprite.bounds.max.x - halfCameraWidth;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touchPosition;
                var hits = Physics2D.RaycastAll(touchStartPosition, Vector2.zero, 10f, LayerMask.GetMask("Item"));
                _isPanning = hits.Length < 1;
            }
            if(_isPanning && touch.phase == TouchPhase.Moved)
            {
                var direction = touchStartPosition - touchPosition;
                var newPosition = Camera.main.transform.position + direction;
                newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
                newPosition.y = 0;
                Camera.main.transform.position = newPosition;
            }
            if(_isPanning && touch.phase == TouchPhase.Ended)
                _isPanning = false;
        }
    }
}
