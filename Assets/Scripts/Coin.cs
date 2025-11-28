using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private float despawnTime;
    
    public float initialUpVelocity = 1.2f;   // how strongly it pops up
    public float gravity = 10f;           // how fast it falls
    public float maxHeightOffset = 0.15f;  // how high in world units the sprite moves

    private float height = 0f;            // simulated Z height
    private float verticalVelocity;
    
    public Transform sprite;  
    
    private SpriteRenderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        StartCoroutine(Despawn(despawnTime));
        
        verticalVelocity = initialUpVelocity;
    }
    
    void Update()
    {
        // height physics
        verticalVelocity -= gravity * Time.deltaTime;
        height += verticalVelocity * Time.deltaTime;

        // stop on ground
        if (height < 0)
        {
            height = 0;
            verticalVelocity = 0;
        }

        // move the sprite upward based on height
        sprite.localPosition = new Vector3(0, height * maxHeightOffset, 0);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerStats>().ModifyCoins(value);
            Destroy(gameObject);
            // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            // GetComponentInChildren<BoxCollider2D>().enabled = false;
            //StartCoroutine(PlayAnimThenDestroy());
        }
    }

    // IEnumerator PlayAnimThenDestroy()
    // {
    //     //animator.SetBool("Destroyed", true);
    //     yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    //     Destroy(gameObject);
    // }



    IEnumerator Despawn(float lifetime)
    {
        float blinkDuration = 1.0f;
        float delayDecrease = 0.08f;
        int flashes = 10;
        
        yield return new WaitForSeconds(lifetime - blinkDuration);

        float timer = blinkDuration / flashes;

        for (int i = 0; i < flashes; i++)
        {
            float alpha = (i % 2 == 0) ? 1f : 0f;
            Color c = _renderer.color;
            c.a = alpha;
            _renderer.color = c;
            yield return new WaitForSeconds(timer);
            timer = Mathf.Max(0.05f, timer - delayDecrease);
        }
        Destroy(gameObject);
    }
}