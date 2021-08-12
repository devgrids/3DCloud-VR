using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class dalCuenta : MonoBehaviour
{
    public static dalCuenta sharedInstance;
    public Cuenta cuenta;
    
    public TMP_InputField inputUser;
    public TMP_InputField inputPassword;

    public static bool isDocente;

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

    public void guardarCuenta()
    {
        Cuenta obj = new Cuenta();
        obj.nickname = "JoseMV";
        obj.nombres = "Piero Jose";
        obj.password = "cloud";
        obj.telefono = "987456123";
        obj.tipoCuenta = 0;
        obj.apellidos = "Moreno Vásquez";
        obj.domicilio = "Barcelona";
        obj.email = "piero.jmv.2001@gmail.com";

        string json = JsonUtility.ToJson(obj);
        StartCoroutine(IE_guardarCuenta(json));
    }

    public void obtenerCuenta()
    {
        StartCoroutine(IE_obtenerCuenta(inputUser.text, inputPassword.text));
    }

    #endregion

    #region IEnumerator Callback Methods

    private IEnumerator IE_guardarCuenta(string jsonCuenta)
    {
        WWWForm form = new WWWForm();
        form.AddField("cuenta", jsonCuenta);

        UnityWebRequest www = UnityWebRequest.Post(Util.BaseUrl + "/3dcloud/controllers/cuenta/guardarCuenta.php", form);
        yield return www.SendWebRequest();

        string res = Util.DebugNetwork(www);
        if (res != Util.Error)
        {
            Debug.Log(res);
        }
    }

    private IEnumerator IE_obtenerCuenta(string user, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", user);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(Util.BaseUrl + "/3dcloud/controllers/cuenta/obtenerCuentaPorEmailAndPassword.php", form);
        yield return www.SendWebRequest();

        string res = Util.DebugNetwork(www);

        if (res != Util.Error)
        {
            this.cuenta = JsonUtility.FromJson<Cuenta>(res);
            if (this.cuenta.idCuenta > 0)
            {
                if (this.cuenta.tipoCuenta == 0)
                {
                    GameManager.sharedInstance.SetGameState(GameState.lobbyDocente);
                }
                else if (this.cuenta.tipoCuenta == 1)
                {
                    dalEstudiante.sharedInstance.obtenerEstudiante(this.cuenta.idCuenta);
                    
                }
                isDocente = this.cuenta.tipoCuenta == 0;
            }
        }
    }

    #endregion

   
}
