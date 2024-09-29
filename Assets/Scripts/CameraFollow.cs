using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform ninja; // Ninja cần theo dõi
    public float followSpeed = 2f; // Tốc độ di chuyển của camera
    private float lastNinjaY; // Lưu trữ vị trí y trước đó của ninja
    private bool isJumping; // Biến để theo dõi trạng thái nhảy của ninja

    private void Awake() {
        Application.targetFrameRate = 60;
    }

    void Start() {
        if (ninja == null) {
            Debug.LogError("Ninja không được gán cho CameraFollow script.");
            enabled = false; // Vô hiệu hóa script nếu không có ninja
            return;
        }

        lastNinjaY = ninja.position.y;
    }

    void Update() {
        // Kiểm tra trạng thái nhảy của ninja
        if (ninja.position.y > lastNinjaY) // Ninja đang nhảy lên
        {
            isJumping = true;
        } else if (ninja.position.y < lastNinjaY) // Ninja đang rơi xuống
          {
            isJumping = false;
        }

        lastNinjaY = ninja.position.y; // Cập nhật vị trí y của ninja

        // Theo dõi ninja nếu đang nhảy lên
        if (isJumping) {
            Vector3 newCameraPosition = new Vector3(transform.position.x, ninja.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newCameraPosition, followSpeed * Time.deltaTime);
        }
    }
}
