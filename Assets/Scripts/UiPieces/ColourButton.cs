using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourButton : MonoBehaviour
{
    public Color colour;
    [SerializeField]
    private GameObject checkMark;
    public bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        var buttonImage = GetComponent<Image>();
        buttonImage.color = colour;
    }

    public void OnClick()
    {
        var colourButtons = FindObjectsOfType<ColourButton>();
        foreach (var button in colourButtons)
            button.Deselect();

        isSelected = true;
        checkMark.SetActive(isSelected);
    }

    public void Deselect()
    {
        isSelected = false;
        checkMark.SetActive(isSelected);
    }
}
