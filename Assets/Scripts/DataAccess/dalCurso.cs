using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class dalCurso : MonoBehaviour
{
    public static dalCurso sharedInstance;

    public Curso curso;
    public List<Curso> listaDeCursos;

    #region UNITY Methods

    private void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    #endregion

    #region UI Callback Methods

    public void obtenerListaDeCursos()
    {
        StartCoroutine(IE_obtenerListaDeCursos());
    }

    public void obtenerListaDeCursos(int idEstudiante)
    {
        StartCoroutine(IE_obtenerListaDeCursos(idEstudiante));
    }

    #endregion

    #region IEnumerator Callback Methods



    public IEnumerator IE_obtenerListaDeCursos()
    {
        UnityWebRequest www = UnityWebRequest.Get(Util.BaseUrl + "/3dcloud/controllers/curso/obtenerListaCursos.php");
        yield return www.SendWebRequest();

        String res = Util.DebugNetwork(www);

        if (res != Util.Error)
        {
            this.listaDeCursos = Util.GetJsonList<Curso>(res);
            foreach(var curso in listaDeCursos)
            {
                LobbyManager.sharedInstance.AddContenedorListaCurso(curso.nombre, curso.capacidad, curso.imagen);
            }    
        }
    }

    public IEnumerator IE_obtenerListaDeCursos(int idEstudiante)
    {
        WWWForm form = new WWWForm();
        form.AddField("idEstudiante", idEstudiante);

        UnityWebRequest www = UnityWebRequest.Post(Util.BaseUrl + "/3dcloud/controllers/curso/obtenerListaDeCursosPorIdEstudiante.php", form);
        yield return www.SendWebRequest();

        String res = Util.DebugNetwork(www);

        if (res != Util.Error)
        {
            this.listaDeCursos = Util.GetJsonList<Curso>(res);
            GameManager.sharedInstance.SetGameState(GameState.lobbyEstudiante);
            foreach (var curso in listaDeCursos)
            {
                LobbyManager.sharedInstance.AddContenedorListaCurso(curso.nombre, curso.capacidad, curso.imagen);
            }
            LobbyEstudianteManager.sharedInstance.SetAttributesGUI();
        }
    }

    #endregion
}
