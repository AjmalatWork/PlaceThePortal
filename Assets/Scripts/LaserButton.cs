using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class LaserButton : MonoBehaviour
{
    Vector3 buttonUpPosition;
    Vector3 buttonDownPosition;
    [NonSerialized] public bool isButtonPressed = false;
    public float buttonDelay = 1f;
    public float moveSpeed = 5f;
    float lastCollisionTime;
    float currentCollisionTime;
    float timeSinceLastCollision;
    Laser laser;
    public GameObject laserGun;

    void Awake()
    {
        buttonUpPosition = transform.position;
        buttonDownPosition = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 3, transform.position.z);
        lastCollisionTime = Time.time;
        laser = laserGun.GetComponent<Laser>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Portable") && !isButtonPressed)
        {
            PressButton();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Portable") && isButtonPressed)
        {
            if(timeSinceLastCollision > 1.5f * buttonDelay)
            {
                StartCoroutine(ButtonReleaseCoroutine());
            }            
        }
    }

    void PressButton()
    {
        currentCollisionTime = Time.time;
        timeSinceLastCollision = currentCollisionTime - lastCollisionTime;
        lastCollisionTime = currentCollisionTime;

        StartCoroutine(ButtonPressCoroutine());

        laser.SwitchLaser(!laser.isLaserOn);      
    }

    private IEnumerator ButtonPressCoroutine()
    {
        isButtonPressed = true;
        while (transform.position != buttonDownPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, buttonDownPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator ButtonReleaseCoroutine()
    {
        yield return new WaitForSeconds(buttonDelay);
        while (transform.position != buttonUpPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, buttonUpPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isButtonPressed = false;
    }

}

