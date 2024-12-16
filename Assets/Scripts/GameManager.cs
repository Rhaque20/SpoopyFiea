using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Ongoing, GameOver, Victory}
    private GameObject _monster;

    [SerializeField]private Transform _collectableList;

    public static Action onMonsterActivate;

    // Note to self, never make the two int variables public again
    public static int requiredNumCollectable;

    public static int maxCollectables;

    public static GameState activeState = GameState.Ongoing;


    public static void CollectableObtained(int index)
    {
        requiredNumCollectable--;
        Debug.Log("Obtained item " + index);
        UIManager.updateCollectable?.Invoke(maxCollectables - requiredNumCollectable, maxCollectables);

        if (requiredNumCollectable == 0)
        {
            Debug.Log("Collected everything");
            UIManager.victory?.Invoke();
            activeState = GameState.Victory;
        }
    }

    
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Calling Awake in game manager");
        onMonsterActivate += SpawnMonster;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _monster = GameObject.FindWithTag("AI");
        if (_monster == null)
            Debug.Log("Monster is null");
        _monster.SetActive(false);
        Debug.Log("Scene has loaded");

       GameObject[] interactArray = GameObject.FindGameObjectsWithTag("Interactable");

        foreach(GameObject g in interactArray)
        {
            if(g.name.CompareTo("Collectables") == 0)
            {
                _collectableList = g.transform;
                requiredNumCollectable = _collectableList.childCount;
                break;
            }
        }

        maxCollectables = requiredNumCollectable;

        UIManager.updateCollectable?.Invoke(maxCollectables - requiredNumCollectable, maxCollectables);
    }

    void SpawnMonster()
    {
        _monster.SetActive(true);
    }
}
