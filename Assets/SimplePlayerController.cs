using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class SimplePlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f; // Geschwindigkeit der Bewegung
    [SerializeField] private float jumpForce = 5f; // Sprungkraft
    [SerializeField] private float saltoSpeed = 360f; // Drehgeschwindigkeit für den Salto in Grad/Sekunde

    private Rigidbody _rigidbody;
    private Animator _animator;
    public bool _isGrounded;
    public bool _is2DMode = false; // Ob der 2D-Modus aktiv ist
    public bool _isJumping = false; // Flag für den Sprung
    public bool _isDead=false;

    private void Awake()
    {
        // Referenz auf das Rigidbody-Objekt speichern
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        // Rotation um X- und Z-Achse sperren, damit die Kapsel nicht kippt
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnEnable()
    {
        // Abonnieren des Moduswechsel-Ereignisses
        CameraSwitch.OnModeSwitched += HandleModeSwitched;
    }

    private void OnDisable()
    {
        // Abmelden vom Moduswechsel-Ereignis
        CameraSwitch.OnModeSwitched -= HandleModeSwitched;
    }

    private void HandleModeSwitched(bool is2DMode)
    {
        // Modus aktualisieren
        _is2DMode = is2DMode;
    }

    private void Update()
    {
        // Bewegungseingabe
        float horizontal = Input.GetAxis("Horizontal"); // A/D oder Pfeiltasten
        float vertical = Input.GetAxis("Vertical"); // W/S oder Pfeiltasten

        Vector3 movement;

        if (_is2DMode)
        {
            // Im 2D-Modus nur Bewegung entlang der Z-Achse (links/rechts)
            movement = new Vector3(0f, 0f, horizontal).normalized * movementSpeed;
        }
        else
        {
            // Im 3D-Modus Bewegung entlang der X- und Z-Achse
            movement = new Vector3(horizontal, 0f, vertical).normalized * movementSpeed;
        }

        // Bewegung in Rigidbody umsetzen (kein Y für Sprung hier)
        Vector3 newVelocity = new Vector3(movement.x, _rigidbody.velocity.y, movement.z);
        _rigidbody.velocity = newVelocity;

        if (movement.magnitude > 0f) // Bewegungseingabe vorhanden
        {
            _animator.SetFloat("Speed", 1f);
        } else // Keine Bewegungseingabe
        {
            _animator.SetFloat("Speed", 0f);
        }

        // Sprung
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _animator.SetTrigger("Salto");
            
        }
        if (transform.position.y<-6){
            _isDead=true;
            Time.timeScale=0.0001f;
            StartCoroutine(wait());
            
        }

        
    }
    private void OnGUI(){
        if(_isDead==true){
            string gameOverMessage="Game Over";
            gameOverMessage = GUI.TextField(new Rect(900, 400, 100, 20), gameOverMessage, 25);   
        }     
    }
    private void gameOver(){
        Time.timeScale=1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
    }
    IEnumerator wait(){
        yield return new WaitForSecondsRealtime(6); 
        gameOver();   
    }

    private void FixedUpdate()
    {
        // FixedUpdate wird verwendet, um die physikalische Simulation zu behandeln
    }

    private void OnCollisionStay(Collision collision)
    {
        // Prüfen, ob die Kapsel den Boden berührt
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Wenn die Kapsel den Boden verlässt
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}
