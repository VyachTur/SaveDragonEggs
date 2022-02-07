using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasketScript : MonoBehaviour
{
    //public int count = 4; // пробное поле для теста

    [SerializeField] private float basketBound = 22f;
    [SerializeField] private int maxScore = 28;
    [SerializeField] private GameObject panel;
    private AudioSource audioSource;
    private Text scoreGT;
    private int score = 0;

    private void Start()
    {
        //count = 10; // пробное поле для теста

        GameObject scoreGO = GameObject.Find("Score");
        scoreGT = scoreGO.GetComponent<Text>();
        scoreGT.text = $"{score}/{maxScore}";

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 pos = this.transform.position;
        pos.x = -mousePos3D.x;

        if (Mathf.Abs(pos.x) > basketBound) return;
        this.transform.position = pos;

        if (score < maxScore)
        {
            score = this.transform.childCount;
            scoreGT.text = $"{score}/{maxScore}";
        }

        if (score >= maxScore)
        {
            //print("You WIN!");
            panel.SetActive(true); // You WIN!
            StartCoroutine(PauseCoroutine(5)); // Wait

            GameObject dragonGO = GameObject.Find("DragonEnemy");
            Destroy(dragonGO);

            GameObject basketGO = GameObject.Find("Basket");
            GameObject[] eggsGO = GameObject.FindGameObjectsWithTag("DragonEnemyEgg");
            basketGO.GetComponent<MeshRenderer>().enabled = false;

            foreach (var egg in eggsGO)
            {
                egg.GetComponent<MeshRenderer>().enabled = false;
            }

        }
    }

    IEnumerator PauseCoroutine(float timeWait) 
    {
        yield return new WaitForSeconds(timeWait);

        SceneManager.LoadScene("Scene0");
        panel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Привязка яиц к корзине (чтобы не просачивались через коллайдер)
        if (other.tag == "DragonEnemyEgg")
        {
            Transform transformEgg = other.gameObject.transform;
            transformEgg.parent = this.transform;

            audioSource.Play();
        }
    }

}