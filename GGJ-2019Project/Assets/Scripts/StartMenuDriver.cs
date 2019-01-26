using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuDriver : MonoBehaviour
{

    public enum nextScene
    {
        NONE,
        CREDITS,
        GAME,
        SCENEJUMPA,
        END
    };

    private nextScene chosenScene;

    public float startAnimationDelay = .5f;
    private float startDelay = 0;
    public float nextSceneDelay = .5f;
    private float nextDelay = 0;

    public int gameStartSceneIndex = 1;
    public int creditsSceneIndex = 1;

    public GameObject showHideThing;
    public GameObject mainThing;

    public int sceneJumpAIndex = 2;

    public List<GameObject> TutorialSlideshow;
    private int currentTutorialSlide = 0;

    private Animator _animations;

	// Use this for initialization
	void Start ()
	{
	    _animations = this.GetComponent<Animator>();
	    chosenScene = nextScene.NONE;
	    currentTutorialSlide = -1;
	}
	
	// Update is called once per frame
	void Update () {
	    if (startDelay > startAnimationDelay)
	    {
	        setTriggerForAnimator("Start");
	        startDelay = 0;
	    }
	    else
	    {
	        startDelay += Time.deltaTime;
	    }

	    if (chosenScene != nextScene.NONE)
	    {
	        if (nextDelay > nextSceneDelay)
	        {
	            if (chosenScene == nextScene.GAME)
	            {
	                SceneManager.LoadScene(gameStartSceneIndex);
	            }
                else if (chosenScene == nextScene.CREDITS)
	            {
	                SceneManager.LoadScene(creditsSceneIndex);
	            }
                else if (chosenScene == nextScene.END)
	            {
	                Application.Quit();
	            }
                else if (chosenScene == nextScene.SCENEJUMPA)
	            {
	                SceneManager.LoadScene(sceneJumpAIndex);
	            }
	            nextSceneDelay = 0;
	        }
	        else
	        {
	            nextDelay += Time.deltaTime;
	        }
	    }
	}

    public void startGame()
    {
        chosenScene = nextScene.GAME;
        setTriggerForAnimator("Used");
        Debug.Log("loading new game...");

    }

    public void quitGame()
    {
        chosenScene = nextScene.END;
        setTriggerForAnimator("Used");
        Debug.Log("quitting...");

    }

    public void credits()
    {
        chosenScene = nextScene.CREDITS;
        setTriggerForAnimator("Used");
        Debug.Log("loading credits...");
    }

    public void sceneJumpA()
    {
        chosenScene = nextScene.SCENEJUMPA;
        setTriggerForAnimator("Used");
        Debug.Log("loading credits...");
    }

    private void setTriggerForAnimator(string name)
    {
        if (_animations != null)
        {
            _animations.SetTrigger(name);
        }
    }

    public void showThing()
    {
        showHideThing.SetActive(true);
        mainThing.SetActive(false);
    }

    public void hideThing()
    {
        showHideThing.SetActive(false);
        mainThing.SetActive(true);
    }

    public void nextSlide()
    {
        if (currentTutorialSlide >= 0 && currentTutorialSlide <= TutorialSlideshow.Count - 1)
        {
            TutorialSlideshow[currentTutorialSlide].SetActive(false);
        }
        currentTutorialSlide++;

        if (currentTutorialSlide >= TutorialSlideshow.Count)
        {
            currentTutorialSlide = -1;
        }
        else
        {
            TutorialSlideshow[currentTutorialSlide].SetActive(true);
        }
        

    }
}
