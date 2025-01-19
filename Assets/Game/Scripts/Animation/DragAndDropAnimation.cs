using UnityEngine;
using DG.Tweening;

public class DragAndDropAnimation
{
    private Transform _transform;
    private float _scaleFactor;
    private float _duration = 0.2f;
    private Vector3 _originalScale;

    public DragAndDropAnimation(Transform transform, float scaleOffset)
    {
        _transform = transform;
        _scaleFactor = scaleOffset;
        _originalScale = transform.localScale;
    }

    public void Drag()
    {
        _transform.DOScale(_transform.localScale * _scaleFactor, _duration).SetEase(Ease.OutBack);
    }

    public void Drop()
    {
        _transform.DOScale(_originalScale, _duration).SetEase(Ease.InOutBack);
    }
}
