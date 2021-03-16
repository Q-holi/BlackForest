using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spin_Shot : MonoBehaviour
{

    //회전되는 스피드이다.
    public float rot_Speed;

    //총알이 발사될 위치이다.
    public Transform pos;

    //발사될 총알 오브젝트이다.
    public GameObject bullet;

    private void Update()
    {
        if (Input.GetKeyDown("3"))
            {
                InvokeRepeating("SpinShot", 0.1f, 0.05f);
                Invoke("CancleInvokeLog", 4f);
            }
    }

    void SpinShot()
    {
            //회전
            pos.Rotate(Vector3.forward * rot_Speed * 100 * Time.deltaTime);
            Instantiate(bullet, pos.position, pos.rotation);
    }

    private void CancleInvokeLog()
    {
        CancelInvoke("SpinShot");
    }
}
