using Cinemachine;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private CinemachineVirtualCamera camera;
    // here i'm setting up the cinemachine to follow the ball
    void Start()
    {
        GameObject ball = GameObject.Find("ballprefab(Clone)");
        Debug.Log(ball.transform);
        camera = GetComponent<CinemachineVirtualCamera>();
        camera.LookAt.LookAt(ball.transform);
        camera.LookAt.LookAt(ball.transform);
    }
}
