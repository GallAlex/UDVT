using UnityEngine;

/// <summary>
/// Class helps defining colors and offers certtain color schemes
/// </summary>
public class ColorHelper
{
    //singleBlueHue Range goes from light blue to dark blue (https://colorbrewer2.org/#type=sequential&scheme=Blues&n=3)
    public static Color[] singleBlueHue = new[] { ReturnColorFromHex("#deebf7"), ReturnColorFromHex("#9ecae1"), ReturnColorFromHex("#3182bd") };
    // singleGreyHue Range goes from light grey to dark grey
    public static Color[] singleGreyHue = new[] { ReturnColorFromHex("#f0f0f0"), ReturnColorFromHex("#bdbdbd"), ReturnColorFromHex("#636363") };
    // singleRedHue Range goes from light red to dark red
    public static Color[] singleRedHue = new[] { ReturnColorFromHex("#fee0d2"), ReturnColorFromHex("#fc9272"), ReturnColorFromHex("#de2d26") };

    public static Color[] blueHueValues = new[] { ReturnColorFromHex("#7BBCFF"), ReturnColorFromHex("#1387FF"), ReturnColorFromHex("#00458D") };
    public static Color[] yellowHueValues = new[] { ReturnColorFromHex("#FFDE70"), ReturnColorFromHex("#FFC500"), ReturnColorFromHex("#D9A700") };
    public static Color[] orangeHueValues = new[] { ReturnColorFromHex("#FFA070"), ReturnColorFromHex("#FF5600"), ReturnColorFromHex("#D94900") };

    /// <summary>
    /// Method takes in string with hex value of color and returns color between 0-1
    /// Strings have to begin with '#' 
    /// </summary>
    /// <param name="hexColor"></param>
    /// <returns></returns>
    public static Color ReturnColorFromHex(string hexColor)
    {
        Color newCol;
        if (!ColorUtility.TryParseHtmlString(hexColor, out newCol)) Debug.LogError("ColorHelper: Color could not be parsed from hex string: " + hexColor);

        return newCol;
    }

    /// <summary>
    /// Method takes in int array with color values between 0-255
    /// If no alpha value is provided it is set to 255
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color ReturnColorFromInt(int[] color)
    {
        Color newCol;

        //Check if color has fourth value
        if (color.Length < 4)
        {
            newCol = new Color(color[0] / 255.0f, color[1] / 255.0f, color[2] / 255.0f, 255.0f);
        }
        else
        {
            newCol = new Color(color[0] / 255.0f, color[1] / 255.0f, color[2] / 255.0f, color[3] / 255.0f);
        }

        return newCol;
    }

}