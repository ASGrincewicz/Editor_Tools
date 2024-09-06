namespace Editor.CardData
{
    public enum CardTypes{TBD, Action, Boss,
        [System.ComponentModel.Description("Character: Ally")]
        Character_Ally,
        [System.ComponentModel.Description("Character: Hunter")]
        Character_Hunter,
        Creature,
        Environment,
        [System.ComponentModel.Description("Gear: Equipment")]
        Gear_Equipment,
        [System.ComponentModel.Description("Gear: Upgrade")]
        Gear_Upgrade,
        Starship
    };
}