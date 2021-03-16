using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int maxHp = 100;
    public int curHp = 100;
    float imsi;
    public PlayerMovement player;
    public GameObject[] Stages;
    public GameObject menuSet;
    public Slider HealthBar;

    //public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage; 
    //public GameObject UIRestartBtn;
    public GameObject UIGameoverImage;
    public GameObject UIFinishImage;

    private void Start()
    {
        imsi = (float)curHp / (float)maxHp;
    }

    public void NextStage()
    {
        //Change Stage
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else
        { // Game Clear
            //Player Control Lock
            Time.timeScale = 0;

            //Restrat Button UI
            //Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
            //btnText.text = "Clear!";
            UIFinishImage.SetActive(true);
        }

        //Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    private void ViewBtn()
    {
        //UIRestartBtn.SetActive(true);
    }

    public void HealthDown()
    {
        if (curHp > 0)
        {
            curHp -= 10;
            //UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            //All Health UI Off
            //UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            curHp = -1;
            // Player Die Effect
            player.OnDie();
            // Result UI
            Debug.Log("죽었습니다!");
            // Retry Button UI
            UIGameoverImage.SetActive(true);
            //UIRestartBtn.SetActive(true);
        }
        imsi = (float)curHp / (float)maxHp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (curHp > 1)
            {
                //Player Reposition
                PlayerReposition();
            }

            //Health Down
            HealthDown();
        }

    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();

        HandleHp();


        if (Input.GetButtonDown("Cancel"))
        {

            if (menuSet.activeSelf)
            {
                Time.timeScale = 1;
                menuSet.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                menuSet.SetActive(true);
            }

        }
    }

    void HandleHp()
    {
        HealthBar.value = Mathf.Lerp(HealthBar.value, imsi, Time.deltaTime * 10);
    }

    public void Main_Title()
    {
        SceneManager.LoadScene("MainTitle");
        Time.timeScale = 1;
    }

    public void Continue()
    {
        menuSet.SetActive(false);
        Time.timeScale = 1;
    }

    public void Option()
    {
        SceneManager.LoadScene("Option2");
        Time.timeScale = 1;
    }
}
