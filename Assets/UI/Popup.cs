using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    public void ClosePopup() {
        StartCoroutine(IeClosePopup());
    }

    private IEnumerator IeClosePopup() {
        animator.Play("close");
        yield return new WaitForSeconds(0.58f);
        gameObject.SetActive(false);
    }
}
