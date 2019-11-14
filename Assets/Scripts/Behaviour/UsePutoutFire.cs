using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePutoutFire : MonoBehaviour {

    ExtinguisherBehaviour ext;
    // Use this for initialization
    void Start () {
        ext = transform.GetComponent<ExtinguisherBehaviour>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.J))
        {
            ext.UseExtgui(true);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            ext.UseExtgui(false);
        }
	}
}
