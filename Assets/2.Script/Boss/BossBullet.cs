using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour

{
    public float speed = 5f;
    public int damage = 5;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);

    }
    public void Update()
    {
        //두번째 파라미터에 Space.World를 해줌으로써 Rotation에 의한 방향 오류를 수정함
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "Projectile")
        {
            if (collision.gameObject.tag == "Player" && collision.gameObject.layer == 9)
            {
                Destroy(gameObject);
            }
        }
    }


}
