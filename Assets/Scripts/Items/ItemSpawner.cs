using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] SpawnableObjects;
    [SerializeField] private float[] Spawnablepriority;

    [Range(2, 50)]
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;

    [SerializeField] private GameObject instancePool;
    [SerializeField] private GameObject defaultObject;

    private ItemsCommon cmn;

    private Transform collectablesPool;
    private Transform activeItemsPool;

  

    private void Awake()
    {
        cmn = GameObject.FindGameObjectWithTag("GameController").transform.GetComponent<ItemsCommon>();
        collectablesPool = cmn.collectablesPool.transform;
        activeItemsPool = cmn.activeItemsPool.transform;
    }

    private void Start()
    {
        StartCoroutine(SpawnTimer());
    }
    
    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTime,maxSpawnTime));

        //Spawning new Item
        if(transform.childCount == 0)
        {
            int randomNumber = Random.Range(0, cmn.icons.Length);
            GameObject newItemObj;
            if (collectablesPool.childCount < 1)
            {
                newItemObj = Instantiate(cmn.baseItem, gameObject.transform.position, Quaternion.identity);                
            }
            else
            {
                newItemObj = collectablesPool.GetChild(0).gameObject;
            }
            newItemObj.transform.parent = transform;
            newItemObj.transform.position = transform.position;

            Item newItem = newItemObj.transform.GetComponent<Item>();
            newItem.icon = cmn.icons[randomNumber];
            newItem.itemType = randomNumber + 1;
            newItem.SetItemPool(cmn.collectablesPool.transform);
            newItem.SetItemIcon(cmn.iconSprites[randomNumber]);

        }
        
        StartCoroutine(SpawnTimer());
    }
}
