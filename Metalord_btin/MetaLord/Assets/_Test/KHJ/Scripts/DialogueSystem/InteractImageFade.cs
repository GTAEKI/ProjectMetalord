using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InteractImageFade : MonoBehaviour
{
    public Transform playerTransform;
    public float distanceThreshold = 5f; // �����ϰ��� �ϴ� �Ÿ� �Ӱ谪
    public Image image;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
        }
    }
    void Update()
    {
        FadeNpcInteractImage();
    }

    private void FadeNpcInteractImage()
    {
        if(playerTransform != null) 
        {
            // NPC�� �÷��̾� ���� �Ÿ� ���
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            // �Ÿ��� ���� ���� �� ����
            float alphaValue = Mathf.InverseLerp(0f, distanceThreshold, distance);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alphaValue);
        }
    }

}
