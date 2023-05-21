public struct DamageInfo
{
    public int amount;
    public Element element;
    public bool bypassArmor;

    public DamageInfo(int amount, Element element = Element.PHYSICAL, bool bypassArmor = false)
    {
        this.amount = amount;
        this.element = element;
        this.bypassArmor = bypassArmor;
    }

    public DamageInfo WithDamageAmount(int amount) => new(amount, element, bypassArmor);
    public DamageInfo WithElement(Element element) => new(amount, element, bypassArmor);
    public DamageInfo WithBypassArmor(bool bypassArmor) => new(amount, element, bypassArmor);

}