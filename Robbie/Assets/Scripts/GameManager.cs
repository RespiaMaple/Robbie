using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    SceneFader fader;
    List<Orb> orbs;
    Door lockedDoor;

    float gameTime;
    bool gameIsOver;
    
    public int deathNum;

    private void Awake() 
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        orbs = new List<Orb>();

        DontDestroyOnLoad(gameObject);    
    }

    private void Update() 
    {
        if(gameIsOver)
            return;
        
        gameTime += Time.deltaTime;
        UIManager.UpdateTimeUI(gameTime);
    }

    public static void ReigsterDoor(Door door)
    {
        instance.lockedDoor = door;
    }

    public static void RegisterSceneFader(SceneFader obj)
    {
        instance.fader = obj;
    }

    public static void RegisterOrb(Orb orb)
    {
        if(instance == null)
            return;
        if(!instance.orbs.Contains(orb))
            instance.orbs.Add(orb);
        UIManager.UpdateOrbUI(instance.orbs.Count);

    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if(!instance.orbs.Contains(orb))
            return;
        instance.orbs.Remove(orb);

        if (instance.orbs.Count == 0)
            instance.lockedDoor.Open();
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }

    public static void PlayerWon()
    {
        instance.gameIsOver = true;

        UIManager.DisplayGameOver();

        AudioManger.PlayerWonAudio();

    }

    public static bool GameOver()
    {
        return instance.gameIsOver;
    }

    public static void PlayerDied()
    {
        if (instance == null)
			return;
        
        instance.fader.FadeOut();//Bug待修正，死亡後會先loadscene再fadeout

        instance.deathNum++;

        UIManager.UpdateDeathUI(instance.deathNum);

        if(instance.fader != null)
			instance.fader.FadeOut();
        
        instance.Invoke("RestartScene", 1.5f);
    }

    void RestartScene()
    {
        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
