using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DragonEggScript : MonoBehaviour
{
    private static float bottomY = -25f;
    private GameObject[] heartsGO;
    private AudioSource audioSource;

    private void Start()
    {
        heartsGO = GameObject.FindGameObjectsWithTag("Heart");

        audioSource = GetComponent<AudioSource>();

        #region Получение значения поля из другого скрипта (Проба)
        //GameObject basketGO = GameObject.Find("Basket");
        //Component basketComponent = basketGO.GetComponent<BasketScript>() as BasketScript;

        //print($"Before: {((BasketScript)basketComponent).count}");

        //if (basketComponent != null) ((BasketScript)basketComponent).count = 777;

        //print($"After: {((BasketScript)basketComponent).count}");
        #endregion

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ground")
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            var em = ps.emission;
            em.enabled = true;

            Renderer rend;
            rend = GetComponent<Renderer>();
            rend.enabled = false;

            audioSource.Play();

            // Уничтожение всех яиц в корзине
            GameObject basketGO = GameObject.Find("Basket");
            Transform[] chdInBasket = basketGO.GetComponentsInChildren<Transform>();
            //print(chdInBasket.Length);
            foreach (var obj in chdInBasket)
            {
                if (obj.tag == "DragonEnemyEgg")
                {
                    Destroy(obj.gameObject);
                }  
            }

            if (heartsGO.Length == 3) Destroy(GameObject.Find("HeartHigh"));
            if (heartsGO.Length == 2) Destroy(GameObject.Find("HeartMiddle"));
            if (heartsGO.Length <= 1) 
            { 
                Destroy(GameObject.Find("HeartLow"));
                print("Game Over");

                GameObject panelGO = GameObject.Find("PanelLOOSE");
                //print(panelGO.name);
                Text panelGT = panelGO.GetComponent<Text>();
                panelGT.enabled = true; // You LOOSE!

                GameObject dragonGO = GameObject.Find("DragonEnemy");
                Destroy(dragonGO);

                GameObject eggGO = GameObject.FindGameObjectWithTag("DragonEnemyEgg");
                basketGO.GetComponent<MeshRenderer>().enabled = false;
                eggGO.GetComponent<MeshRenderer>().enabled = false;

                StartCoroutine(PauseCoroutine(5)); // Wait
            } 
        }
    }

    IEnumerator PauseCoroutine(float timeWait) 
    {
        yield return new WaitForSeconds(timeWait);

        SceneManager.LoadScene("Scene0");
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (transform.position.y < bottomY)
        {
            if (heartsGO.Length > 1)
                Destroy(this.gameObject);
        }
    }
}