using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform body;
    public float sensX = 10;
    public float sensY = 5;
    public float angleX = 0;
    public float angleY = 0;
    //public PlayerController myPlayer;
    //public string status;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //-- Movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        angleX += mouseX * sensX;
        angleY += mouseY * sensY;

        angleY = Mathf.Clamp(angleY, -85f, 85f);

        //myPlayer = gameObject.GetComponentInParent<PlayerController>();
        //status = myPlayer.PlayerStatus;

        transform.localRotation = Quaternion.Euler(-angleY, 0, 0);
        body.transform.localRotation = Quaternion.Euler(0, angleX, 0);

    }
}
