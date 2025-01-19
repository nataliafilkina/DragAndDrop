using UnityEngine;

public interface IDraggble
{
    public void BeginDrag(Vector3 touchPosition);
    public void Drop();
}

