using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayButtonController : MonoBehaviour
{
    public GameObject portal;
    [NonSerialized] public bool isPlay = true;
    [NonSerialized] public bool isRestart = false;

    GameObject[] portals;
    GameObject[] portalPlaceholders;
    GameObject ball;
    GameObject[] stars;
    Vector3 ballPosition;
    Vector3 ballVelocity;
    Vector3[] starPosition = new Vector3[3];
    Rigidbody2D ballRb;
    PortalController[] portalControllers;
    PlayerController[] playerControllers;
    BallController ballController;

    private void Awake()
    {
        // Freeze time at the beginning
        Time.timeScale = 0f;

        // Get original positions of game objects
        GetOriginalPosition();        
    }

    private void OnMouseDown()
    {
        if (isPlay) 
        {
            // Start time on pressing Play
            Time.timeScale = 1f;
            isPlay = false;
            isRestart = true;

            // Activate portals if portalplaceholders are placed on placeable objects
            activatePortals();
        }

        else if (isRestart)
        {
            // Freeze time on pressing restart
            Time.timeScale = 0f;
            isPlay = true;
            isRestart = false;

            // Set game objects to their original positions
            SetOriginalPosition();

            // Convert portals back to placeholders on restart
            deactivatePortals();
        }
        
    }

    private void activatePortals()
    {
        // Portal object
        portals = new GameObject[2];

        // Portal Placeholder object
        portalPlaceholders = GameObject.FindGameObjectsWithTag("PortalPlaceholder");

        // Portal Controller Script
        portalControllers = new PortalController[2];

        // Portal Placeholder Controller Script
        playerControllers = new PlayerController[2];

        // If placeholders are available
        if (portalPlaceholders.Length > 0)
        {
            for (int i = 0; i < 2; i++)
            {
                // Get script for public object isPortalPlaced
                playerControllers[i] = portalPlaceholders[i].GetComponent<PlayerController>();

                // If portal is placed, then create portal in the place of the placeholder
                if (playerControllers[i].isPortalPlaced)
                {
                    // Create portal at the place of the portal placeholder
                    portals[i] = Instantiate(portal, portalPlaceholders[i].transform.position, portalPlaceholders[i].transform.rotation);

                    // Get script for public object destination
                    portalControllers[i] = portals[i].GetComponent<PortalController>();

                }

                // Deactivate Portal Placeholder
                portalPlaceholders[i].SetActive(false);
            }

            // If both portals are placed, then add destinations
            if (playerControllers[0].isPortalPlaced && playerControllers[1].isPortalPlaced)
            {
                portalControllers[0].destination = portalControllers[1].transform;
                portalControllers[1].destination = portalControllers[0].transform;
            }
            // if only one portal is placed, then remove destination
            else if (playerControllers[0].isPortalPlaced)
            {
                portalControllers[0].destination = null;
            }
            else if (playerControllers[1].isPortalPlaced)
            {
                portalControllers[1].destination = null;
            }
        }
    }

    private void deactivatePortals()
    {
        // If portal placeholders and portals are available
        if (portalPlaceholders.Length > 0 && portals.Length > 0)
        { 
            // Set placeholders as active and destroy portal objects
            for (int i = 0; i < 2; i++)
            {
                portalPlaceholders[i].SetActive(true);
                Destroy(portals[i]);
            }      
        }
    }

    private void GetOriginalPosition()
    {
        ball = GameObject.FindGameObjectWithTag("Portable");
        ballRb = ball.GetComponent<Rigidbody2D>();
        ballController = ball.GetComponent<BallController>();
        stars = GameObject.FindGameObjectsWithTag("Collectible");

        // Get original position of ball and velocity
        ballPosition = ball.transform.position;
        ballVelocity = ballRb.velocity;

        // Get original positions of stars
        for(int i = 0;i< stars.Length; i++) 
        {
            starPosition[i] = stars[i].transform.position;
        }
    }

    private void SetOriginalPosition()
    {
        // Set ball to original position and velocity
        ball.transform.position = ballPosition;
        ballRb.velocity = ballVelocity;

        // Set original positions of stars
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(true);
            stars[i].transform.position = starPosition[i];
        }

        // Set starsCollected to zero on restart
        ballController.starsCollected = 0f;
    }

}
