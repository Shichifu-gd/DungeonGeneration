using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelRestart : MonoBehaviour
{
    [SerializeField]
    bool Switch = true;
    bool pauseLevelRestart;

    float TimeToRestart = 0;
    float endTime;

    void Update()
    {
        if (Switch == true && pauseLevelRestart == false)
        {
            // temporarily ->
            if (TimeToRestart < endTime) TimeToRestart += Time.deltaTime;
            else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // <| (test time)
        }
    }

    #region assignment

    public bool PauseLevelRestart()
    {
        pauseLevelRestart = !pauseLevelRestart;
        return pauseLevelRestart;
    }

    public float EndTime
    {
        set
        {
            if (value > 0) endTime = value;
            else endTime = 10;
        }
    }

    #endregion
}