using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOptions : MonoBehaviour
{
    public EditPlayer editPlayer;
    private AudioSource audioSource;

    [Header("Colour Options")]
    [SerializeField]
    private RectTransform colourPanel;
    [SerializeField]
    private GameObject colourButton;
    [SerializeField]
    private ColumnPositioning colourButtonPositioning;

    [Header("Sound Options")]
    public List<SoundOption> BounceSoundOptions;
    [SerializeField]
    private Dropdown soundDropdown;

    [Header("Animation")]
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private float openCloseDuration;
    [SerializeField]
    private LeanTweenType animationType;

    private SoundOption selectedSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        CreateColourButtons();

        SetUpSoundOptions();
        PopulateSoundDropdown();

        LeanTween.scale(mainPanel, Vector3.one, openCloseDuration).setEase(animationType);
    }

    private void CreateColourButtons()
    {
        var i = 0;

        var availableColours = GlobalControl.Instance.playerColours;
        foreach (var colour in availableColours)
        {
            var button = GameObject.Instantiate(colourButton);

            button.transform.SetParent(colourPanel);
            button.transform.localScale = Vector3.one;
            button.transform.localPosition = colourButtonPositioning.CalculatePosition(i);

            var colourScript = button.GetComponent<ColourButton>();
            colourScript.colour = colour;

            if (colour == editPlayer.player.Colour) {
                colourScript.OnClick();
            }

            var buttonScript = button.GetComponent<Button>();
            buttonScript.onClick.AddListener(() => editPlayer.ChangePlayerColour(colour));

            i++;
        }
    }

    private void SetUpSoundOptions()
    {
        BounceSoundOptions = BounceSoundOptions.OrderBy(o => o.DisplayName).ToList();

        var noSoundOption = ScriptableObject.CreateInstance<SoundOption>();
        noSoundOption.DisplayName = "No Sound";
        BounceSoundOptions.Insert(0, noSoundOption);
    }

    private void PopulateSoundDropdown()
    {
        foreach (var option in BounceSoundOptions)
        {
            soundDropdown.options.Add(new Dropdown.OptionData(option.DisplayName));
        }

        selectedSound = editPlayer.player.bounceSound;
        soundDropdown.value = BounceSoundOptions.IndexOf(
            BounceSoundOptions.FirstOrDefault(b => b.DisplayName == selectedSound?.DisplayName)
        );

        soundDropdown.onValueChanged.AddListener(value => selectedSound = BounceSoundOptions[value]);
        soundDropdown.onValueChanged.AddListener(value => audioSource.PlayOneShot(BounceSoundOptions[value].Sound));
    }

    public void OnClose()
    {
        editPlayer.ChangePlayerSound(selectedSound);

        LeanTween.scale(mainPanel, Vector3.zero, openCloseDuration).setEase(animationType).setDestroyOnComplete(true);
    }
}
