using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpace : MonoBehaviour
{

    private GameObject owningPlayer;
    [SerializeField] private Item itemHeld;

    private Texture defaultTexture;

    private EnvCommon env;
    private RawImage img;

    private void Awake()
    {
        env = GameObject.FindGameObjectWithTag("GameController").transform.GetComponent<EnvCommon>();
        defaultTexture = env.invSlotTexture;
        img = transform.GetComponent<RawImage>();
    }

    public void SetOwningPlayer(GameObject newOwningPlayer)
    {
        owningPlayer = newOwningPlayer;
    }
    public GameObject GetOwningPlayer()
    {
        return owningPlayer;
    }
    public void SetItemHeld(Item newItemHeld)
    {
        itemHeld = newItemHeld;
    }
    public Item GetItemHeld()
    {
        return itemHeld;
    }

    public int GetItemType()
    {
        if(itemHeld == null)
        {
            return 0;
        }
        return itemHeld.itemType;
    }

    public void ResetSpace()
    {
        itemHeld.ItemUseFinished();
        img.texture = defaultTexture;
        itemHeld = null;
    }

    public void SetNewItem(GameObject newItemObj)
    {
        Item newItem = newItemObj.GetComponent<Item>();
        newItem.ItemCollected();
        itemHeld = newItem;
        img.texture = newItem.icon;
        newItemObj.SetActive(false);
    }

}
