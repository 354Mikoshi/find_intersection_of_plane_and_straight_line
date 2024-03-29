﻿using UnityEngine;
using UnityEngine.UI;

public class FindCoordinateOfIntersection : MonoBehaviour
{
    public GameObject[] spheres; //メッシュの頂点の球
    public GameObject intersection_sphere; //交点の球
    public Text coordinate; //画面上に出す座標のテキスト
    private Vector3[] point; //メッシュの頂点の座標
    private Vector3 normal, intersection; //normal: メッシュの法線ベクトル、intersection: 交点の座標
    private MeshMaker meshMaker;
    private float[] abcd; //メッシュが貼られている平面の方程式の係数
    private float parameter; //交点の座標を求めるときに使うパラメータ
    private bool intersectionIsInsidePolygon; //交点がメッシュの中に存在しているかどうか

    private void Start() {
        point = new Vector3[spheres.Length];
        for (int i = 0; i < spheres.Length; i++) point[i] = spheres[i].gameObject.transform.position;
        normal = CalculateOuterProduct(point[0], point[1], point[2]); //3点point[0]~[2]を通る平面の法線ベクトルを求める
    }

    private void Update() {
        abcd = CalculateEquationOfPlane(point[0], point[1], point[2]);

        //gameObject.transform.rotation * Vector3.forward, gameObject.transform.positionはカメラの視線の方向ベクトル
        intersection = CalculateCoordinateOfIntersection(abcd, gameObject.transform.rotation * Vector3.forward, gameObject.transform.position);

        intersectionIsInsidePolygon = WhetherIntersectionIsInsidePolygon(point, intersection, normal);

        //交点がメッシュの内部にあるときだけ交点をアクティブにする
        intersection_sphere.SetActive(intersectionIsInsidePolygon);

        //交点がメッシュの内部にあり、かつ交点がカメラの前側にあるときに、テキストに交点の座標を表示する
        if (intersectionIsInsidePolygon && parameter > 0f) coordinate.text = "(" + intersection.x.ToString("F2") + ", " + intersection.y.ToString("F2") + ", " + intersection.z.ToString("F2") + ")";
        else coordinate.text = "※交点が存在しません※";
    }

    //変数intersectionの値を読み取る、書き込むプロパティ
    public Vector3 Intersection {
        get { return intersection; }
        private set { intersection = value;  }
    }

    //このメソッドは、vec1,vec2,vec3の3点を通る平面の方程式ax+by+cz+d=0のa,b,c,dを配列で返す
    private float[] CalculateEquationOfPlane(Vector3 vec1, Vector3 vec2, Vector3 vec3) {
        float[] ans = new float[]{
            normal.x,
            normal.y,
            normal.z,
            -normal.x * vec1.x - normal.y * vec1.y - normal.z * vec1.z
        };
        return ans;
    }

    //このメソッドでは、カメラの視線とメッシュとの交点の座標が求められる
    private Vector3 CalculateCoordinateOfIntersection(float[] plane, Vector3 angle, Vector3 position) {
        parameter = -(plane[0] * position.x + plane[1] * position.y + plane[2] * position.z + plane[3]) / (plane[0] * angle.x + plane[1] * angle.y + plane[2] * angle.z);
        float x = angle.x * parameter + position.x;
        float y = angle.y * parameter + position.y;
        float z = angle.z * parameter + position.z;
        return new Vector3(x, y, z);
    }

    //このメソッドでは、vec1,vec2,vec3の3点を通る平面の法線ベクトルが求められる
    private Vector3 CalculateOuterProduct(Vector3 vec1, Vector3 vec2, Vector3 vec3) {
        Vector3 tmp1 = vec1 - vec2;
        Vector3 tmp2 = vec1 - vec3;
        return Vector3.Cross(tmp1, tmp2); //Vector3.Crossは外積を求めるメソッド
    }

    //このメソッドは引用させていただきました
    private bool WhetherIntersectionIsInsidePolygon(Vector3[] vertices, Vector3 intersection, Vector3 normal) {
        float angle_sum = 0f;
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 tmp1 = vertices[i] - intersection;
            Vector3 tmp2 = vertices[(i + 1) % vertices.Length] - intersection;
            float angle = Vector3.Angle(tmp1, tmp2);
            Vector3 cross = Vector3.Cross(tmp1, tmp2);
            if (Vector3.Dot(cross, normal) < 0) angle *= -1;
            angle_sum += angle;
        }
        angle_sum /= 360f;
        return Mathf.Abs(angle_sum) >= 0.1f;
    }
}
