using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthSliderInterface : MonoBehaviour
{
    [SerializeField] EnemyInterface _enemy;
    private Slider _slider;

    private void OnEnable()
    {
        _enemy.TookDamage.AddListener(UpdateDisplay);
        _enemy.Healed.AddListener(UpdateDisplay);
    }

    private void OnDisable()
    {
        _enemy.TookDamage.RemoveListener(UpdateDisplay);
        _enemy.Healed.RemoveListener(UpdateDisplay);
    }

    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _enemy.MaxHealth;
        _slider.value = _enemy.CurrentHealth;
        
    }

    public void UpdateDisplay()
    {
        _slider.value = _enemy.CurrentHealth;
    }

}
