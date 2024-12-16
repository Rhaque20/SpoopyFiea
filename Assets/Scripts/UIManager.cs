using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _collectablesText;
    [SerializeField]
    private PlayerCollection _playerCollection;
    [SerializeField]
    private GameObject _gameOverScreen,_victoryScreen;
    [SerializeField]
    private Image _staminaBar;
    [SerializeField]
    private Image _distractionCounter;
    [SerializeField]
    private Sprite _emptyHandSprite, _distractionSprite;
    private static Image _cursor;

    [SerializeField] private Color _staminaColor, _exhaustedColor;

    public static Action gameOver, victory;
    public static Action<float,bool> sprintStatus;
    public static Action<int, int> updateCollectable;
    public static Action<bool> hoverInteractable;
    public static Action<GameObject> updateDistractible;

    //private static UIManager _instance = null;

    //public static UIManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = FindObjectOfType<UIManager>();
    //            DontDestroyOnLoad(_instance);
    //        }
    //        else
    //        {
    //            Destroy(_instance);
    //            _instance = Instantiate(_UIManagerPrefabStatic).GetComponent<UIManager>();
    //            DontDestroyOnLoad(_instance);
    //        }

    //        return _instance;
    //    }
    //}

    void GatherReferences()
    {
        _cursor = transform.GetChild(0).GetComponent<Image>();
        _collectablesText = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        _staminaBar = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        _gameOverScreen = transform.GetChild(3).gameObject;
        _victoryScreen = transform.GetChild(4).gameObject;
    }

    public static void HoverInteractable(bool onInteractable)
    {
        if(onInteractable)
        {
            _cursor.color = Color.green;
        }
        else
        {
            _cursor.color = Color.white;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GatherReferences();
        _gameOverScreen.SetActive(false);
        _victoryScreen.SetActive(false);
        Debug.Log("Setting gameover screen to false");
    }

    private void AddCollectable()
    {
        _collectablesText.text = "Collectables : " + _playerCollection.CollectableCount;
    }

    public void UpdateCollectables(int numCollect, int maxCollectable)
    {
        _collectablesText.SetText(numCollect + " / " + maxCollectable);
    }

    public void UpdateStaminaGauge(float staminaRatio, bool exhausted)
    {
        Debug.Log("Updating stamina with "+staminaRatio);
        _staminaBar.fillAmount = staminaRatio;

        if(exhausted)
        {
            _staminaBar.color = _exhaustedColor;
        }
        else
            _staminaBar.color= _staminaColor;
    }

    private void OnEnable()
    {
        

        /*_gameOverScreen = GameObject.Find("GameOver");*/
    }
    // Start is called before the first frame update
    void Start()
    {
        /*Collectable.OnCollected += AddCollectable;*/
        gameOver += GameOverScreen;
        sprintStatus += UpdateStaminaGauge;
        updateCollectable += UpdateCollectables;
        victory += Victory;
        hoverInteractable += HoverInteractable;
        SceneManager.sceneLoaded += OnSceneLoaded;
        updateDistractible += UpdateDistractible;
        GatherReferences() ;
        UpdateCollectables(GameManager.maxCollectables - GameManager.requiredNumCollectable, GameManager.maxCollectables);
        /*_gameOverScreen.SetActive(false);*/
    }

    void UpdateDistractible(GameObject itemGrabbed)
    {
        if(itemGrabbed == null)
        {
            _distractionCounter.sprite = _emptyHandSprite;
        }
        else if(itemGrabbed.CompareTag("Distraction"))
        {
            _distractionCounter.sprite = _distractionSprite;
        }
    }

    void GameOverScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        _gameOverScreen.SetActive(true);
    }

    void Victory()
    {
        Cursor.lockState = CursorLockMode.None;
        _victoryScreen.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        /*Collectable.OnCollected-=AddCollectable;*/
        gameOver -= GameOverScreen;
        sprintStatus -= UpdateStaminaGauge;
        victory -= Victory;
        updateCollectable -= UpdateCollectables;
    }

    //MOVE THIS ELSEWHERE
    public void RestartLevel()
    {
        _gameOverScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
