using System.Collections.Generic;
using UnityEngine;

public static class FileConstants
{
    public const string Play            = "Play";
    public const string Replay          = "Replay";
    public const string HappySnowman    = "HappySnowman";
    public const string End             = "End";
    public const string PortalIcon      = "PortalIcon";
    public const string PortalAnim      = "PortalAnim_";
    public const string Portal          = "Portal";
}

public static class ValueConstants
{
    public const float alphaTransparent = 0.3f;
    public const float alphaOpaque      = 1f;
}

public static class NameConstants
{
    public const string Ball        = "Ball";
    public const string StarBurst   = "StarBurst";
    public const string Portal      = "Portal";
    public const string PortalFX    = "PortalFX";
    public const string LaserChild  = "LaserChild";
    public const string Orange      = "Orange";
    public const string Green       = "Green";
    public const string Blue        = "Blue";
    public const string ColorCode   = "ColorCode";
}

public static class TagConstants
{
    public const string Portable            = "Portable";
    public const string Placeable           = "Placeable";
    public const string PortalIcon          = "PortalIcon";
    public const string PortalPlaceholder   = "PortalPlaceholder";
    public const string Goal                = "Goal";
    public const string Collectible         = "Collectible";
    public const string PlayButton          = "PlayButton";
    public const string Button              = "Button";
    public const string Laser               = "Laser";
}

public static class Colors
{
    public const string Orange  = "#D0420C";
    public const string Green   = "#00A410";
    public const string Blue    = "#2D66E1";
}

public static class AnimationConstants
{
    public const string FadeIn  = "FadeIn";
    public const string FadeOut = "FadeOut";
}

public static class SFX
{
    public const int LevelEnd       = 0;
    public const int Teleport       = 1;
    public const int PlacePortal    = 2;
}

public static class VectorConstants
{
    public static readonly Vector3 playButtonA          = new(-196f, 659f, 0f);
    public static readonly Vector3 playButtonB          = new(-282f, 745f, 0f);
    public static readonly Vector3 playButtonRotation   = new(0f, 0f, 225f);
    public static readonly Vector3 DefaultScale         = new(1f, 1f, 1f);

    public static readonly Vector3 PortalIconA          = new(105f, 70f, 0f);
    public static readonly Vector3 PortalIconB          = new(200f, 70f, 0f);
    public static readonly Vector3 PortalIconRotation   = new(0f, 0f, 90f);
    public static readonly Vector3 PortalIconScale      = new(-1f, 1f, 1f);

    public static readonly Vector3 PortalIconOffScale      = new(0.1f, 0.1f, 0.1f);
}

public static class TextVectorConstants
{
    public static readonly Vector3 TextPosLevel1    = new(-158f, -100f, 0f);
    public static readonly Vector3 TextPosLevel2    = Vector3.zero;
    public static readonly Vector3 TextPosLevel3    = new(-141f, 1116f, 0f);
    public static readonly Vector3 TextPosLevel4    = Vector3.zero;
    public static readonly Vector3 TextPosLevel5    = new(5f, 743f, 0f);
    public static readonly Vector3 TextPosLevel6    = Vector3.zero;
    public static readonly Vector3 TextPosLevel7    = new(-234f, -190f, 0f);
    public static readonly Vector3 TextPosLevel8    = new(-259f, 305f, 0f);
    public static readonly Vector3 TextPosLevel9    = Vector3.zero;
    public static readonly Vector3 TextPosLevel10   = Vector3.zero;
    public static readonly Vector3 TextPosLevel11   = Vector3.zero;
    public static readonly Vector3 TextPosLevel12   = new(-137f, -93f, 0f);

    public static readonly Vector3[] TutorialTextPos =
    {
        Vector3.zero,
        TextPosLevel1,
        TextPosLevel2,
        TextPosLevel3,
        TextPosLevel4,
        TextPosLevel5,
        TextPosLevel6,
        TextPosLevel7,
        TextPosLevel8,
        TextPosLevel9,
        TextPosLevel10,
        TextPosLevel11,
        TextPosLevel12
    };

}

public static class TextConstants
{
    public const string EmptyString = "";
    public const string LevelText1  = "Place the portals on platform and wall surfaces to guide Snowy's head to its body";
    public const string LevelText2  = "";
    public const string LevelText3  = "Portal cannot be placed on small platforms";
    public const string LevelText4  = "";
    public const string LevelText5  = "Same color portals are connected";
    public const string LevelText6  = "";
    public const string LevelText7  = "Lasers are deadly!";
    public const string LevelText8  = "Pressing the Button can turn lasers on and off";
    public const string LevelText9  = "";
    public const string LevelText10 = "";
    public const string LevelText11 = "";
    public const string LevelText12 = "";

    public static readonly string[] TutorialText = 
    { 
        EmptyString,
        LevelText1, 
        LevelText2, 
        LevelText3, 
        LevelText4, 
        LevelText5, 
        LevelText6, 
        LevelText7, 
        LevelText8, 
        LevelText9, 
        LevelText10, 
        LevelText11, 
        LevelText12
    };
    
}

public static class Mapping
{
    public static readonly Dictionary<string, string> PortalColorToSprite = new()
    {
        { NameConstants.Orange, FileConstants.PortalIcon + NameConstants.Orange },
        { NameConstants.Blue, FileConstants.PortalIcon + NameConstants.Blue },
        { NameConstants.Green, FileConstants.PortalIcon + NameConstants.Green },
    };

    public static readonly Dictionary<string, string> PortalColorToAnim = new()
    {
        { NameConstants.Orange, FileConstants.PortalAnim + NameConstants.Orange },
        { NameConstants.Blue, FileConstants.PortalAnim + NameConstants.Blue },
        { NameConstants.Green, FileConstants.PortalAnim + NameConstants.Green },
    };

    public static readonly Dictionary<string, string> ColorNameToHex = new()
    {
        { NameConstants.Orange, Colors.Orange },
        { NameConstants.Blue, Colors.Blue },
        { NameConstants.Green, Colors.Green },
    };
}