//using UnityEngine;

//public class PlayerMoving : MonoBehaviour
//{

//    public float moveSpeed;
    

   
     

//    public GameObject Player;
//    private Rigidbody rb;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    // Update is called once per frame
//    void Update()
//    { 
//        float h = Input.GetAxis("Horizontal");
//        float v = Input.GetAxis("Vertical");

//        Vector3 moveDir = new Vector3(h, 0, v);

//        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
//        Rotate();
//    }

//    private void Rotate()
//    {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        Plane plane = new Plane(Vector3.up, Vector3.zero);

//        float rayLength;

//        if(plane.Raycast(ray, out rayLength))
//        {
//            Vector3 pointToLook = ray.GetPoint(rayLength);
//            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
//        }
//    }

   
//}
