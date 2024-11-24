using System.Collections;
using System.Collections.Generic;
using Entities.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{

    [SerializeField] private float timePenalty = 2f;
    private AudioSource _killRegie;

    void Awake()
    {
        _killRegie = GetComponent<AudioSource>();
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerMovement player)) return;


        _killRegie.Play();
        Timer.Instance.DecreaseTime(timePenalty);
        player.Respawn();
    }
}
