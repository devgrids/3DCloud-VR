using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class dalEstudiante : MonoBehaviour
{
    public static dalEstudiante sharedInstance;
    public Estudiante estudiante;

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


    public void obtenerEstudiante(int idCuenta)
    {
        StartCoroutine(IE_obtenerEstudiante(idCuenta));
    }

    #endregion


    #region IEnumerator Callback Methods

    public IEnumerator IE_obtenerEstudiante(int idCuenta)
    {
        WWWForm form = new WWWForm();
        form.AddField("idCuenta", idCuenta);

        UnityWebRequest www = UnityWebRequest.Post(Util.BaseUrl + "/3dcloud/controllers/estudiante/obtenerEstudiantePorIdCuenta.php", form);
        yield return www.SendWebRequest();

        string res = Util.DebugNetwork(www);

        if (res != Util.Error)
        {
            this.estudiante = JsonUtility.FromJson<Estudiante>(res);
            dalCurso.sharedInstance.obtenerListaDeCursos(this.estudiante.idEstudiante);
        }
    }

    #endregion
}
