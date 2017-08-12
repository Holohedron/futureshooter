using UnityEngine;

public static class Utility
{
    public static float getZOnSphere(float x, float y, float r)
    {
        return Mathf.Sqrt((r * r) - (x * x) - (y * y)) / 2;
    }

    public static bool mouseChanged()
    {
        return (Mathf.Abs(Input.GetAxis("Mouse X")) > 0 && Mathf.Abs(Input.GetAxis("Mouse Y")) > 0);
    }

    public static bool rightStickChanged()
    {
        return (Mathf.Abs(Input.GetAxis("RightHorizontal")) > 0 && Mathf.Abs(Input.GetAxis("RightVertical")) > 0);
    }
}
