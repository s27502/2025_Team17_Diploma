using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float despawnTime;
    [SerializeField] private AudioClip collectSound;
    
    public float initialUpVelocity = 1.2f;   
    public float gravity = 10f;          
    public float maxHeightOffset = 0.15f;  

    private float height = 0f;            
    private float verticalVelocity;
    
    private Collider2D col;
    
    public Transform sprite;  
    
    public SpriteRenderer _renderer;

    void Awake()
    {
        
    }
    void Start()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
        StartCoroutine(PickUpCooldown());
        StartCoroutine(Despawn(despawnTime));
        
        verticalVelocity = initialUpVelocity;
    }
    
    void Update()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        height += verticalVelocity * Time.deltaTime;
        
        if (height < 0)
        {
            height = 0;
            verticalVelocity = 0;
        }
        
        sprite.localPosition = new Vector3(0, height * maxHeightOffset, 0);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySfx(collectSound);
            Destroy(gameObject);
        }
    }


    IEnumerator PickUpCooldown()
    {
        yield return new WaitForSeconds(2f);
        col.enabled = true;
    }
    
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