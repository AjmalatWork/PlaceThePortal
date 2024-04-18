# PlaceThePortal
 A puzzle game made in unity for Android

All scripts are in the folder Assets->Scripts
Below is the rundown for what each script does and what object is is attached to:

// In this format
Script Name
Attached To Object
Function

**********************************************************************
BallController           
Ball                      
To check if victory condition is achieved and collection of stars by Ball
**********************************************************************
PortalController            
Portal                    
For the logic of teleportation between portals conserving the direction and velocity of the object
**********************************************************************
PortalIconController        
PortalIcon                
Clicking on Portal Icon generates two Portal Placeholders
**********************************************************************
PlayerController            
PortalPlaceholder         
For the logic of dragging and placing the placeholders
**********************************************************************
PlayButtonController        
PlayButton                
For the logic of play and restart to freeze and resume time and reset all positions
**********************************************************************
CameraController        
Camera              
To control the camera movement for larger levels
**********************************************************************
Laser      
LaserGun                
For the logic of drawing the laser and switching it on and off
**********************************************************************
LaserButton      
LaserButton                
For the logic of pressing the button and controlling the laser with it
**********************************************************************
