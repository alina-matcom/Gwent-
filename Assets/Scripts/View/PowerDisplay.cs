using UnityEngine;
using UnityEngine.UI;

public class PowerDisplay : MonoBehaviour
{
    public Texture2D[] numberImages;
    public double power;

    public void UpdateDisplay()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        string powerString = power.ToString();
        for (int i = 0; i < powerString.Length; i++)
        {
            int digit = int.Parse(powerString[i].ToString());
            GameObject rawImageObj = new("DynamicRawImage");
            rawImageObj.transform.SetParent(transform);
            rawImageObj.transform.localPosition = new Vector3(rawImageObj.transform.localPosition.x, rawImageObj.transform.localPosition.y, 0);
            rawImageObj.transform.localScale = new Vector3(1, 1, 1);

            RawImage rawImage = rawImageObj.AddComponent<RawImage>();
            rawImage.texture = numberImages[digit];
            rawImage.SetNativeSize();
        }
    }

    public void SetPower(double newPower)
    {
        power = newPower;
        UpdateDisplay();
    }
}
