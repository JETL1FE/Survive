using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(Inputs.Instance.mouseWorldPosition.x, Inputs.Instance.mouseWorldPosition.y);
    }
}
