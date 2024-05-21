using Constants;
using Managers;
using UnityEngine;

public class Disc : MonoBehaviour
{
    [SerializeField] private Renderer discRenderer;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Material redMaterial, yellowMaterial;
    [SerializeField] private CurrentPlayer currentPlayer;
    [SerializeField] private Vector2 rowColumn;
    
    private Vector3 targetPosition;
    private bool shouldLerp;

    public void Drop(CurrentPlayer player)
    {
        currentPlayer = player;
        discRenderer.material = player == CurrentPlayer.Player1 ? redMaterial : yellowMaterial;
        Invoke(nameof(Stop), 1.5f);
    }

    private void Stop()
    {
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        GoToProperPosition();
    }

    private void Update()
    {
        if (shouldLerp)
        {
            LerpToTarget(targetPosition);
        }
    }

    private void LerpToTarget(Vector3 target)
    {
        float speed = 1.5f;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, target, step);
    }

    private void GoToProperPosition()
    {
        if (GameManager.Instance.IsGameOver()) return;
        transform.SetParent(Board.Instance.transform);
        targetPosition = Board.Instance.GetGridPosition((int)rowColumn.x, (int)rowColumn.y);
        shouldLerp = true;
    }

    public void Reset()
    {
        DiscPooler.Instance.ReturnDisc(this);
    }

    public void SetColumnRow(int column, int discsCount)
    {
        rowColumn = new Vector2(column, discsCount);
    }

    public CurrentPlayer GetPlayer()
    {
        return currentPlayer;
    }

    private void Release()
    {
        shouldLerp = false;
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        rigidBody.constraints = RigidbodyConstraints.None;
    }

    public void Shoot()
    {
        Release();
        rigidBody.AddForce(Vector3.up * 700);
        rigidBody.AddForce(Vector3.right * 100 * Random.Range(-1, 1));
        rigidBody.AddForce(Vector3.forward * 300 * Random.Range(-1, 1));
    }
}
