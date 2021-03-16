using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaraHook : MonoBehaviour
{

    public LineRenderer line;
    public Transform hook;
    Vector2 mousedir;

    public bool isHookActive;
    public bool isLineMax;
    public bool isAttach;
    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);


        if (Input.GetKeyDown(KeyCode.E) && !isHookActive)
        {
            hook.position = transform.position;
            mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            isHookActive = true;
            isLineMax = false;
            hook.gameObject.SetActive(true);
        }

        if (isHookActive && !isLineMax && !isAttach)
        {
            hook.Translate(mousedir.normalized * Time.deltaTime * 30);

            if (Vector2.Distance(transform.position, hook.position) > 5) // 이부분 숫자 늘리면 선길이랑 시간늘어남
            {
                isLineMax = true;
            }
        }
        else if (isHookActive && isLineMax && !isAttach)
        {
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * 20);
            if (Vector2.Distance(transform.position, hook.position) < 0.2f)
            {
                isHookActive = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
            }
        }
        else if (isAttach)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isAttach = false;
                isHookActive = false;
                isLineMax = false;
                hook.GetComponent<Hook>().joint2D.enabled = false;
                hook.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isAttach = false;
                isHookActive = false;
                isLineMax = false;
                hook.GetComponent<Hook>().joint2D.enabled = false;
                hook.gameObject.SetActive(false);
            }
        }
    }

}
