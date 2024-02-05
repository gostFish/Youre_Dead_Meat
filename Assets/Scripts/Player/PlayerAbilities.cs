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

        flags.canUseItem = false;
        audioSrc.PlayOneShot(iCmn.penguinSound);
        

        GameObject throwable;
        if(projectilesPool.childCount < 1)
        {
            throwable = Instantiate(iCmn.baseProjectile);
        }
        else
        {
            throwable = projectilesPool.GetChild(0).gameObject;
        }
        throwable.transform.parent = activeItemsPool;
        throwable.GetComponent<SpriteRenderer>().sprite = iCmn.penguinProjectile;
        throwable.SetActive(true);

        if (throwable.transform.GetComponent<Rigidbody2D>() == null) { return; }

        if (flags.facingRight)
        {
            throwable.transform.position = tran.position + new Vector3(0.5f, 0, 0);
            throwable.GetComponent<Rigidbody2D>().AddForce(new Vector2(pCmn.throwForce, 0f), ForceMode2D.Impulse);
        }
        else
        {
            throwable.transform.position = tran.position + new Vector3(-3.5f, 0, 0);
            throwable.GetComponent<Rigidbody2D>().AddForce(new Vector2(-pCmn.throwForce, 0f), ForceMode2D.Impulse);
        }

        StartCoroutine(RemoveProjectile(throwable));        
        flags.canUseItem = true;
    }

    public IEnumerator DoggieGrenade()
    {
        flags.canUseItem = false;
        GameObject throwable;
        if (projectilesPool.childCount < 1)
        {
            throwable = Instantiate(iCmn.baseProjectile);
        }
        else
        {
            throwable = projectilesPool.GetChild(0).gameObject;
        }
        Debug.Log("Item thrown");
        throwable.transform.parent = activeItemsPool;
        throwable.GetComponent<SpriteRenderer>().sprite = iCmn.chihuawaProjectile;
        throwable.SetActive(true);

        if (throwable.transform.GetComponent<Rigidbody2D>() != null)
        {
            if (flags.facingRight)
            {
                throwable.transform.position = tran.position + new Vector3(0.5f, 0, 0);
                throwable.GetComponent<Rigidbody2D>().AddForce(new Vector2(pCmn.throwForce / 3.5f, 50f), ForceMode2D.Impulse);
            }
            else
            {
                throwable.transform.position = tran.position + new Vector3(-3.5f, 0, 0);
                throwable.GetComponent<Rigidbody2D>().AddForce(new Vector2(-pCmn.throwForce / 3.5f, 50f), ForceMode2D.Impulse);
            }
            audioSrc.PlayOneShot(iCmn.dogSound);
            flags.canUseItem = true;

            yield return new WaitForSeconds(1f);
            Debug.Log("Timer Passed with force: " + pCmn.explosionForce + " and explosion radius: " + pCmn.explosionRadius);
            Collider2D[] objects = Physics2D.OverlapCircleAll(throwable.transform.position, pCmn.explosionRadius, 8);
            Debug.Log("objects has found " + objects.Length + " objects");
            foreach (Collider2D obj in objects)
            {
                Vector2 dir = obj.transform.position - throwable.transform.position;
                obj.GetComponent<Rigidbody2D>().AddForce(dir * pCmn.explosionForce);
                Debug.Log("Should have applied force to " + obj.ToString() + " with force (" + dir + " * " + pCmn.explosionForce + ") ");
            }
            Collider2D[] players = Physics2D.OverlapCircleAll(throwable.transform.position, pCmn.explosionRadius, 7);
            Debug.Log("Players has found " + players.Length + " objects");
            foreach (Collider2D obj in players)
            {
                Vector2 dir = obj.gameObject.transform.position - throwable.transform.position;
                Debug.Log("Should have applied force to " + obj.gameObject.ToString() + " with force (" + dir + " * " + pCmn.explosionForce + ") ");
                if (obj.gameObject.transform.parent.transform.GetComponent<Rigidbody2D>() != null)
                {
                    Debug.Log("Explosion happening");
                    obj.gameObject.transform.parent.transform.GetComponent<Rigidbody2D>().AddForce(dir * pCmn.explosionForce);
                }
            }
            audioSrc.PlayOneShot(iCmn.explosionSound);
        }
        //Destroy(throwable);
        StartCoroutine(RemoveProjectile(throwable));
        flags.canUseItem = true;
    }

    public IEnumerator CatLaser()
    {

        flags.canMove = false; //Stop from moving
        flags.canUseItem = false;

        catGun.SetActive(true);
        /*if (laserGun.transform.childCount > 0)
        {
            laserGun.transform.GetChild(0).gameObject.SetActive(false);
        }
        laserGun.SetActive(true);
        laserGun.transform.GetComponent<Rigidbody2D>().simulated = false;
        laserGun.transform.GetComponent<Rigidbody2D>().isKinematic = true;
        laserGun.transform.GetComponent<SpriteRenderer>().enabled = true;
        laserGun.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);*/


        //laserGun.GetComponent<SpriteRenderer>().sprite = cmn.catGun;
        //laserGun.GetComponent<SpriteRenderer>().sortingOrder = 1;

        if (flags.facingRight)
        {
            //laserGun.transform.position = laserGun.transform.position + new Vector3(-1.2f, 0.6f, 0);
            catGun.transform.GetComponent<SpriteRenderer>().flipX = false;
            catGun.transform.localPosition = new Vector3(-0.246f, 0.033f, 0);
        }
        else
        {
            //laserGun.transform.position = laserGun.transform.position + new Vector3(-3.5f, 0.6f, 0);
            //catGun.transform.GetComponent<SpriteRenderer>().flipX = true;
            catGun.transform.GetComponent<SpriteRenderer>().flipX = true;
            catGun.transform.localPosition = new Vector3(-0.66f, 0.033f, 0);
        }
        yield return new WaitForSeconds(0.2f);

        audioSrc.PlayOneShot(iCmn.laserSound);
        RaycastHit2D[] hit;
        //GameObject laserInst = Instantiate(cmn.laser, tran.position, Quaternion.identity);
        catLaser.SetActive(true);
        //laserInst.GetComponent<SpriteRenderer>().sortingOrder = 2;
        //laserInst.SetActive(true);

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

        //Destroy(laserGun);
        //Destroy(laserInst);
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
            endPos = toothBrush.transform.localPosition + new Vector3(-0.2f, 3.8f, 0);
        }
        else
        {
            endRot = Quaternion.Euler(0, 0, -50);
            endPos = toothBrush.transform.localPosition + new Vector3(0.2f, 3.8f, 0);
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
            endPos = toothBrush.transform.localPosition + new Vector3(0.2f, -3.8f, 0);
        }
        else
        {
            endRot = Quaternion.Euler(0, 0, 50);
            endPos = toothBrush.transform.localPosition + new Vector3(-0.2f, -3.8f, 0);
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
