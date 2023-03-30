using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareHighScore : MonoBehaviour
{
    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    private const string TWEET_LANGUAGE = "es";
    public static string descriptionParam;
    private string appStoreLink = "https://rioni.itch.io/get-high";

    public SetTextMesh textMesh;

    public void ShareToTW(string linkParameter)
    {
        Result result = textMesh.GetResult;

        string modo = result.modoUCM ? "- Modo UCM" : "Modo Normal";
        string nameParameter = "He conseguido " + result.creditos * 6 + " creditos en Get High, intenta aprobar si puedes " + modo;//this is limited in text length 
        nameParameter += "\n#ComjamOnFDI2022 #GetHigh";
        Application.OpenURL(TWITTER_ADDRESS +
           "?text=" + WWW.EscapeURL(nameParameter + "\n" + descriptionParam + "\n" + "Obtén el juego:\n" + appStoreLink));
    }
}
