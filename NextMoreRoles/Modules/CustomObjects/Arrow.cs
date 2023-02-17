using UnityEngine;

namespace NextMoreRoles.Modules.CustomObjects;

public class Arrow
{
    public static Sprite GetSprite() => ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Objects.Arrow.png", 200f);

    private Vector3 targetPos;
    private SpriteRenderer sprite;
    private GameObject arrow;
    private ArrowBehaviour arrowBehaviour;
    public Arrow(Color color)
    {
        this.arrow = new GameObject("Arrow");
        this.arrow.layer = 5;
        this.sprite = this.arrow.AddComponent<SpriteRenderer>();
        this.sprite.color = color;
        this.arrowBehaviour = this.arrow.AddComponent<ArrowBehaviour>();
        this.arrowBehaviour.image = this.sprite;
    }

    public void SetSprite(SpriteRenderer sprite) => this.arrowBehaviour.image = sprite;
    public void SetColor(Color color) => this.sprite.color = color;
    public void SetTarget(Vector3 pos) => this.targetPos = pos;
    public void SetActive(bool active) {
        if (this.arrow == null) return;
        this.arrow.SetActive(active);
    }
    public void Destroy() => Object.Destroy(this.arrow);

    //unity側で勝手に実行
    public void Update()
    {
        UpdateArrow();
    }

    public void UpdateArrow(Vector3? targetPos = null)
    {
        if (this.arrow == null) return;
        if (targetPos.HasValue) this.targetPos = targetPos.Value;
        this.arrowBehaviour.target = this.targetPos;
        this.arrowBehaviour.Update();
    }
}
