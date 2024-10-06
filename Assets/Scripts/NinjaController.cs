using UnityEngine;
using System.Collections;

public class NinjaController : MonoBehaviour {
    public float baseJumpForce = 10f; // Lực nhảy cơ bản
    public float platformHideDelay = 1f; // Thời gian để ẩn platform sau va chạm
    public float followSpeed = 2f; // Tốc độ di chuyển của camera
    public float delayBeforeJump = 1f; // Thời gian đứng yên trên không trước khi nhảy
    public float bounceForce = 5f; // Lực bật lại khi va chạm với Wall

    public GameObject bulletPrefab; // Prefab của đạn
    public Transform bulletSpawnPoint; // Vị trí để bắn đạn
    public float bulletSpeed = 10f; // Tốc độ đạn

    public Rigidbody2D rb;
    private bool isJumping; // Biến để theo dõi trạng thái nhảy của ninja
    private bool isWaitingToJump; // Biến theo dõi khi ninja đang đứng yên chờ nhảy

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
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

    // Phát hiện va chạm với các đối tượng có tag "Wall" dùng OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            // Bật ninja lại khi va chạm với Wall
            BounceBack(collision);
        }
    }

    // Hàm để bật ninja lại khi va chạm với Wall
    void BounceBack(Collision2D collision) {
        // Lấy hướng của va chạm
        Vector2 collisionNormal = collision.contacts[0].normal;

        // Áp dụng lực bật lại theo hướng ngược lại của va chạm
        rb.velocity = collisionNormal * bounceForce;
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

    // Hàm để bắn đạn khi ấn button
    public void ShootBullet() {
        if (bulletPrefab != null && bulletSpawnPoint != null) {
            // Tạo một viên đạn từ prefab tại vị trí bắn
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

            // Lấy Rigidbody2D của đạn để áp dụng lực
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            // Bắn đạn bay lên theo hướng trục y
            bulletRb.velocity = Vector2.up * bulletSpeed;
        } else {
            Debug.LogWarning("Bullet prefab or spawn point is not assigned.");
        }
    }

    // Hàm để ninja bất động, phá hủy enemy trong camera và sau đó rơi xuống
    public void FreezeAndDestroyEnemies() {
        FreezeAndDestroyCoroutine();
    }

    private void FreezeAndDestroyCoroutine() {
        // Tạm thời dừng ninja
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        // Lấy tất cả các enemy trong camera
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
            // Chỉ phá hủy những enemy trong camera hiện tại
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1) {
                Destroy(enemy);
            }
        }
        //AllowToMove();
    }

    public void AllowToMove() {
        //StartCoroutine(AllowToMoveCoroutine());
        AllowToMoveCoroutine();
    }

    void AllowToMoveCoroutine() {
        // Chờ 1 giây trước khi ninja rơi xuống
        //yield return new WaitForSeconds(1f);

        // Kích hoạt lại trọng lực cho ninja
        rb.isKinematic = false;
    }
}
