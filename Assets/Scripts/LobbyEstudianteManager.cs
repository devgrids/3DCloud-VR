using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyEstudianteManager : MonoBehaviour
{
    public static LobbyEstudianteManager sharedInstance;

    public TMP_Text textNombres;
    public TMP_Text textCantidadCursos;
    public TMP_Text textMonedas;
    public TMP_Text textGemas;
    public TMP_Text textItems;

    private Cuenta cuenta;
    private Estudiante estudiante;

    private void Awake()
    {
        sharedInstance = this;
        
    }

    public void SetAttributesGUI()
    {
        this.cuenta = dalCuenta.sharedInstance.cuenta;
        this.estudiante = dalEstudiante.sharedInstance.estudiante;

        SetNombresGUI();
        SetCantidadCursosGUI();
        SetMonedasGUI();
        SetGemasGUI();
        SetItemsGUI();
    }

    public void SetNombresGUI()
    {
        textNombres.text = "Nombres: " + cuenta.nombres + " " + cuenta.apellidos;
    }

    public void SetCantidadCursosGUI()
    {
        textCantidadCursos.text = "Cursos: " + dalCurso.sharedInstance.listaDeCursos.Count.ToString();
    }

    public void SetMonedasGUI()
    {
        textMonedas.text = "Monedas: " + estudiante.monedas.ToString();
    }

    public void SetGemasGUI()
    {
        textGemas.text = "Gemas: " + estudiante.gemas.ToString();
    }

    public void SetItemsGUI()
    {
        //textItems.text = "Items: " + estudiante.items.ToString();
    }

}
