using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Transform overlay;
    [SerializeField]
    private float xOffset;
    [SerializeField]
    private float transitionDuration;
    [SerializeField]
    private LeanTweenType animationType;

    private float startXPos;
    private bool loadNewScene;

    // Start is called before the first frame update
    void Start()
    {
        startXPos = overlay.position.x;
        loadNewScene = false;

        LeanTween.moveX(overlay.gameObject, startXPos - xOffset, transitionDuration).setEase(animationType);
    }

    public void LoadScene(Scenes scene)
    {
        if (loadNewScene)
            return;

        loadNewScene = true;

        overlay.position = new Vector3(startXPos + xOffset, overlay.position.y, overlay.position.z);

        LeanTween.moveX(overlay.gameObject, startXPos, transitionDuration).setEase(animationType);

        StartCoroutine(GlobalControl.LoadSceneAfter(scene, transitionDuration));
    }
}
