# PlaceThePortal
 A puzzle game made in unity for Android

All screipts are in the folder Assets->Scripts
Below is the rundown for what each script does and what object is is attached to:

Script Name                 Attached To               Function
VictoryCondition            Ball                      To check if vicotry condition is achieved
PortalController            Portal                    For the logic of teleportation between portals conserving the direction and velocity of the object
PortalIconController        PortalIcon                Clicking on Portal Icon generates two Portal Placeholders
PlayerController            PortalPlaceholder         For the logic of dragging and placing the placeholders
PlayButtonController        PlayButton                For the logic of play and restart to freeze and resume time
