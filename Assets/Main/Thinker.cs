using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Thinker: MonoBehaviour
{
    [Header("Target")]
    public string Target;
    public string TargetName;
    public string KillTarget;

    [Header("Activation")]
    public float Delay;
    public string Message;

    public Thinker Owner;
    public Thinker Enemy;
    public Thinker Activator;

    private bool inUse;

    public static List<Thinker> entitylist = new List<Thinker>();
    public Action<Thinker, Thinker, Thinker> Use;
    private bool IsDelayed;

    private void OnEnable()
    {
        inUse = true;

        if (!entitylist.Contains(this))
            entitylist.Add(this);
    }
    public void Activate(Thinker other, Thinker activator)
    {
        Use?.Invoke(this, other, activator);
    }
    Thinker Find(Thinker from, string match)
    {
        int index = from == null ? -1 : entitylist.IndexOf(from);
        int startIndex = index + 1;

        for (int i = startIndex; i < entitylist.Count; i++)
        {
            Thinker entity = entitylist[i];

            if (entity == null || !entity.inUse)
                continue;

            if (entity.TargetName == match)
            {
                return entity;
            }
        }

        return null;
    }

    public IEnumerator ThinkDelay(Thinker ent)
    {
        float delay = ent.Delay;

        yield return new WaitForSeconds(delay);

        if (ent == null || !ent.inUse)
            yield break;

        ent.Delay = 0;

        UseTargets(ent, ent.Activator);

        FreeEdict(ent);
    }

    List<Thinker> FindAll(string targetname)
    {
        List<Thinker> results = new List<Thinker>();

        foreach (Thinker entity in entitylist)
        {
            if (entity != null && entity.TargetName == targetname)
                results.Add(entity);
        }

        return results;
    }

    public void UseTargets(Thinker ent, Thinker activator)
    {
        Thinker t;

        if (ent.Delay > 0 && !ent.IsDelayed)
        {
            t = Spawn();

            t.IsDelayed = true;
            t.Delay = ent.Delay;
            t.name = "DelayedUsed";
            t.Activator = activator;
            t.Message = ent.Message;
            t.Target = ent.Target;
            t.KillTarget = ent.KillTarget;

            StartCoroutine(ThinkDelay(t));

            return;
        }

        if (!string.IsNullOrEmpty(ent.Message))
        {
            PickupMessageUI.Instance.ShowMessage(ent.Message, 3.0f);
        }

        if (!string.IsNullOrEmpty(ent.KillTarget))
        {
            List<Thinker> targets = FindAll(ent.KillTarget);

            foreach (Thinker target in targets)
            {
                FreeEdict(target);

                if (!ent.inUse)
                {
                    Debug.Log("Entity was removed while using killtargets");
                    return;
                }
            }
        }
        int iterations = 0;
        int maxIterations = entitylist.Count;

        if (!string.IsNullOrEmpty(ent.Target))
        {
            t = null;
            Debug.Log($"SOURCE: {ent.name} | ID: {ent.GetInstanceID()} | TARGET: {ent.Target}");
            while ((t = Find(t, ent.Target)) != null )
            {
                if (++iterations > maxIterations)
                {
                    Debug.LogError($"UseTargets: infinite loop detected! Target: {ent.Target}");
                    return;
                }

                if (t == ent)
                {
                    Debug.Log("WARNING: Entity used itself.\n");
                }

                else
                {
                    t.Use?.Invoke(t, ent, activator);
                }

                if (!ent.inUse)
                {
                    Debug.Log("entity was removed while using targets\n");
                    return;
                }
            }

        }
    }

    public Thinker Spawn()
    {
        GameObject obj = new GameObject("Edict");
        Thinker entity = obj.AddComponent<Thinker>();
        InitEdict(entity);

        entitylist.Add(entity);

        return entity;
    }

    public void FreeEdict(Thinker entity)
    {
        if (entity == null || !entity.inUse)
            return;

        entity.inUse = false;
        entity.gameObject.SetActive(false);
    }

    void InitEdict(Thinker e)
    {
        e.inUse = true;
    }
}
