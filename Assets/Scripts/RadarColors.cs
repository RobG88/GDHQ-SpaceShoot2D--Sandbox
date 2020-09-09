using System.Collections;
using UnityEngine;

// Flash ENEMIES and/or BOSSES when they first 
// enter the radar field of view
// use IENumerator to flash colors


// Radar GameObject or Sprites need to be placed on the RADAR LAYER
public class RadarColors : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    string radarTag; // use transform.parent.gameobject.tag NOT Transform.root.tag;
    private Color _color;


    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        radarTag = transform.parent.gameObject.tag;

        switch (radarTag)
        {
            case "Player":
                ColorUtility.TryParseHtmlString("#000080", out _color); // Blue
                break;
            case "Enemy":
                ColorUtility.TryParseHtmlString("#800000", out _color); // Red
                break;
            case "PowerUp":
                ColorUtility.TryParseHtmlString("#a57c00", out _color); // Gold
                break;
            case "Boss":
                ColorUtility.TryParseHtmlString("#b32134", out _color); //
                break;
            case "Asteroid":
                ColorUtility.TryParseHtmlString("#b2b2a2", out _color); // Grey
                break;
        }

        NewColor(_color);
    }

    void Update()
    { /*
        // Two lines below are code are identical so will not save memory
        // Each will create a duplicate material and apply the new color to 
        // the duplicated material, thus doubling materials & memory.
        //_renderer.material.color = GetRandomColor();
        //_renderer.material.SetColor("_Color", GetRandomColor());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ColorSwitch();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(SwitchColor());
        }
        */
    }

    private Color GetRandomColor()
    {
        return new Color(
            r: Random.Range(0f, 1f),
            g: Random.Range(0f, 1f),
            b: Random.Range(0f, 1f));
    }

    IEnumerator SwitchColor()
    {
        var SwapColors = Random.Range(4, 21);
        for (int i = 0; i < SwapColors; i++)
        {
            ColorSwitch();
            yield return new WaitForSeconds(.5f);
        }

        //OriginalColor();
    }

    private void ColorSwitch()
    {
        // Get the current value of the material properties in the renderer.
        _renderer.GetPropertyBlock(_propBlock);
        // Assign the new color value
        _propBlock.SetColor(name: "_Color", value: GetRandomColor());
        // Apply the edited values to the renderer.
        _renderer.SetPropertyBlock(_propBlock);
    }


    private void NewColor(Color color)
    {
        // Get the current value of the material properties in the renderer.
        _renderer.GetPropertyBlock(_propBlock);
        // Assign the new color value
        _propBlock.SetColor(name: "_Color", value: color);
        // Apply the edited values to the renderer.
        _renderer.SetPropertyBlock(_propBlock);
    }
    /*
    private void OriginalColor()
    {
        // Get the current value of the material properties in the renderer.
        _renderer.GetPropertyBlock(_propBlock);
        // Assign the new color value
        _propBlock.SetColor(name: "_Color", value: _originalColor);
        // Apply the edited values to the renderer.
        _renderer.SetPropertyBlock(_propBlock);
    }
    */
}