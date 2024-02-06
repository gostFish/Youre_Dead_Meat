using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCommon : MonoBehaviour
{
    [Header("Instance pools")]
    public GameObject collectablesPool;
    public GameObject activeItemsPool;
    public GameObject projectilesPool;

    [Header("Base Items for spawning")]
    public GameObject baseItem;
    public GameObject baseProjectile;

    [Header("Textures/Sprites")]
    public Sprite penguinProjectile;
    public Sprite chihuawaProjectile;
    
    public Texture[] icons;
    public Sprite[] iconSprites;

    [Header("Audio Clips")]
    public AudioClip explosionSound;
    public AudioClip penguinSound;
    public AudioClip dogSound;
    public AudioClip laserSound;
    public AudioClip pillowSound;
    public AudioClip playerHitSound;
    public AudioClip playermeeleSound;
}
