using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class MenuController : MonoBehaviourTrans
{
    public static MenuController s_intance;
    public ReactiveProperty<bool> m_bIsPaused = new ReactiveProperty<bool> (false);

    private void Awake()
    {
        s_intance = this;
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

        m_bIsPaused.Value = !m_bIsPaused.Value;
        Time.timeScale = m_bIsPaused.Value ? 0.0f : 1.0f;
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
