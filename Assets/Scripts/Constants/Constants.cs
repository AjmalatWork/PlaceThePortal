using System.Collections.Generic;

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