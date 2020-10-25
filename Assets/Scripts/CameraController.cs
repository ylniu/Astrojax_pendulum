using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // store a public reference to the Player game object, so we can refer to it's Transform
    // public GameObject player;

    // Store a Vector3 offset from the player (a distance to place the camera from the player at all times)
    private Vector3 offset;




    public Transform target;
    public float distance = 100f;
    public float minDistance = 10f;
    public float maxDistance = 200f;
    public float wheelSpeed = 40f;
    [Range(0, 2)]
    public int workingButton = 0;

    public float axisXSpeed = 1f;
    public float axisYSpeed = 1f;
    public float minYLimit = 10f;
    public float maxYLimit = 80f;
    public float smoothTime = 0.5f;
    public float smoothDampMaxSpeed = 10000f;
    public float smoothDampDeltaTime = 0.02f;

    private float defaultDistance;

    private Vector3 _lastMousePos;
    private Vector3 _saveMousePos;
    private float _smoothDistance;
    private float _velocityDistance;
    private Vector3 _smooth;
    private Vector3 _velocity;


    private bool isUIEnter;







    // At the start of the game..
    void Start()
    {
        // Create an offset by subtracting the Camera's position from the player's position
        offset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // After the standard 'Update()' loop runs, and just before each frame is rendered..
    void LateUpdate()
    {
        if (target)
        {
            //float angle = 0;
            //if (Input.GetMouseButton(0)) angle = 1;
            //if (Input.GetMouseButton(1)) angle = -1;

            //transform.RotateAround(
            //    player.transform.position,
            //    player.transform.up,
            //    angle);










            //----------------------------------------------------------------------------
            //if (Input.GetMouseButton(workingButton))
            //{
            //    if (Input.GetMouseButtonDown(workingButton))
            //    {
            //        _lastMousePos = Input.mousePosition;
            //    }
            //    _saveMousePos.x += (Input.mousePosition.x - _lastMousePos.x) * axisXSpeed;
            //    _saveMousePos.y -= (Input.mousePosition.y - _lastMousePos.y) * axisYSpeed;
            //    _lastMousePos = Input.mousePosition;
            //    _saveMousePos.y = Mathf.Clamp(_saveMousePos.y, minYLimit, maxYLimit);
            //}
            //_smooth = Vector3.SmoothDamp(_smooth, _saveMousePos, ref _velocity, smoothTime, smoothDampMaxSpeed, smoothDampDeltaTime);
            //transform.rotation = Quaternion.Euler(_smooth.y, _smooth.x, 0);
            //if (target != null)
            //{
            //    distance -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
            //    // distance = Mathf.Clamp(distance, minDistance, maxDistance);
            //    _smoothDistance = Mathf.SmoothDamp(_smoothDistance, distance, ref _velocityDistance, smoothTime, smoothDampMaxSpeed, smoothDampDeltaTime);
            //    //transform.position = transform.rotation * new Vector3(0, 0, -60) + target.position;
            //    // transform.position = transform.rotation * new Vector3(0, 0, -_smoothDistance) + target.position;
            //}
            //----------------------------------------------------------------------------






            //if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            //    distance -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
            //    distance = Mathf.Clamp(distance, minDistance, maxDistance);
            //    _smoothDistance = Mathf.SmoothDamp(_smoothDistance, distance, ref _velocityDistance, smoothTime, smoothDampMaxSpeed, smoothDampDeltaTime);
            //    transform.position = transform.rotation * new Vector3(0, 0, -60) + target.position;
            //    transform.position = transform.rotation * new Vector3(0, 0, -_smoothDistance) + target.position;
            //}
            //else if (target != null)
            //{
            //    distance -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
            //    distance = Mathf.Clamp(distance, minDistance, maxDistance);
            //    _smoothDistance = Mathf.SmoothDamp(_smoothDistance, distance, ref _velocityDistance, smoothTime, smoothDampMaxSpeed, smoothDampDeltaTime);
            //    transform.position = transform.rotation * new Vector3(0, 0, -60) + target.position;
            //    transform.position = transform.rotation * new Vector3(0, 0, -_smoothDistance) + target.position;
            //}

            //鼠标滚轮的效果
            //Camera.main.fieldOfView 摄像机的视野
            //Camera.main.orthographicSize 摄像机的正交投影
            //Zoom out
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Camera.main.fieldOfView <= 100)
                    Camera.main.fieldOfView += 2;
                if (Camera.main.orthographicSize <= 20)
                    Camera.main.orthographicSize += 0.5F;
            }
            //Zoom in
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (Camera.main.fieldOfView > 2)
                    Camera.main.fieldOfView -= 2;
                if (Camera.main.orthographicSize >= 1)
                    Camera.main.orthographicSize -= 0.5F;
            }
        }
    }
}