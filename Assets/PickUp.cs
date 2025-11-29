using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private int value;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerStats>().ModifyCoins(value);
            Destroy(transform.parent.gameObject);
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
}
