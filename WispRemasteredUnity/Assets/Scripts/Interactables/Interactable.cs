using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected List<WispFormType> usableByList;
    private HashSet<WispFormType> usableBy = new HashSet<WispFormType>();

    [SerializeField] private InteractableType type;

    private void Awake()
    {
        foreach(var form in usableByList)
        {
            usableBy.Add(form);
        }
    }

    public bool IsUsableBy(WispFormType form)
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
    Powerline,
    Firework
}
