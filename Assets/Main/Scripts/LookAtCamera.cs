using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform mLookAt;
    private Transform localTrans;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        localTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mLookAt != null)
        {
            localTrans.LookAt(mLookAt);
            localTrans.Rotate(0f, 180f, 0f);

        }
    }
}
