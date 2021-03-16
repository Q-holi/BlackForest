using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    GaraHook garaHook;
    public DistanceJoint2D joint2D;
    // Start is called before the first frame update
    void Start()
    {
        garaHook = GameObject.Find("Player2").GetComponent<GaraHook>();
        joint2D = GetComponent<DistanceJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ring"))
        {
            joint2D.enabled = true;
            garaHook.isAttach = true;
        }
    }
}
