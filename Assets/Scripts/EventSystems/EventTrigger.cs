using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EventTrigger : MonoBehaviour
{

    public enum TriggerType { None, MonsterTrigger, OpenDoor, LightFlickering};
    [SerializeField]private TriggerType type;

    private Light _lightSource;

    [SerializeField] private float _lightFlickerInterval = 0.3f;
    [SerializeField] private Vector2 _intensityRange = Vector2.zero;
    private float _flickerTimer = 0f;
    private float _originalIntensity;


    private void Start()
    {
        _lightSource = GetComponent<Light>();
        if(_lightSource != null)
        {
            _originalIntensity = _lightSource.intensity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            switch(type)
            {
                case TriggerType.MonsterTrigger:
                    GameManager.onMonsterActivate?.Invoke();
                    gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            switch (type)
            {
                case TriggerType.LightFlickering:
                    if (_lightSource != null && _flickerTimer <= 0f)
                    {
                        _flickerTimer = _lightFlickerInterval;
                        _lightSource.intensity = Random.Range(_intensityRange.x, _intensityRange.y);
                    }
                    _flickerTimer -= Time.deltaTime;
                    break;
            }
        }

        Debug.Log("Contact with " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            switch (type)
            {
                case TriggerType.LightFlickering:
                    _flickerTimer = 0;
                    _lightSource.intensity = _originalIntensity;
                    break;
            }
        }
    }
}
