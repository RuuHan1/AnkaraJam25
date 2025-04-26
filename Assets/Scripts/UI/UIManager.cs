using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera cam;

    [SerializeField]
    private CarController car;

    public void PlayButton()
    {
        cam.Target.TrackingTarget = null;
        StartCoroutine(PlayCoroutine());
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    IEnumerator PlayCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}