using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

//A Modelclass for the Menu. So much code!
public class MenuModel
{
    public ReactiveProperty<bool> m_bIsPaused = new ReactiveProperty<bool>(false); //Is the Game Paused right now?
}

//This script controls the PauseMenu
public class MenuController : MonoBehaviourTrans
{
    public static MenuController s_instance; //Singleton instance

    public MenuModel m_menumodel = new MenuModel();

    private void Awake()
    {
        //this class is a singleton, so if there is another one already, destroy this one
        if (s_instance != null)
        {
            Debug.Log("Singleton class: " + this.GetType().ToString() + " had another instance on: " + gameObject.ToString() + " which was destroyed.");
            Destroy(this);
            return;
        }
        s_instance = this;   //init singleton instance
    }

	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }

    void OpenPauseMenu()
    {
        m_menumodel.m_bIsPaused.Value = !m_menumodel.m_bIsPaused.Value;
        Time.timeScale = m_menumodel.m_bIsPaused.Value ? 0.0f : 1.0f;
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator OpenPauseAfterDelay(float _fDelay)
    {
        yield return new WaitForSeconds(_fDelay);
        OpenPauseMenu();
    }
}
