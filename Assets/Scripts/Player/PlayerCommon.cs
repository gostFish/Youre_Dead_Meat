using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommon : MonoBehaviour
{
   
    //Todo Define as Const/Final

    public Texture liveHeartTexture;
    public Texture deadHeartTexture;

    //public Sprite catGun;
   // public Sprite toothBrushSprite;

    //public GameObject laser;
    


    public GameObject canPlayerCollectwidget;

    public Texture defaultIcon;

    // public GameObject nearbyItem;

    public float explosionForce;
    public float explosionRadius;
    public float throwForce;
    public float laserForce;
    public float meeleForce;
    public float meeleRange;

    public float speed;
    public float jumpForce;

    

    public float animationSpeed;
    public float respawnTime;
    //public int maxLives;


    //public Animator playerAnimator;
    public GameObject[] spawnPoints;
    public GameObject[] playerWinScreens;
    public GameObject[] playerInventories;
    public GameObject[] respawnTimers;
    public GameObject[] lifeIcons;
    public GameObject[] controlIcons;

    //Instance pools
    public GameObject playerPool;
    public GameObject projectilesPool;
    public GameObject tempItemsPool;
    public GameObject othersPool;


}
