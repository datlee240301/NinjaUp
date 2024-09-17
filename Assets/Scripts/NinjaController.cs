using UnityEngine;
using System.Collections;

public class NinjaController : MonoBehaviour {
    public float baseJumpForce = 10f; // Lực nhảy cơ bản
    public float platformHideDelay = 1f; // Thời gian để ẩn platform sau va chạm
    public float screenPadding = 0.5f; // Khoảng cách nhỏ để tránh ninja ra ngoài màn hình
    public float followSpeed = 2f; // Tốc độ di chuyển của camera
    public float delayBeforeJump = 1f; // Thời gian đứng yên trên không trước khi nhảy

    private Rigidbody2D rb;
    private float screenLeft;
    private float screenRight;
    private bool isJumping; // Biến để theo dõi trạng thái nhảy của ninja
    private bool isWaitingToJump; // Biến theo dõi khi ninja đang đứng yên chờ nhảy

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        // Tính toán biên màn hình dựa trên Camera chính
        screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + screenPadding;
        screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - screenPadding;
    }

    void Update() {
        // Kiểm tra trạng thái nhảy của ninja
        if (rb.velocity.y > 0 && !isWaitingToJump) // Ninja đang nhảy lên, nhưng không đang chờ nhảy
        {
            isJumping = true;
        } else if (rb.velocity.y < 0 && !isWaitingToJump) // Ninja đang rơi xuống
          {
            isJumping = false;
        }

        // Theo dõi ninja nếu đang nhảy lên
        if (isJumping) {
            Vector3 newCameraPosition = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCameraPosition, followSpeed * Time.deltaTime);
        }

        // Kiểm tra nếu ninja chạm vào cạnh trái hoặc cạnh phải của màn hình
        if (transform.position.x <= screenLeft || transform.position.x >= screenRight) {
            // Đảo ngược vận tốc trên trục x để bật lại
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }

        // Kiểm tra nếu ninja biến mất khỏi camera
        CheckNinjaOutOfCamera();
    }

    // Kiểm tra nếu ninja ra khỏi camera
    void CheckNinjaOutOfCamera() {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        // Nếu ninja ra ngoài màn hình (viewPos.x hoặc viewPos.y ngoài phạm vi [0,1])
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) {
            Debug.Log("vip pro max");
        }
    }

    // Gọi hàm nhảy theo hướng vuông góc với platform
    public void Jump(Vector2 jumpDirection, float jumpForce) {
        rb.velocity = jumpDirection * jumpForce; // Nhảy theo hướng của platform
    }

    // Phát hiện va chạm với platform
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Platform") && !isWaitingToJump) {
            // Lấy thông tin của platform
            Collider2D platformCollider = collision.GetComponent<Collider2D>();
            float platformLength = platformCollider.bounds.size.x; // Lấy chiều dài của platform

            // Tính toán lực nhảy tỉ lệ nghịch với độ dài của platform
            float adjustedJumpForce = baseJumpForce / platformLength;

            // Lấy góc của platform và tính vector vuông góc
            Transform platformTransform = collision.transform;
            Vector2 platformDirection = platformTransform.right; // Hướng của platform (theo chiều x của platform)
            Vector2 jumpDirection = Vector2.Perpendicular(platformDirection).normalized; // Tạo vector vuông góc và chuẩn hóa

            // Bắt đầu quá trình chờ 1 giây trước khi nhảy
            StartCoroutine(WaitBeforeJump(jumpDirection, adjustedJumpForce));

            // Ẩn platform sau 1 giây
            StartCoroutine(HidePlatformAfterDelay(collision.gameObject, platformHideDelay));
        }
    }

    // Coroutine để đứng yên 1 giây trước khi nhảy
    IEnumerator WaitBeforeJump(Vector2 jumpDirection, float jumpForce) {
        isWaitingToJump = true;

        // Dừng chuyển động của ninja (tạm thời ngừng lực và vận tốc)
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // Ngăn không cho ninja bị tác động bởi trọng lực

        // Chờ 1 giây
        yield return new WaitForSeconds(delayBeforeJump);

        // Kích hoạt nhảy sau khi đã đợi đủ thời gian
        rb.isKinematic = false; // Kích hoạt lại trọng lực
        Jump(jumpDirection, jumpForce);

        isWaitingToJump = false;
    }

    // Coroutine để ẩn platform sau 1 giây
    IEnumerator HidePlatformAfterDelay(GameObject platform, float delay) {
        yield return new WaitForSeconds(delay);
        platform.SetActive(false); // Ẩn platform
    }
}
