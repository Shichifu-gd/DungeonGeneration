using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelRestart : MonoBehaviour
{
    [SerializeField]
    private bool Switch = true;
    private bool pauseLevelRestart;

    private float TimeToRestart = 0;
    private float endTime;

    private void Update()
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