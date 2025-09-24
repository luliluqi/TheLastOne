using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : BaseSingleton<GameManager>
{
    int currentGameRunTime;
    public int CurrentGameRunTime { get { return currentGameRunTime; } }
    public UnityAction<int> GameTimeUpdateEvent;
    public UnityAction<bool> PauseEvent;
    public UnityAction GameOverEvent;
    public UnityAction GameRestartEvent;
    bool isPause;

    public void Pause(bool isPause)
    {
        PauseEvent?.Invoke(isPause);
        this.isPause = isPause;
    }

    private void Start()
    {
        NewGame();
        LevelSystem._Instance.GetComponent<PlayerDataController>().PlayerDead += GameOver;
    }

    void NewGame()
    {
        StartCoroutine(Tick());
    }

    void GameOver()
    {
        GameOverEvent?.Invoke();
        LevelSystem._Instance.GetComponent<PlayerInput>().enabled = false;
        ResourcesLoadManager._Instance.ResourcesLoadObject<PropertiesControllerSO>(ResourcesLoadManager._Instance.ConfigPath + "PropertiesControllerSO", out var PCSO);
        ResourcesLoadManager._Instance.ResourcesLoadObject<NpcManagerSO>(ResourcesLoadManager._Instance.ConfigPath + "NpcManagerSO", out var NCSO);
        PCSO.ResetSO();
        NCSO.ResetSO();
        Pause(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Restart()
    {
        GameRestartEvent?.Invoke();
        currentGameRunTime = 0;
        GameTimeUpdateEvent?.Invoke(currentGameRunTime);
        LevelSystem._Instance.GetComponent<PlayerInput>().enabled = true;
        Pause(false);
    }

    IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!isPause)
            {
                currentGameRunTime += 1;
                GameTimeUpdateEvent?.Invoke(currentGameRunTime);
            }
        }
    }
}
