using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class is singleton as only one playbutton can be present in each level
public class PlayButtonController : BaseUIButton, IClickableUI
{
    public GameObject portal;
    [NonSerialized] public bool isPlay = true;
    [NonSerialized] public float playPressedTime;

    GameObject[] portals;
    GameObject[] portalPlaceholders;
    PortalController[] portalControllers;
    PlayerController[] playerControllers;
    Image image;

    List<IResetable> resetableObjects = new();
    List<int> destinationSetIndex = new();
    public static PlayButtonController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }

        image = GetComponent<Image>();
    }

    public void RegisterResetableObject(IResetable resetableObject)
    {
        resetableObjects.Add(resetableObject);
    }

    public void UnregisterResetableObject(IResetable resetableObject)
    {
        resetableObjects.Remove(resetableObject);
    }

    private void Start()
    {
        // Freeze time at the beginning
        Time.timeScale = 0f;

        // Get original positions of game objects
        GetOriginalPosition();

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
        playPressedTime = Time.time;

        // Activate portals if portalplaceholders are placed on placeable objects
        ActivatePortals();
    }

    void ResetGame()
    {
        isPlay = true;

        // Set game objects to their original positions
        SetOriginalPosition();

        // Convert portals back to placeholders on restart
        DeactivatePortals();

        // Freeze time on pressing restart
        Time.timeScale = 0f;
    }

    public new void OnClick()
    {
        if (isPlay)
        {
            image.sprite = Resources.Load<Sprite>(FileConstants.Replay);
            StartGame();
        }
        else
        {            
            image.sprite = Resources.Load<Sprite>(FileConstants.Play);
            ResetGame();
        }

    }

    private void ActivatePortals()
    {        
        // Portal Placeholder object
        portalPlaceholders = GameObject.FindGameObjectsWithTag(TagConstants.PortalPlaceholder);

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
                    portals[i] = ObjectPooler.Instance.SpawnFromPool(NameConstants.Portal, portalPlaceholders[i].transform.position, portalPlaceholders[i].transform.rotation, playerControllers[i]);

                    // Get script for public object destination
                    portalControllers[i] = portals[i].GetComponent<PortalController>();
                }

                // Deactivate Portal Placeholder
                portalPlaceholders[i].SetActive(false);
            }            

            // New logic for adding destination to portals based on color
            for (int i = 0; i < numberOfPortals; i++)
            {
                if (destinationSetIndex.Contains(i))
                    continue;

                if (playerControllers[i].isPortalPlaced)
                {
                    // Set destination as null if portal is placed
                    portalControllers[i].destination = null;

                    for (int j = i + 1; j < numberOfPortals; j++)
                    {
                        // if color is not same, go to next portal
                        if (playerControllers[i].color != playerControllers[j].color)
                            continue;

                        // if portal is not placed, go to next portal
                        if (!playerControllers[j].isPortalPlaced)
                            continue;

                        // Set destination
                        portalControllers[j].destination = portalControllers[i].transform;
                        portalControllers[i].destination = portalControllers[j].transform;

                        destinationSetIndex.Add(j);
                        destinationSetIndex.Add(i);
                        break;                        
                    }                        
                }
            }
            destinationSetIndex.Clear();
        }
    }

    private void DeactivatePortals()
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
            // Destroy portals
            for (int i = 0; i < numberOfPortals; i++)
            {
                if(portals[i] != null)
                {
                    portals[i].SetActive(false);
                }              
            }
        }
    }
}
