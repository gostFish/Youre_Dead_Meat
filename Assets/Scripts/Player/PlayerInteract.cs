using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInteract : MonoBehaviour
{
    private GameObject GameController;
    private PlayerAbilities Abilities;

    private PlayerCommon pCmn;
    private ItemsCommon iCmn;

    private PlayerFlags flags;
    private PlayerManager pManager;

    private GameObject canPlayerCollectwidget;
    private InventorySpace[] inventory;
    private int invIndex;

    private GameObject nearbyItem; 
    
    private AudioSource audioSrc;       

    private GameObject[] opponentPlayers;
    private GameObject[] playerItems;
    


    void Awake()
    {        
        GameController = GameObject.FindGameObjectWithTag("GameController");
        Abilities = transform.GetComponent<PlayerAbilities>();

        pCmn = GameController.GetComponent<PlayerCommon>();
        iCmn = GameController.GetComponent<ItemsCommon>();
        flags = transform.parent.GetComponent<PlayerFlags>();
        pManager = transform.parent.GetComponent<PlayerManager>();


        canPlayerCollectwidget = transform.GetChild(0).gameObject;
        canPlayerCollectwidget.SetActive(false);
        flags.canMeele = true;
        flags.canUseItem = true;
        flags.canCollect = true;
        invIndex = -1;
        audioSrc = transform.GetComponent<AudioSource>();

        Abilities.SetPlayerCommon(pCmn,iCmn);
        Abilities.SetPlayerFlags(flags);
        Abilities.SetAudioSource(audioSrc);
        Abilities.SetTransform(transform);


        //Assign all opponents
        int tempIndex = 0;
        opponentPlayers = new GameObject[pCmn.playerPool.transform.childCount - 1];
        for (int i = 0; i < pCmn.playerPool.transform.childCount;i++)
        {
            if(pCmn.playerPool.transform.GetChild(i) != transform.parent)
            {
                opponentPlayers[tempIndex] = pCmn.playerPool.transform.GetChild(i).gameObject;
                tempIndex++;
            }            
        }

        //Assign Inventory
        inventory = pCmn.playerInventories[pManager.playerNumber].GetComponentsInChildren<InventorySpace>();

        playerItems = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            playerItems[i] = transform.GetChild(i).gameObject;              
        }
        Abilities.SetPlayerHeldItems(playerItems);



    }

    public void CollectItem()
    {
        if (nearbyItem != null && flags.canCollect)
        {
            invIndex++;
            if(invIndex >= inventory.Length)
            {
                pManager.PlayerWins();
            }

            inventory[invIndex].SetNewItem(nearbyItem.GetComponent<Item>());
            nearbyItem.GetComponent<Item>().ItemFinished();

            StartCoroutine(CollectCooldown());
        }
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {        
            return;
        }

        if (context.performed && flags.canUseItem)
        {
            switch (inventory[invIndex].GetItemType())
            {
                case 0:
                    Debug.Log("Null item used");
                    break;
                case 1:                    
                    Abilities.ShootPenguin();
                    break;
                case 2:
                    StartCoroutine(Abilities.CatLaser());
                    break;
                case 3:
                    StartCoroutine(Abilities.DoggieGrenade());
                    break;
                case 4:
                    StartCoroutine(Abilities.ToothbrushAbility());
                    break;
                case 5:
                    //Applied to every opponent (can make to random player?)
                    foreach (GameObject opponent in opponentPlayers)
                    {
                        StartCoroutine(opponent.GetComponent<PlayerMove>().SwapControls());
                    }                    
                    break;
                default:
                    Debug.Log("Item unnacounted for");
                    break;
            }
            if(invIndex >= 0 && invIndex < inventory.Length)
            {
                inventory[invIndex].ResetSpace();
                invIndex--;
            }
            
        }
    }
        
    

    private void OnTriggerStay2D(Collider2D collision)
    {     
       if(collision.transform.gameObject.layer == 8)
       {
            canPlayerCollectwidget.SetActive(true);
            nearbyItem = collision.transform.gameObject;
       }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == 8)
        {
            canPlayerCollectwidget.SetActive(false);
            nearbyItem = null;
        }
    }

    

    IEnumerator CollectCooldown()
    {
        flags.canCollect = false;
        yield return new WaitForSeconds(0.8f);
        flags.canCollect = true;
    }

    public void PlayHitSound()
    {
        audioSrc.PlayOneShot(iCmn.playerHitSound);
    }

    public void PlayPillowsound()
    {
        audioSrc.PlayOneShot(iCmn.pillowSound);
    }

}
