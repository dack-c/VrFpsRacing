using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update

    public void OnClickGameStart()
    {
        SceneManager.LoadScene("terrain tset Scene_M2");
    }
}
