using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    [Header("Move")] 
    [SerializeField] private float moveSpeed;

    [SerializeField] private float xInput;
    [SerializeField] private float zInput;

    [Header("Zoom")] 
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomModifier;

    [SerializeField] private float minZoomDist;
    [SerializeField] private float maxZoomDist;
    
    [SerializeField] private float dist; //between cam base and main cam
    
    //lock camera corner
    [SerializeField] private Transform corner1;
    [SerializeField] private Transform corner2;
    
    public static CameraController instance;

    private void Awake()
    {
        instance = this;
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 50;
        
        //zoom setting
        zoomSpeed = 25;
        minZoomDist = 15;
        maxZoomDist = 50;
    }

    // Update is called once per frame
    void Update()
    {
        MoveByKB();
        Zoom();
    }

    void MoveByKB()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        
        Vector3 dir = (transform.forward * zInput) + (transform.right * xInput);
        transform.position += dir * moveSpeed * Time.deltaTime;
        
        //Clamp map
        transform.position = Clamp(corner1.position, corner2.position);
    }//end moveKB
    
    private Vector3 Clamp(Vector3 lowerLeft, Vector3 topRight)
    {
        Vector3 pos = new Vector3(Mathf.Clamp(transform.position.x, lowerLeft.x, topRight.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, lowerLeft.z, topRight.z));

        return pos;
    }
    
    void Zoom()
    {
        zoomModifier = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKey(KeyCode.Z))
        {
            zoomModifier = 0.01f;
        }

        if (Input.GetKey(KeyCode.X))
        {
            zoomModifier = -0.01f;
        }

        dist = Vector3.Distance(transform.position, cam.transform.position);

        if (dist < minZoomDist && zoomModifier > 0f)
        {
            return; //too colse
        }
        else if (dist > maxZoomDist && zoomModifier < 0f)
        {
            return; //too far
        }

        cam.transform.position += cam.transform.forward * zoomModifier * zoomSpeed;
    }
    
    
}//end class
