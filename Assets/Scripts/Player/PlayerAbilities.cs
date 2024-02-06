using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
        
    private AudioSource audioSrc;

    private PlayerCommon pCmn;
    private ItemsCommon iCmn;
    private PlayerFlags flags;
    private Transform tran;

    private GameObject catGun;
    private GameObject catLaser;
    private GameObject toothBrush;
    private Transform projectilesPool;
    private Transform activeItemsPool;

    public void SetPlayerCommon(PlayerCommon newPCommon, ItemsCommon newICommon)
    {
        pCmn = newPCommon;
        iCmn = newICommon;
        projectilesPool = iCmn.projectilesPool.transform;
        activeItemsPool = iCmn.activeItemsPool.transform;
    }

    public void SetPlayerFlags(PlayerFlags newFlags)
    {
        flags = newFlags;
    }

    public void SetAudioSource(AudioSource newAudioSrc)
    {
        audioSrc = newAudioSrc;
    }

    public void SetTransform(Transform newTran)
    {
        tran = newTran;
    }

    public void SetPlayerHeldItems(GameObject[] heldItems)
    {
        catGun = heldItems[1];
        catLaser = heldItems[2];
        toothBrush = heldItems[3];
    }

    public void ShootPenguin()
    {
        GameObject throwable = Throw(1);        
        throwable.GetComponent<SpriteRenderer>().sprite = iCmn.penguinProjectile;

        if (throwable.transform.GetComponent<Rigidbody2D>() == null) { return; }

        audioSrc.PlayOneShot(iCmn.penguinSound);
        StartCoroutine(RemoveProjectile(throwable));
    }

    public GameObject Throw(float throwModifier)
    {
        GameObject throwable;
        if (projectilesPool.childCount < 1)
        {
            throwable = Instantiate(iCmn.baseProjectile);
        }
        else
        {
            throwable = projectilesPool.GetChild(0).gameObject;
        }
        throwable.transform.parent = activeItemsPool;
        throwable.SetActive(true);

        if (throwable.transform.GetComponent<Rigidbody2D>() != null)
        {
            if (flags.facingRight)
            {
                throwable.transform.position = tran.position + new Vector3(0.5f, 0, 0);
                throwable.GetComponent<Rigidbody2D>().AddForce(new Vector2(pCmn.throwForce * throwModifier, 50f), ForceMode2D.Impulse);
            }
            else
            {
                throwable.transform.position = tran.position + new Vector3(-3.5f, 0, 0);
                throwable.GetComponent<Rigidbody2D>().AddForce(new Vector2(-pCmn.throwForce * throwModifier, 50f), ForceMode2D.Impulse);
            }
        }
        else
        { return null; }
        return throwable;
    }

    public IEnumerator DoggieGrenade()
    {
        GameObject throwable = Throw(0.3f);
        if(throwable == null) {
            Debug.Log("Doggie Grenade has a null rigidody");
            yield return null; 
        }
        
        throwable.GetComponent<SpriteRenderer>().sprite = iCmn.chihuawaProjectile;
        audioSrc.PlayOneShot(iCmn.dogSound);
        yield return new WaitForSeconds(1f);
        throwable.transform.parent = projectilesPool;
        throwable.SetActive(false);
        //StartCoroutine(RemoveProjectile(throwable));

        //Explosion part

        Debug.Log("Timer Passed with force: " + pCmn.explosionForce + " and explosion radius: " + pCmn.explosionRadius);
            Collider2D[] objects = Physics2D.OverlapCircleAll(throwable.transform.position, pCmn.explosionRadius);
            Debug.Log("objects has found " + objects.Length + " objects");
            
            foreach (Collider2D obj in objects)
            {
                if(obj.gameObject.layer == 8 || obj.gameObject.layer == 7)
                {
                    if(obj.transform.position.x - throwable.transform.position.x > 0)
                    {
                        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(pCmn.explosionForce, pCmn.explosionForce));
                    }
                    else
                    {
                    obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(-pCmn.explosionForce, pCmn.explosionForce));
                }
                    
                    
                    //Debug.Log("Should have applied force to " + obj.ToString() + " with force (" + dir + " * " + pCmn.explosionForce + ") ");
                }
            }
            
            audioSrc.PlayOneShot(iCmn.explosionSound);            
        
        StartCoroutine(RemoveProjectile(throwable));
        flags.canUseItem = true;
    }

    public IEnumerator CatLaser()
    {
        flags.canMove = false; //Stop from moving
        flags.canUseItem = false;

        catGun.SetActive(true);
        

        if (flags.facingRight)
        {
            catGun.transform.GetComponent<SpriteRenderer>().flipX = false;
            catGun.transform.localPosition = new Vector3(-0.246f, 0.033f, 0);
        }
        else
        {
            catGun.transform.GetComponent<SpriteRenderer>().flipX = true;
            catGun.transform.localPosition = new Vector3(-0.66f, 0.033f, 0);
        }
        yield return new WaitForSeconds(0.2f);

        audioSrc.PlayOneShot(iCmn.laserSound);
        catLaser.SetActive(true);

        RaycastHit2D[] hit;
        if (flags.facingRight)
        {
            hit = Physics2D.RaycastAll(tran.position + new Vector3(-2, 0, 0), Vector2.right, Mathf.Infinity);
            catLaser.transform.GetComponent<SpriteRenderer>().flipX = false;
            catLaser.transform.localPosition = new Vector3(9.72f, -0.093f, 0);
        }
        else
        {
            hit = Physics2D.RaycastAll(tran.position + new Vector3(-3, 0, 0), -Vector2.right, Mathf.Infinity);
            catLaser.transform.GetComponent<SpriteRenderer>().flipX = true;
            catLaser.transform.localPosition = new Vector3(-9.72f, -0.093f, 0);
        }

        foreach (RaycastHit2D obj in hit)
        {
            ApplyDirectionalForce(obj.transform.gameObject, pCmn.laserForce);
        }
        yield return new WaitForSeconds(1.0f);
        foreach (RaycastHit2D obj in hit)
        {
            ApplyDirectionalForce(obj.transform.gameObject, pCmn.laserForce / 2);
        }
        yield return new WaitForSeconds(1.0f);
        foreach (RaycastHit2D obj in hit)
        {
            ApplyDirectionalForce(obj.transform.gameObject, pCmn.laserForce / 3);
        }

        catGun.SetActive(false);
        catLaser.SetActive(false);
        flags.canMove = true;
        flags.canUseItem = true;
    }

    public void ApplyDirectionalForce(GameObject target, float ForcePower)
    {
        if (target.transform.GetComponent<Rigidbody2D>() != null)
        {
            if (flags.facingRight)
            {
                target.transform.GetComponent<Rigidbody2D>().AddForce((new Vector2(1, 0.5f) * ForcePower), ForceMode2D.Impulse);
            }
            else
            {
                target.transform.GetComponent<Rigidbody2D>().AddForce((new Vector2(-1, 0.5f) * ForcePower), ForceMode2D.Impulse);
            }
        }

    }

    public IEnumerator ToothbrushAbility()
    {
        
        toothBrush.SetActive(true);

        
        
        if (flags.facingRight)
        {
//            brush.transform.position = tran.position + new Vector3(4.5f, 1, 0);
        }
        else
        {
            // brush.transform.position = tran.position + new Vector3(-9.5f, 1, 0);
            toothBrush.transform.GetComponent<SpriteRenderer>().flipX = true;
        }

        //brush.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        flags.canMove = false;

        float elapsedTime = 0;
        float animTime = 0.4f;
        Quaternion endRot = Quaternion.identity;
        Vector3 endPos = Vector3.zero;
        if (flags.facingRight)
        {
            endRot = Quaternion.Euler(0, 0, 50);
            endPos = toothBrush.transform.localPosition + new Vector3(-0.2f, 0.8f, 0);
        }
        else
        {
            endRot = Quaternion.Euler(0, 0, -50);
            endPos = toothBrush.transform.localPosition + new Vector3(0.2f, 0.8f, 0);
        }
        yield return new WaitForSeconds(0.1f);
        while (elapsedTime < animTime)
        {
            toothBrush.transform.rotation = Quaternion.Lerp(Quaternion.identity, endRot, (elapsedTime / animTime));
            toothBrush.transform.localPosition = Vector3.Lerp(toothBrush.transform.localPosition, endPos, (elapsedTime / animTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }

        //ActualHit happens
        pCmn.meeleForce = pCmn.meeleForce * 2;
        pCmn.meeleRange = pCmn.meeleRange * 1.2f;
        Meele();
        pCmn.meeleForce = pCmn.meeleForce / 2;
        pCmn.meeleRange = pCmn.meeleRange / 1.2f;

        elapsedTime = 0;
        animTime = 0.2f;

        endRot = Quaternion.identity;
        endPos = Vector3.zero;
        audioSrc.PlayOneShot(iCmn.playermeeleSound);

        if (flags.facingRight)
        {
            endRot = Quaternion.Euler(0, 0, -50);
            endPos = toothBrush.transform.localPosition + new Vector3(0.2f, -0.8f, 0);
        }
        else
        {
            endRot = Quaternion.Euler(0, 0, 50);
            endPos = toothBrush.transform.localPosition + new Vector3(-0.2f, -0.8f, 0);
        }

        while (elapsedTime < animTime)
        {
            toothBrush.transform.rotation = Quaternion.Lerp(endRot, Quaternion.identity, (elapsedTime / animTime));
            toothBrush.transform.localPosition = Vector3.Lerp(toothBrush.transform.localPosition, endPos, (elapsedTime / animTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }


        yield return null;
        flags.canMove = true;

        toothBrush.SetActive(false);
    }

    public void Meele()
    {
        if (!flags.canMeele) { return; }
        flags.canUseItem = false;
        //StartCoroutine(MeeleCooldown());
        audioSrc.PlayOneShot(iCmn.playermeeleSound);
        RaycastHit2D hit;
        if (flags.facingRight)
        {
            hit = Physics2D.Raycast(tran.position + new Vector3(-2.75f, 0, 0), Vector2.right);
        }
        else
        {
            hit = Physics2D.Raycast(tran.position + new Vector3(-2.75f, 0, 0), -Vector2.right);
        }


        if (hit.transform == null) { return; }

        if (hit.transform.GetComponent<Rigidbody2D>() != null)
        {
            float distance = Mathf.Abs(hit.point.x - (tran.position.x - 3f));
            if (distance < pCmn.meeleRange)
            {

                if (hit.transform != null && hit.transform.GetComponent<PlayerMove>() != null)
                {
                    Debug.Log("Hitting " + hit.transform.name.ToString());
                    hit.transform.GetComponent<PlayerMove>().StartHitStunned();
                    
                }
                //hit.transform.GetComponent<PlayerInteract>().PlayHitSound(); //Play hit sound on other player
                audioSrc.PlayOneShot(iCmn.playerHitSound);
                // Apply the force to the rigidbody.
                if (flags.facingRight)
                {
                    hit.transform.GetComponent<Rigidbody2D>().AddForce(Vector3.right * (pCmn.meeleForce));
                }
                else
                {
                    hit.transform.GetComponent<Rigidbody2D>().AddForce(-Vector3.right * (pCmn.meeleForce));
                }
            }
        }
        flags.canUseItem = true;
    }

    private IEnumerator RemoveProjectile(GameObject projectile)
    {
        yield return new WaitForSeconds(2f);
        projectile.transform.parent = projectilesPool;
        projectile.SetActive(false);
    }
}
