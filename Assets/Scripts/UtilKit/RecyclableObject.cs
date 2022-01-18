using UnityEngine;

public class RecyclableObject : MonoBehaviour
{
    public bool IsAvailabe
    {
        get { return _isAvailable; }
        set
        {
            _isAvailable = value;
            gameObject.SetActive(!_isAvailable);
        }
    }

    private bool _isAvailable;
}
