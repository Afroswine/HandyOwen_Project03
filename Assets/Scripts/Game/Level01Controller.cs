using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class Level01Controller : MonoBehaviour
{
    public UnityEvent ToggleMenu;

    [SerializeField] AudioClip _startingSong;
    [SerializeField] Text _currentScoreTextView;

    int _currentScore;

    private void Start()
    {
        if(_startingSong != null)
        {
            AudioManager.Instance.PlaySong(_startingSong);
        }

        _currentScore = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        // Exit Level
        //TODO bring up popup menu for navigation
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu.Invoke();
        }
    }

    public void IncreaseScore(int scoreIncrease)
    {
        // increase score
        _currentScore += scoreIncrease;
        // update score display, so we can see the new score
        _currentScoreTextView.text =
            "Score: " + _currentScore.ToString();
    }

}
