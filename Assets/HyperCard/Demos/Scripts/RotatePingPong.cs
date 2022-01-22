using UnityEngine;

public class RotatePingPong : MonoBehaviour
{
    public float Rotation = 20;
    public float Speed = 5;

    private float _startAngle;
    private float _endAngle;


    void Start () {
        var rot = transform.rotation.eulerAngles;

        _startAngle = rot.y - Rotation;
        _endAngle = rot.y + Rotation;
    }

    void Update () {

        var y = Mathf.PingPong(Time.time * Speed, _endAngle - _startAngle) + _startAngle;

        transform.localEulerAngles = new Vector3(0, y, 0);
    }
}
