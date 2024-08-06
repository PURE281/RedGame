using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_Data", menuName = "Character/Player", order = 1)]
[SerializeField]
public class SOCharacterData : ScriptableObject
{
    public int Id;
    public string Name;
    public Sprite Sprite;
    public int Level;
    public float Exp;
    public float MaxHp;
    public float CurHp;
    public float MaxTp;
    public float CurTp;
    public float OriPA;
    public float CurPA;
    public float OriPD;
    public float CurPD;
    public float OriMA;
    public float CurMA;
    public float OriMD;
    public float CurMD;
    public List<SkillInfo> skills;
}
[Serializable]
public class SkillInfo
{
    public string Name;
    public string Desc;
    public int Cost;
    public int Count;
    public float Value;
    public SkillType skillType;
}