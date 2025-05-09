using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float _destroyTime=1f;
    private Vector3 _offset = new Vector3(0f, 1f, 0f);

    void Start()
    {
        Destroy(gameObject, _destroyTime);
        
        transform.localPosition += _offset;
    }
}
