using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CameraFocus : MonoBehaviour
{
    public float dist;
    
    public float rotateSpeed;
    public float maxPitch;
    public float minFov;
    public float maxFov;
    public float minOrthoSize;
    public float maxOrthoSize;
    public Text fovTypeText;
    public Scrollbar fovScrollBar;
    
    public Camera cam => this.GetComponent<Camera>();
    
    [Header("Debug")]
    
    [SerializeField] bool isMouseControlled;
    [SerializeField] Vector2 recordMousePos; 
    
    public void BeginMouseControl()
    {
        recordMousePos = Input.mousePosition;
        isMouseControlled = true;
    }
    
    public void EndMouseControl()
    {
        isMouseControlled = false;
    }
    
    void Update()
    {
        if(isMouseControlled)
        {
            Vector2 curMousePos = Input.mousePosition;
            var deltaPos = curMousePos - recordMousePos;
            var curPos = this.transform.position;
            
            this.transform.position = Quaternion.Euler(0, rotateSpeed * Time.deltaTime * deltaPos.x, 0) * curPos;
            
            curPos = this.transform.position;
            var pitchAxis = Vector3.Cross(curPos, Vector3.up).normalized;
            this.transform.position = Quaternion.AngleAxis(-rotateSpeed * Time.deltaTime * deltaPos.y, pitchAxis) * curPos;
            if(Mathf.Sin(Mathf.Deg2Rad * maxPitch) * dist < Mathf.Abs(this.transform.position.y)) this.transform.position = curPos;
            
            recordMousePos = curMousePos;
        }
        
        var fovRate = fovScrollBar.value; 
        if(fovScrollBar.value <= 0.5f)
        {
            cam.orthographic = true;
            cam.orthographicSize = (fovRate * 2) * (maxOrthoSize - minOrthoSize) + minOrthoSize;
            fovTypeText.text = "Ortho: " + cam.orthographicSize;
        }
        else
        {
            cam.orthographic = false;
            var fov = (fovRate - 0.5f) * 2 * (maxFov - minFov) + minFov;
            cam.fieldOfView = fov;
            fovTypeText.text = "Persp: " + cam.fieldOfView;
        }
        this.transform.rotation = Quaternion.LookRotation(-this.transform.position, Vector3.up);
        this.transform.position = this.transform.position.normalized * dist;
        
    }
}
