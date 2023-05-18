
using UnityEngine;
using UnityEngine.UI;

public class PreferredAspectLayoutElement : MonoBehaviour, ILayoutElement
{

    private RectTransform Parent => transform.parent.GetComponent<RectTransform>();

    public float minWidth => 1;
    public float minHeight => 1;
    public float preferredWidth => axis == Axis.Vertical ? Parent.rect.width : Parent.rect.height / aspectRatio.y * aspectRatio.x;
    public float preferredHeight => axis == Axis.Horizontal ? Parent.rect.height : Parent.rect.width / aspectRatio.x * aspectRatio.y;
    [SerializeField] public int layoutPriority { get; private set; }
    public float flexibleWidth => axis == Axis.Vertical ? 1 : 0;
    public float flexibleHeight => axis == Axis.Horizontal ? 1 : 0;

    [SerializeField] private Axis axis;
    [SerializeField] private Vector2 aspectRatio;

    public enum Axis
    {
        Horizontal, Vertical
    }

    public void CalculateLayoutInputHorizontal()
    {

    }

    public void CalculateLayoutInputVertical()
    {

    }
}
