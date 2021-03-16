using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject powerbullet;
    public int damage = 10;
    public Transform playerposition;
    public float rot_Speed;
    public Transform pos;
    public Transform target;
    //발사될 총알 오브젝트

    float timer;
    int waitingTime;

    void Start()
    {
        timer = 0.0f;
        waitingTime = 3;
    }
    void RanodomP()
    {
        int ran = Random.Range(1,5);
        if (ran == 1)
        {
            CicleShot();
        }
        else if (ran == 2)
        {
            WonGiOk();
        }
        else if (ran == 3)
        {
            gotoshot();
        }
        else if(ran == 4)
        {
            InvokeRepeating("SpinShot", 0.1f, 0.05f);
            Invoke("CancleInvokeLog", 2f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > waitingTime)
        {
            RanodomP();
            timer = 0;
        }
    }

    void CicleShot()
    {
        for (int i = 0; i < 360; i += 13)
        {
            firePoint.rotation = Quaternion.Euler(0, 0, i);
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        }
    }
    void SpinShot()
    {
        //회전
        pos.Rotate(Vector3.forward * rot_Speed * 100 * Time.deltaTime);
        Instantiate(bulletPrefab, pos.position, pos.rotation);
    }

    private void CancleInvokeLog()
    {
        CancelInvoke("SpinShot");
    }


    void WonGiOk()
    {
        int shotpos = Random.Range(90, 270);
        firePoint.rotation = Quaternion.Euler(0, 0, shotpos);
        Instantiate(powerbullet, firePoint.position, firePoint.rotation);
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
    }

    public void gotoshot()
    {
        //Target방향으로 발사될 오브젝트 수록
        List<Transform> bl = new List<Transform>();

        for (int i = 0; i < 360; i += 13)
        {
            //총알 생성
            var temp = Instantiate(bulletPrefab);

            //5초후 삭제
            Destroy(temp, 5f);

            temp.transform.position = firePoint.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);

            //?초후에 Target에게 날아갈 오브젝트 수록
            bl.Add(temp.transform);
        }
        //총알을 Target 방향으로 이동시킨다.
        StartCoroutine(BulletToTarget(bl));
    }
    IEnumerator BulletToTarget(List<Transform> bl)
    {
        //0.5초 후에 시작
        yield return new WaitForSeconds(0.5f);


        for (int i = 0; i < bl.Count; i++)
        {
            //현재 총알의 위치에서 플레이의 위치의 벡터값을 뻴셈하여 방향을 구함
            var target_dir = target.transform.position - bl[i].position;

            //x,y의 값을 조합하여 Z방향 값으로 변형함. -> ~도 단위로 변형
            var angle = Mathf.Atan2(target_dir.y, target_dir.x) * Mathf.Rad2Deg;

            //Target 방향으로 이동
            bl[i].rotation = Quaternion.Euler(0, 0, angle);
        }
        //데이터 해제
        bl.Clear();
    }
}
