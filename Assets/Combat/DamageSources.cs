public static class DamageSources
{
    public static DamageInfo BLEED => new(2, Element.PHYSICAL, true);
    public static DamageInfo PHYSICAL => new(3, Element.PHYSICAL, false);
    public static DamageInfo FIRE => new(3, Element.FIRE, false);
    public static DamageInfo ICE => new(3, Element.ICE, false);
    public static DamageInfo AIR => new(3, Element.AIR, false);
    public static DamageInfo EARTH => new(3, Element.EARTH, false);
    public static DamageInfo FUNGAL => new(3, Element.FUNGAL, false);
    public static DamageInfo NECROTIC => new(3, Element.NECROTIC, true);
}