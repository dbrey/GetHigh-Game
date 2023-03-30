using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTextMesh : MonoBehaviour
{
    TMPro.TextMeshProUGUI mText;
    [SerializeField]TMPro.TextMeshProUGUI asignaturaText;
    Result result;
    [SerializeField] TextBlockAsset profesText;

    public Result GetResult => result; 

    // Start is called before the first frame update
    void Start()
    {
        result = FindObjectOfType<Result>();

        mText = this.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        Debug.Log("Time :" + result.TotalTime);

        int mins = (int)result.TotalTime / 60;
        int secs = (int)result.TotalTime % 60;

        // El / 6 es para que siempre se trunque al múltiplo de 6 más por debajo (porque los créditos se consiguen de 6 en 6)
        int asignaturas = Mathf.Max((((int)result.TotalTime - 20) / 6), 0); 

        mText.text = ("Tus apuntes han durado " + mins + ":" + secs.ToString().PadLeft(2, '0') + "\nTus creditos son " + asignaturas * 6);

        if(result.modoUCM)
            asignaturaText.text = profesText.GetAsignatura(asignaturas);
    }

    private void OnDestroy()
    {
        Destroy(result.gameObject);
    }
}
