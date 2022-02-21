using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public IEnumerator Scene(string SceneName, float Delay)
    {
        yield return new WaitForSeconds(Delay);
        SceneManager.LoadScene(SceneName);
    }
}
