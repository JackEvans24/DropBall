using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenSpawn : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private float spawnIndex;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<Text>().text = spawnIndex.ToString();

        var gameManager = FindObjectOfType<GameManager>();
        button.onClick.AddListener(() => gameManager.SpawnToken(transform.position));
    }
}
