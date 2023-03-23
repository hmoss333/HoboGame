using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] private Transform target;
    [SerializeField] float smoothTime, rotTime;
    private float camXOffset;
    private float camYOffset;
    private float camZOffset;
    Quaternion baseRot;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        camXOffset = cameraPos.x;
        camYOffset = cameraPos.y;
        camZOffset = cameraPos.z;

        baseRot = transform.rotation;
    }

    //Late Update should always be used for camera follow logic
    //This calculates after all other update logic to ensure that it uses the most accurate position values
    void LateUpdate()
    {
        Vector3 pos = target.transform.position;
            pos.x += camXOffset;
            pos.y += camYOffset;
            pos.z += camZOffset;

        transform.position = Vector3.Lerp(transform.position, pos, smoothTime * Time.deltaTime); //update camera position
        transform.rotation = Quaternion.Lerp(transform.rotation, baseRot, rotTime * Time.deltaTime); //update camera rotation based on focus state
    }


    //Target Get/Sets
    public void SetTarget(GameObject newTargetObj)
    {
        target = newTargetObj.transform;
    }

    public Transform GetTarget()
    {
        return target;
    }
}
