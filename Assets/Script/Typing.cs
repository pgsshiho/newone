using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Typing : MonoBehaviour
{
    public TMP_Text story;
    private string a;
    private string b;

    void Start()
    {
        // ���
        a = "����� ���� �̳��� ���� ������ ����Դϴ�.";
        b = "����� ������ ���μ� \n������ ���� ���Ѿ߸� �մϴ�.";
        StartCoroutine(typingSequence());
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Start");
        }
    }

    IEnumerator typingSequence()
    {
        yield return StartCoroutine(typing(a, story));
        yield return new WaitForSeconds(0.6f);
        yield return StartCoroutine(typing(b, story));
        
    }

    IEnumerator typing(string talk, TMP_Text textComponent)
    {
        // �ؽ�Ʈ�� null ������ ����
        textComponent.text = string.Empty;

        for (int i = 0; i < talk.Length; i++)
        {
            textComponent.text += talk[i];
            // �ӵ�
            yield return new WaitForSeconds(0.18f);
        }
    }
    
}