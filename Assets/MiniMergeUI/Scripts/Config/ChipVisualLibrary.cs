using MiniMergeUI.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniMergeUI/Chip Visual Library")]
public class ChipVisualLibrary : ScriptableObject
{
    [Serializable]
    public struct Entry
    {
        public ChipType Type;
        public Sprite Sprite;
    }

    [SerializeField] private Entry[] _entries;

    public Sprite GetSprite(ChipType type)
    {
        for (int i = 0; i < _entries.Length; i++)
            if (_entries[i].Type == type)
                return _entries[i].Sprite;

        return null;
    }

    public ChipType GetRandomType()
    {
        if (_entries == null || _entries.Length == 0)
            return default;

        return _entries[UnityEngine.Random.Range(0, _entries.Length)].Type;
    }
}
