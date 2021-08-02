using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected List<WispForm> usableByList;
    private HashSet<WispForm> usableBy = new();

    [SerializeField] private InteractableType type;

    private void Awake()
    {
        foreach(var form in usableByList)
        {
            usableBy.Add(form);
        }
    }

    public bool IsUsableBy(WispForm form)
    {
        return usableBy.Contains(form);
    }

    public InteractableType Type
    {
        get => type;
    }
}

public enum InteractableType
{
    Powerline
}
