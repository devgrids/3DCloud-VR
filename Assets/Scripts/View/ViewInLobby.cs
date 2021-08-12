using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRUiKits.Utils;

public class ViewInLobby : MonoBehaviour
{
    private OptionsManager options;
    [SerializeField] private Image imageScene; 

    void Start()
    {
        options = gameObject.GetComponent<OptionsManager>();
        imageScene.sprite = LobbyManager.sharedInstance.imageList[LobbyManager.indexImagen];
    }

    void Update()
    {
        
    }

    public void SetImageScene()
    {
        LobbyManager.indexImagen = options.GetOption();
        Debug.Log(LobbyManager.indexImagen);
        imageScene.sprite = LobbyManager.sharedInstance.imageList[LobbyManager.indexImagen];
    }
}
