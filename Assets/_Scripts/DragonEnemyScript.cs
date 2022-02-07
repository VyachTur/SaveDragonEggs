using UnityEngine;

public class DragonEnemyScript : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField]
    private GameObject dragonEggPrefab;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float timeBetweenEggDrops = 1f;
    [SerializeField] private float leftRightDistance = 10f;
    [SerializeField] private float chanceDirections = 0.3f;

    void Start()
    {
        Invoke("DropEgg", 2f);
    }

    void DropEgg()
    {
        Vector3 myVector = new Vector3(0.0f, 5.0f, 0.0f);
        GameObject egg = Instantiate<GameObject>(dragonEggPrefab);
        egg.transform.position = transform.position + myVector;
        Invoke("DropEgg", timeBetweenEggDrops);
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        if (Mathf.Abs(pos.x) > leftRightDistance) speed *= -1;
    }

    private void FixedUpdate()
    {
        if (Random.value < chanceDirections)
        {
            speed *= -1;
        }
    }

}
