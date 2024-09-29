using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(HideObject());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Ninja") {

        }
    }

    IEnumerator HideObject() {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
