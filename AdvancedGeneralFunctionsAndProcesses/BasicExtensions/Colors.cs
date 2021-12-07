using CommonBasicLibraries.BasicDataSettingsAndProcesses;
using CommonBasicLibraries.CollectionClasses;
using cs = CommonBasicLibraries.BasicDataSettingsAndProcesses.SColorString;
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
public static class Colors
{
    public static string ToWebColor(this string color)
    {
        if (color == cs.Transparent)
        {
            return "none"; //this is how svg shows as transparent
        }
        if (color.Length == 0)
        {
            throw new CustomBasicException("Had no color");
        }
        if (color.Length != 9)
        {
            throw new CustomBasicException("Color In Wrong Format");
        }
        if (color.StartsWith("#FF") == false)
        {
            throw new CustomBasicException("Colors must start with FF so no transparency");
        }
        string output = $"#{color.Substring(3, 6)}";
        return output;
    }
    public static string ToColor(this string thisStr, bool showError = true)
    {
        switch (thisStr)
        {
            case "Boy":
                return cs.Blue;
            case "Girl":
                return cs.DeepPink; //just in case you are playing the game of life and it needs to be converted to colors.
            case "AliceBlue":
                return cs.AliceBlue;
            case "AntiqueWhite":
                return cs.AntiqueWhite;
            case "Aqua":
                return cs.Aqua;
            case "Aquamarine":
                return cs.Aquamarine;
            case "Azure":
                return cs.Azure;
            case "Beige":
                return cs.Beige;
            case "Bisque":
                return cs.Bisque;
            case "Black":
                return cs.Black;
            case "BlanchedAlmond":
                return cs.BlanchedAlmond;
            case "Blue":
                return cs.Blue;
            case "BlueViolet":
                return cs.BlueViolet;
            case "Brown":
                return cs.Brown;
            case "BurlyWood":
                return cs.BurlyWood;
            case "CadetBlue":
                return cs.CadetBlue;
            case "Chartreuse":
                return cs.Chartreuse;
            case "Chocolate":
                return cs.Chocolate;
            case "Coral":
                return cs.Coral;
            case "CornflowerBlue":
                return cs.CornflowerBlue;
            case "Cornsilk":
                return cs.Cornsilk;
            case "Crimson":
                return cs.Crimson;
            case "Cyan":
                return cs.Cyan;
            case "DarkBlue":
                return cs.DarkBlue;
            case "DarkCyan":
                return cs.DarkCyan;
            case "DarkGoldenrod":
                return cs.DarkGoldenrod;
            case "DarkGray":
                return cs.DarkGray;
            case "DarkGreen":
                return cs.DarkGreen;
            case "DarkKhaki":
                return cs.DarkKhaki;
            case "DarkMagenta":
                return cs.DarkMagenta;
            case "DarkOliveGreen":
                return cs.DarkOliveGreen;
            case "DarkOrange":
                return cs.DarkOrange;
            case "DarkOrchid":
                return cs.DarkOrchid;
            case "DarkRed":
                return cs.DarkRed;
            case "DarkSalmon":
                return cs.DarkSalmon;
            case "DarkSeaGreen":
                return cs.DarkSeaGreen;
            case "DarkSlateBlue":
                return cs.DarkSlateBlue;
            case "DarkSlateGray":
                return cs.DarkSlateGray;
            case "DarkTurquoise":
                return cs.DarkTurquoise;
            case "DarkViolet":
                return cs.DarkViolet;
            case "DeepPink":
                return cs.DeepPink;
            case "DeepSkyBlue":
                return cs.DeepSkyBlue;
            case "DimGray":
                return cs.DimGray;
            case "DodgerBlue":
                return cs.DodgerBlue;
            case "Firebrick":
                return cs.Firebrick;
            case "FloralWhite":
                return cs.FloralWhite;
            case "ForestGreen":
                return cs.ForestGreen;
            case "Fuchsia":
                return cs.Fuchsia;
            case "Gainsboro":
                return cs.Gainsboro;
            case "GhostWhite":
                return cs.GhostWhite;
            case "Gold":
                return cs.Gold;
            case "Goldenrod":
                return cs.Goldenrod;
            case "Gray":
                return cs.Gray;
            case "Green":
                return cs.Green;
            case "GreenYellow":
                return cs.GreenYellow;
            case "Honeydew":
                return cs.Honeydew;
            case "HotPink":
                return cs.HotPink;
            case "IndianRed":
                return cs.IndianRed;
            case "Indigo":
                return cs.Indigo;
            case "Ivory":
                return cs.Ivory;
            case "Khaki":
                return cs.Khaki;
            case "Lavender":
                return cs.Lavender;
            case "LavenderBlush":
                return cs.LavenderBlush;
            case "LawnGreen":
                return cs.LawnGreen;
            case "LemonChiffon":
                return cs.LemonChiffon;
            case "LightBlue":
                return cs.LightBlue;
            case "LightCoral":
                return cs.LightCoral;
            case "LightCyan":
                return cs.LightCyan;
            case "LightGoldenrodYellow":
                return cs.LightGoldenrodYellow;
            case "LightGray":
                return cs.LightGray;
            case "LightGreen":
                return cs.LightGreen;
            case "LightPink":
                return cs.LightPink;
            case "LightSalmon":
                return cs.LightSalmon;
            case "LightSeaGreen":
                return cs.LightSeaGreen;
            case "LightSkyBlue":
                return cs.LightSkyBlue;
            case "LightSlateGray":
                return cs.LightSlateGray;
            case "LightSteelBlue":
                return cs.LightSteelBlue;
            case "LightYellow":
                return cs.LightYellow;
            case "Lime":
                return cs.Lime;
            case "LimeGreen":
                return cs.LimeGreen;
            case "Linen":
                return cs.Linen;
            case "Magenta":
                return cs.Magenta;
            case "Maroon":
                return cs.Maroon;
            case "MediumAquamarine":
                return cs.MediumAquamarine;
            case "MediumBlue":
                return cs.MediumBlue;
            case "MediumOrchid":
                return cs.MediumOrchid;
            case "MediumPurple":
                return cs.MediumPurple;
            case "MediumSeaGreen":
                return cs.MediumSeaGreen;
            case "MediumSlateBlue":
                return cs.MediumSlateBlue;
            case "MediumSpringGreen":
                return cs.MediumSpringGreen;
            case "MediumTurquoise":
                return cs.MediumTurquoise;
            case "MediumVioletRed":
                return cs.MediumVioletRed;
            case "MidnightBlue":
                return cs.MidnightBlue;
            case "MintCream":
                return cs.MintCream;
            case "MistyRose":
                return cs.MistyRose;
            case "Moccasin":
                return cs.Moccasin;
            case "NavajoWhite":
                return cs.NavajoWhite;
            case "Navy":
                return cs.Navy;
            case "OldLace":
                return cs.OldLace;
            case "Olive":
                return cs.Olive;
            case "OliveDrab":
                return cs.OliveDrab;
            case "Orange":
                return cs.Orange;
            case "OrangeRed":
                return cs.OrangeRed;
            case "Orchid":
                return cs.Orchid;
            case "PaleGoldenrod":
                return cs.PaleGoldenrod;
            case "PaleGreen":
                return cs.PaleGreen;
            case "PaleTurquoise":
                return cs.PaleTurquoise;
            case "PaleVioletRed":
                return cs.PaleVioletRed;
            case "PapayaWhip":
                return cs.PapayaWhip;
            case "PeachPuff":
                return cs.PeachPuff;
            case "Peru":
                return cs.Peru;
            case "Pink":
                return cs.Pink;
            case "Plum":
                return cs.Plum;
            case "PowderBlue":
                return cs.PowderBlue;
            case "Purple":
                return cs.Purple;
            case "Red":
                return cs.Red;
            case "RosyBrown":
                return cs.RosyBrown;
            case "RoyalBlue":
                return cs.RoyalBlue;
            case "SaddleBrown":
                return cs.SaddleBrown;
            case "Salmon":
                return cs.Salmon;
            case "SandyBrown":
                return cs.SandyBrown;
            case "SeaGreen":
                return cs.SeaGreen;
            case "SeaShell":
                return cs.SeaShell;
            case "Sienna":
                return cs.Sienna;
            case "Silver":
                return cs.Silver;
            case "SkyBlue":
                return cs.SkyBlue;
            case "SlateBlue":
                return cs.SlateBlue;
            case "SlateGray":
                return cs.SlateGray;
            case "Snow":
                return cs.Snow;
            case "SpringGreen":
                return cs.SpringGreen;
            case "SteelBlue":
                return cs.SteelBlue;
            case "Tan":
                return cs.Tan;
            case "Teal":
                return cs.Teal;
            case "Thistle":
                return cs.Thistle;
            case "Tomato":
                return cs.Tomato;
            case "None":
            case "Transparent":
                return cs.Transparent;
            case "Turquoise":
                return cs.Turquoise;
            case "Violet":
                return cs.Violet;
            case "Wheat":
                return cs.Wheat;
            case "White":
                return cs.White;
            case "WhiteSmoke":
                return cs.WhiteSmoke;
            case "Yellow":
                return cs.Yellow;
            case "YellowGreen":
                return cs.YellowGreen;
            default:
                if (showError == true)
                {
                    throw new CustomBasicException($"No Color Found For {thisStr}");
                }
                else
                {
                    return "";
                }
        }
    }
    public static string ToColor<E>(this E thisEnum) where E : Enum
    {
        string thisStr = thisEnum.ToString();
        return thisStr.ToColor();
    }
    internal static bool HasColor<E>(this string thisStr) where E : Enum
    {
        string thisColor = thisStr.ToColor(false);
        return thisColor != "";
    }
    public static BasicList<E> GetColorList<E>(this E thisEnum) where E : Enum
    {
        var firsts = Enum.GetValues(thisEnum.GetType());
        BasicList<E> output = new();
        foreach (var thisItem in firsts)
        {
            if (thisItem.ToString()!.HasColor<E>() == true)
            {
                output.Add((E)thisItem);
            }
        }
        return output;
    }
}