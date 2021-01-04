using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    private void OnEnable() => PlayerController.OnPlayAgain += RestartLevel;

    private void OnDisable() => PlayerController.OnPlayAgain -= RestartLevel;

    private void RestartLevel() => StartCoroutine(LoadScene(3));

    private IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(0);
    }
}
