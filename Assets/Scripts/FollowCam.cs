using UnityEngine;
using System.Collections.Generic;

public class FollowCam : MonoBehaviour
{
    public GameObject target; // 따라갈 대상

    public float smoothSpeed = 0.125f; // 카메라 이동의 부드러움 정도
  
    public float offsetX;
    public float offsetY;
    public float offsetZ;
   

    // Update is called once per frame
    void Update()
    {
        Vector3 Followed = new Vector3(target.transform.position.x 
            + offsetX, target.transform.position.y + offsetY, target.transform.position.z + offsetZ);
        transform.position = Followed;
    }
}
