using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCommon : MonoBehaviour
{
    public GameObject collectablesPool;
    public GameObject activeItemsPool;
    public GameObject projectilesPool;

    public GameObject baseItem;
    public GameObject baseProjectile;

    public Sprite penguinProjectile;
    public Sprite chihuawaProjectile;

    public Texture[] icons;
    public Sprite[] iconSprites;

    public AudioClip explosionSound;
    public AudioClip penguinSound;
    public AudioClip dogSound;
    public AudioClip laserSound;
    public AudioClip pillowSound;
    public AudioClip playerHitSound;
    public AudioClip playermeeleSound;
}
