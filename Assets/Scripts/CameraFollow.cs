using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public float followSpeed = 10f; // Tốc độ theo dõi của camera, cao hơn để phản ứng nhanh hơn
    private Transform ninja; // Ninja cần theo dõi
    private float lastNinjaY; // Lưu trữ vị trí y trước đó của ninja
    private bool isJumping; // Biến để theo dõi trạng thái nhảy của ninja

    private void Awake() {
        Application.targetFrameRate = 60;
    }

    void Start() {
        // Tìm vật thể có tag "Ninja"
        GameObject ninjaObject = GameObject.FindGameObjectWithTag("Ninja");

        if (ninjaObject != null) {
            ninja = ninjaObject.transform;
            lastNinjaY = ninja.position.y;
        } else {
            Debug.LogError("Không tìm thấy đối tượng nào với tag 'Ninja'.");
            enabled = false; // Vô hiệu hóa script nếu không tìm thấy ninja
        }
    }

    void Update() {
        if (ninja == null) return; // Không làm gì nếu ninja không tồn tại

        // Kiểm tra trạng thái nhảy của ninja
        if (ninja.position.y > lastNinjaY) {
            isJumping = true;  // Ninja đang nhảy lên
        } else if (ninja.position.y < lastNinjaY) {
            isJumping = false; // Ninja đang rơi xuống
        }

        lastNinjaY = ninja.position.y; // Cập nhật vị trí y của ninja

        // Theo dõi ninja nếu đang nhảy lên
        if (isJumping) {
            Vector3 newCameraPosition = new Vector3(transform.position.x, ninja.position.y, transform.position.z);
            // Di chuyển camera đến vị trí của ninja nhanh hơn, nhưng vẫn mượt
            transform.position = Vector3.Lerp(transform.position, newCameraPosition, followSpeed * Time.deltaTime);
        }
    }
}
