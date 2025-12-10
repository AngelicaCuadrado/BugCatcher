using UnityEngine;

public class EnemyAura : MonoBehaviour
{
    [SerializeField] private Renderer auraRenderer;

    [Header("Aura Colors")]
    public Color idleColor = Color.green;
    public Color patrolColor = Color.cyan;
    public Color chaseColor = Color.yellow;
    public Color attackColor = Color.red;
    public Color walkBackColor = new Color(1f, 0.5f, 0f);
    public Color deadColor = Color.gray;

    [Header("Transition")]
    [SerializeField] private float colorLerpSpeed = 5f; // higher = faster change

    Material auraMat;
    static readonly int AuraColorID = Shader.PropertyToID("_AuraColor");

    Color currentColor;
    Color targetColor;

    private void Awake()
    {
        if (auraRenderer != null)
        {
            auraMat = auraRenderer.material;

            // start from whatever the material has
            if (auraMat.HasProperty(AuraColorID))
                currentColor = targetColor = auraMat.GetColor(AuraColorID);
            else
                currentColor = targetColor = idleColor;
        }
    }

    private void Update()
    {
        if (auraMat == null) return;

        // smoothly go towards targetColor
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorLerpSpeed);
        auraMat.SetColor(AuraColorID, currentColor);
    }

    public void ApplyColorForState(EnemyBaseState state, EnemyStateManager enemy)
    {
        if (auraMat == null || state == null) return;

        Color c = idleColor;

        if (state == enemy.idleState)
            c = idleColor;
        else if (state == enemy.patrolState)
            c = patrolColor;
        else if (state == enemy.chaseState || state == enemy.senseState)
            c = chaseColor;
        else if (state == enemy.attackingState)
            c = attackColor;
        else if (state == enemy.walkBackState)
            c = walkBackColor;
        else if (state == enemy.deadState)
            c = deadColor;

        // Set target color for smooth transition
        targetColor = c;
    }
}
