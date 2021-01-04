using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 380f;
    public int health = 5;
    public Text scoreText;
    public Text healthText;
    public Text winLoseText;
    public Image winLoseBG;

    private Rigidbody _rigidbody;
    private PlayerInput _input;
    private Movement _movement;
    private int score = 0;

    public static event Action OnPlayAgain;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    private void Start()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        _input = new PlayerInput();
        _movement = new Movement(_rigidbody, _input, speed);
    }

    private void Update()
    {
        _input.ReadInput();
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("menu");
    }

    private void FixedUpdate() => _movement.Tick(Time.fixedDeltaTime);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            other.GetComponent<ICollectable>().Collect();
            ++score;
            SetScoreText();
        }

        if (other.CompareTag("Trap"))
            TakeDamage();

        if (other.CompareTag("Goal"))
        {
            SetWin();
            EndController();
            OnPlayAgain?.Invoke();
        }
    }

    void TakeDamage()
    {
        --health;
        SetHealthText();
        if (health == 0)
        {
            SetLose();
            EndController();
            OnPlayAgain?.Invoke();
        }
    }

    private void EndController()
    {
        _rigidbody.drag = 2;
        gameObject.GetComponent<PlayerController>().enabled = false;
    }

    void SetScoreText() => scoreText.text = $"Score: {score}";

    void SetHealthText() => healthText.text = $"Health: {health}";

    void SetWin()
    {
        winLoseBG.gameObject.SetActive(true);
        winLoseText.text = "You Win!";
        winLoseText.color = Color.black;
        winLoseBG.color = Color.green;
    }

    void SetLose()
    {
        winLoseBG.gameObject.SetActive(true);
        winLoseText.text = "Game Over!";
        winLoseText.color = Color.white;
        winLoseBG.color = Color.red;
    }
}
