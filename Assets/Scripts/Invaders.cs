using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    public Invader[] invaderPrefabs = new Invader[5];
    public int rows = 5;
    public int columns = 11;
    public AnimationCurve speed = new AnimationCurve();
    public Projectile missilePrefab;
    public float missileAttackRate = 1.0f;

    private Vector3 direction = Vector2.right;
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;

        InvaderGrid();
    }

    private void InvaderGrid()
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
                Vector3 position = rowPosition;
                position.x += column * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void Update()
    {
        int totalCount = rows * columns;
        int amountAlive = GetAliveCount();
        int amountKilled = totalCount - amountAlive;
        float percentKilled = amountKilled / (float)totalCount;

        float speed = this.speed.Evaluate(percentKilled);
        transform.position += speed * Time.deltaTime * direction;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1f))
            {
                AdvanceRow();
                break;
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        direction = new Vector3(-direction.x, 0f, 0f);

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void MissileAttack()
    {
        int amountAlive = GetAliveCount();

        if (amountAlive == 0)
        {
            return;
        }

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1f / amountAlive))
            {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    public void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

    public int GetAliveCount()
    {
        int count = 0;

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }
}
