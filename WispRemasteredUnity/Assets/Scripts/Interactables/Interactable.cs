using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected List<WispFormType> usableByList;
    private HashSet<WispFormType> usableBy = new HashSet<WispFormType>();

    [SerializeField] private InteractableType type;
    [SerializeField] private int scoreValue;

    public int ScoreValue
    {
        get => scoreValue;
    }

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

    public abstract void DoInteraction(PlayerController player);

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true; //Ensures collider is set to trigger when added
    }
}

public enum InteractableType
{
    SimpleLandable,
    Powerline,
    Firework
}
