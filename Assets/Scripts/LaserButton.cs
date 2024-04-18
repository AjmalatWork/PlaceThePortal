using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class LaserButton : MonoBehaviour
{
    Vector3 buttonUpPosition;
    Vector3 buttonDownPosition;
    bool isButtonPressed = false;
    public float buttonDelay = 0.5f;
    public float moveSpeed = 5f;

    void Awake()
    {
        buttonUpPosition = transform.position;
        buttonDownPosition = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 3, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Portable") && !isButtonPressed)
        {
            StartCoroutine(ButtonPressCoroutine());         
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Portable") && isButtonPressed)
        {
            StartCoroutine(ButtonReleaseCoroutine());            
        }
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

