using System.Collections.Generic;
using UnityEngine;

public class PartInventory : MonoBehaviour
{
    public static PartInventory I;

    [Header("需要的种类数")]
    public int required = 3;

    // 拥有哪些“种类”（用于HUD显示 0/3、1/3…）
    private readonly HashSet<PartType> bag = new HashSet<PartType>();
    // 每种的“数量”（用于真正消耗）
    private readonly Dictionary<PartType, int> counts = new Dictionary<PartType, int>();

    void Awake() => I = this;

    /// <summary>拾取一个碎片</summary>
    public void Add(PartType t)
    {
        // 数量 +1
        counts.TryGetValue(t, out int c);
        counts[t] = c + 1;

        // 第一次拥有该类型时，集合里加入并刷新HUD
        bool firstTime = bag.Add(t);
        if (firstTime)
            HUD.I?.SetParts(bag.Count, required);
    }

    /// <summary>尝试消耗一个指定类型（成功返回 true）</summary>
    public bool TryUse(PartType type)
    {
        if (!counts.TryGetValue(type, out int c) || c <= 0)
            return false;

        c -= 1;
        if (c <= 0)
        {
            counts.Remove(type);
            // 没库存了，就把这个“种类”从拥有集合里移除，并刷新HUD（这样HUD显示的是“当前拥有的种类数”）
            if (bag.Remove(type))
                HUD.I?.SetParts(bag.Count, required);
        }
        else
        {
            counts[type] = c;
        }
        return true;
    }

    /// <summary>当前是否至少拥有该类型 1 个</summary>
    public bool Has(PartType t) => counts.TryGetValue(t, out int c) && c > 0;

    /// <summary>查询剩余数量</summary>
    public int GetCount(PartType t) => counts.TryGetValue(t, out int c) ? c : 0;
}