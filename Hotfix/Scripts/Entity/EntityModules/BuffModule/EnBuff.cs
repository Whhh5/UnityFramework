public enum EnBuff
{
	None = 0,
	[EditorFieldNameAttribute("禁止移动")]
	NoMovement = 1,
    [EditorFieldNameAttribute("易损")]
    Vulnerable,
    [EditorFieldNameAttribute("燃烧")]
    Burn,
    [EditorFieldNameAttribute("冰霜")]
    Crystal,
    [EditorFieldNameAttribute("破甲")]
    ArmorBreak,
    [EditorFieldNameAttribute("崩甲")]
    ShatteredArmor,
    [EditorFieldNameAttribute("冰冻")]
    Freeze,
    [EditorFieldNameAttribute("感电")]
    Electric,
    [EditorFieldNameAttribute("出血")]
    Bleed,
}
