using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Vector3 cameraOffsetDistance = new Vector3(0, 0, -15);


    private void Awake()
    {
        transform.position = cameraOffsetDistance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
