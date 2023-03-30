using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    float time;
    public bool modoUCM = true;

   public int creditos => Mathf.Max((((int)TotalTime - 20) / 6), 0);

    public float TotalTime { get => time; set => time = value; }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
