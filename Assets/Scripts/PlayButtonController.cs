using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayButtonController : MonoBehaviour, IClickableUI
{
    public GameObject portal;
    [NonSerialized] public bool isPlay = true;

    GameObject[] portals;
    GameObject[] portalPlaceholders;
    PortalController[] portalControllers;
    PlayerController[] playerControllers;

    private List<IResetable> resetableObjects = new();

    private void Start()
    {
        // Freeze time at the beginning
        Time.timeScale = 0f;

        // Populate the list of resettable objects
        FindResetableObjects();

        // Get original positions of game objects
        GetOriginalPosition();

    }

    void FindResetableObjects()
    {
        resetableObjects.Clear();
        // Find all objects with IResettable interface and add them to the list
        resetableObjects.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IResetable>());
    }

    private void GetOriginalPosition()
    {
        // Get original state of resetable objects
        foreach (var obj in resetableObjects)
        {
            obj.GetOriginalState();
        }
    }

    private void SetOriginalPosition()
    {
        // Set original state of resetable objects
        foreach (var obj in resetableObjects)
        {
            obj.SetOriginalState();
        }
    }

    void StartGame()
    {
        // Start time on pressing Play
        Time.timeScale = 1f;
        isPlay = false;

        // Activate portals if portalplaceholders are placed on placeable objects
        activatePortals();
    }

    void ResetGame()
    {
        isPlay = true;

        // Set game objects to their original positions
        SetOriginalPosition();

        // Convert portals back to placeholders on restart
        deactivatePortals();

        // Freeze time on pressing restart
        Time.timeScale = 0f;
    }

    public void OnClick()
    {
        if (isPlay)
        {
            StartGame();
        }
        else
        {
            ResetGame();
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
