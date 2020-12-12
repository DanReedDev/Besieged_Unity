using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeWriggle : MonoBehaviour
{

    [SerializeField] private Animator myAnimationContoller;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimationContoller.SetBool("passTree", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimationContoller.SetBool("passTree", false);
        }
    }

}
