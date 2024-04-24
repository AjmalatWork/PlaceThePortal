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
    GameObject button;
    GameObject laserGun;
    GameObject[] stars;
    Vector3 ballPosition;
    Vector3 ballVelocity;
    Vector3 buttonPosition;
    Vector3[] starPosition = new Vector3[3];
    Rigidbody2D ballRb;
    PortalController[] portalControllers;
    PlayerController[] playerControllers;
    BallController ballController;
    LaserButton laserButton;
    Laser laser;
    bool isLaserOn;

    private void Start()
    {
        // Freeze time at the beginning
        Time.timeScale = 0f;

        // Get original positions of game objects
        GetOriginalPosition();

    }

    private void GetOriginalPosition()
    {
        ball = GameObject.FindGameObjectWithTag("Portable");
        ballRb = ball.GetComponent<Rigidbody2D>();
        ballController = ball.GetComponent<BallController>();
        stars = GameObject.FindGameObjectsWithTag("Collectible");

        if (GameObject.FindGameObjectWithTag("Button"))
        {
            button = GameObject.FindGameObjectWithTag("Button");
            laserButton = button.GetComponent<LaserButton>();
            buttonPosition = button.transform.position;
        }

        if (GameObject.FindGameObjectWithTag("Laser"))
        {
            laserGun = GameObject.FindGameObjectWithTag("Laser");
            laser = laserGun.GetComponent<Laser>();
            isLaserOn = laser.isLaserOn;
        }

        // Get original position of ball and button and velocity
        ballPosition = ball.transform.position;
        ballVelocity = ballRb.velocity;

        // Get original positions of stars
        for (int i = 0; i < stars.Length; i++)
        {
            starPosition[i] = stars[i].transform.position;
        }
    }

    private void SetOriginalPosition()
    {
        // Set ball to original position and velocity
        ball.transform.position = ballPosition;
        ballRb.velocity = ballVelocity;

        // Set original position and state of button
        if (button)
        {
            laserButton.isButtonPressed = false;
            button.transform.position = buttonPosition;
        }

        // Set laser state to original state
        if (laser)
        {
            laser.isLaserOn = isLaserOn;
            laser.SwitchLaser(laser.isLaserOn);
        }

        // Set original positions of stars
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(true);
            stars[i].transform.position = starPosition[i];
        }

        // Set starsCollected to zero on restart
        Debug.Log("Stars Collected this run: " + ballController.starsCollected);
        ballController.starsCollected = 0f;
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
        // Portal Placeholder object
        portalPlaceholders = GameObject.FindGameObjectsWithTag("PortalPlaceholder");

        int numberOfPortals = portalPlaceholders.Length;

        // If placeholders are available
        if (numberOfPortals > 0)
        {
            // Portal object
            portals = new GameObject[numberOfPortals];

            // Portal Controller Script
            portalControllers = new PortalController[numberOfPortals];

            // Portal Placeholder Controller Script
            playerControllers = new PlayerController[numberOfPortals];

            // Loop to deactivate portal placeholder and instantiate portal
            for (int i = 0; i < numberOfPortals; i++)
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

            // Loop to add destination of all portals
            for (int i = 0;i < numberOfPortals; i+=2) 
            {
                // If both portals are placed, then add destinations
                if (playerControllers[i].isPortalPlaced && playerControllers[i + 1].isPortalPlaced)
                {
                    portalControllers[i].destination = portalControllers[i + 1].transform;
                    portalControllers[i + 1].destination = portalControllers[i].transform;
                }
                // if only one portal is placed, then remove destination
                else if (playerControllers[i].isPortalPlaced)
                {
                    portalControllers[i].destination = null;
                }
                else if (playerControllers[i + 1].isPortalPlaced)
                {
                    portalControllers[i + 1].destination = null;
                }
            }
        }
    }

    private void deactivatePortals()
    {
        int numberOfPortalPlaceholders = 0;
        int numberOfPortals = 0;

        if (portalPlaceholders != null)
        {
            numberOfPortalPlaceholders = portalPlaceholders.Length;
        }
        
        if(portals != null)
        {
            numberOfPortals = portals.Length;
        }
        

        // If portal placeholders available
        if (numberOfPortalPlaceholders > 0)
        {
            // Set placeholders as active
            for (int i = 0; i < numberOfPortalPlaceholders; i++)
            {
                portalPlaceholders[i].SetActive(true);
            }
        }

        // If portals are available
        if (numberOfPortals > 0)
        {
            // Set placeholders as active
            for (int i = 0; i < numberOfPortals; i++)
            {
                Destroy(portals[i]);
            }
        }
    }
}
