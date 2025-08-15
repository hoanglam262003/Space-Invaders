using UnityEngine;

public class Invaders : MonoBehaviour
{
    public Invader[] invaderPrefabs;
    public int rows = 5;
    public int columns = 11;
    public AnimationCurve speed;
    public int amountKilled { get; private set; }
    public int totalInvaders => this.rows * this.columns;
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;

    private Vector3 direction = Vector2.right;

    private void Awake()
    {
        for (int row = 0; row < this.rows; row++)
        {
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 center = new Vector2(-width / 2, -height / 2); 
            Vector3 rowPosition = new Vector3(center.x, center.y + (row * 2.0f), 0.0f);
            for (int column = 0; column < this.columns; column++)
            {
                Invader invader = Instantiate(this.invaderPrefabs[row], this.transform);
                invader.killed += InvaderKilled;
                Vector3 position = rowPosition;
                position.x += column * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Update()
    {
        this.transform.position += direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f))
            {
                AdvanceRow();
            } else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f))
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void InvaderKilled()
    {
        this.amountKilled++;
    }
}
