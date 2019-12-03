using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelRestart : MonoBehaviour
{
    [SerializeField] bool Switch = true;

    float TimeToRestart = 0;
    float EndTime = 15;

     void Update ()
    {
		if (Switch == true)
        {
            // temporarily ->
            if (TimeToRestart < EndTime) TimeToRestart += Time.deltaTime;
            else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // <| (test time)
        }
    }
}
