using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullRod : InteractableItem
{
    private Animator animator;
    [SerializeField] private List<InteractableItem> interactableItems = new List<InteractableItem>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Interact()
    {
        animator.SetBool("Open", true);
        foreach (var item in interactableItems)
        {
            if (item.curState == ItemStateType.Close) item.curState = ItemStateType.Interact;
        }
        curState = ItemStateType.Close;
    }
}
