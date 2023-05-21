public static class TileModifiers
{
    public static ITileModifier ICE => new Ice();
    public static ITileModifier WEB => new Web();
    public static ITileModifier CALTROPS => new Caltrops();
    public static ITileModifier GREASE => new Grease();
    public static ITileModifier FIRE => new Fire();
    public static ITileModifier SMOKE => new Smoke();
}