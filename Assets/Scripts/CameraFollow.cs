using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 1.2f;
    public float height = 1.0f;
    public float smoothSpeed = 8f;
    public float collisionPadding = 0.2f;
    
    private Vector3 currentOffset;
    //private Quaternion currentRotation;
    
    void Start()
    {
    	if (target == null) return;
    	
    	// Start behind Clementine
    	BehindTarget();
    	//transform.position = target.position + currentOffset;
    	//transform.rotation = currentRotation;
    	
	}
    
    void Update()
    {
        if (target == null) return;
        
        // Press C to reset camera behind Clementine
        if (Input.GetKey(KeyCode.C))
        {
        	BehindTarget();
        }
    }
    
    void LateUpdate()
    {
	if (target == null) return;
	
	Vector3 targetLookPoint = target.position + Vector3.up * 0.6f;
	Vector3 desiredPosition = target.position + currentOffset;
        
        Vector3 dir = desiredPosition - targetLookPoint;
        float dist = dir.magnitude;
        dir.Normalize();
        
        RaycastHit hit;
        if (Physics.Raycast(targetLookPoint, dir, out hit, dist))
        {
        	desiredPosition = hit.point - dir * collisionPadding;
        }
        
        transform.position = Vector3.Lerp(
        	transform.position, 
        	desiredPosition, 
        	smoothSpeed * Time.deltaTime
        );
        
        // Keep the same camera angle
        //Vector3 lookTarget = target.position + Vector3.up * 0.8f;
        transform.LookAt(targetLookPoint);
        //transform.rotation = currentRotation;
        
    }
    
    void BehindTarget()
    {
    	currentOffset = -target.forward * distance + Vector3.up * height;
    	transform.position = target.position + currentOffset;
    	transform.LookAt(target.position + Vector3.up * 0.6f);
    	
    }
}

