using UnityEngine;

public class TPSCamera : MonoBehaviour {
    public GameObject targetObj;
    public float rotatespeed;
    private Vector3 targetPos;

    private void Start() {
        targetPos = targetObj.transform.position;
    }

    private void Update() {
        // targetの移動量分、自分（カメラ）も移動する
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        // マウスの移動量
        float mouseInputX = Input.GetAxis("R_Stick_H");
        float mouseInputY = Input.GetAxis("R_Stick_V");
        // targetの位置のY軸を中心に、回転（公転）する
        transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * rotatespeed);
        // カメラの垂直移動（※角度制限なし、必要が無ければコメントアウト）
        transform.RotateAround(targetPos, transform.right, mouseInputY * Time.deltaTime * rotatespeed);


        /*RTで拡大、LTで縮小*/
        float LorR = Input.GetAxis("L_R_Trigger");
        Vector3 distance_between_camera_and_target = targetObj.transform.position - transform.position;
        if (LorR > 0) {
            transform.position += distance_between_camera_and_target / 50;
        }
        else if (LorR < 0) {
            transform.position += -distance_between_camera_and_target / 50;
        }

    }
}
