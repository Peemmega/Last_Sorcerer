using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitYAxis : MonoBehaviour
{
    public float maxY = 1.0f; // ค่าแกน Y สูงสุดที่อนุญาต
    public float minY = 0.0f; // ค่าแกน Y ต่ำสุด (พื้น)

    void Update()
    {
        // ตรวจสอบว่าแกน Y เกินขอบเขตที่กำหนดหรือไม่
        if (transform.position.y > maxY)
        {
            // ถ้าเกินขอบเขต ให้ตั้งค่ากลับมาเป็น maxY
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
        else if (transform.position.y < minY)
        {
            // ถ้าต่ำกว่าพื้น ให้ตั้งค่ากลับมาเป็น minY
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
    }
}
