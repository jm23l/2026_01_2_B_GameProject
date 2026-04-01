using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [Header("Dialog References")]
    [SerializeField] private DialogDatabaseSo dialogDatabase;

    [Header("UI References")]
    [SerializeField] private GameObject dialogPanel;
    
    [SerializeField] private Image portraitImage;

    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Button NextButton;

    [Header("Dialog Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool useTypewriterEffect = true;

    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private DialogSO currentDialog;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if(dialogDatabase != null)
        {
            dialogDatabase.Initailize();
        }
        else
        {
            Debug.LogError("Dialog Database is not assigned to Dialog Manager");
        }

        if(NextButton != null)
        {
            NextButton.onClick.AddListener(NextDialog);
        }
        else
        {
            Debug.LogError("Next Button is Not assigned");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // UI초기화 후 대화 시작 (ID 1)
        CloseDialog();
        StartDialog(1);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ID로 대화 시작
    public void StartDialog(int dialogId)
    {
        DialogSO dialog = dialogDatabase.GetDialogById(dialogId);
        if(dialog != null)
        {
            StartDialog(dialog);
        }
        else
        {
            Debug.LogError($"Dialog with ID {dialogId} not found!");
        }
    }

    //DialogSo로 대화 시작
    public void StartDialog(DialogSO dialog)
    {
        if (dialog == null) return;

        currentDialog = dialog;
        ShowDialog();
        dialogPanel.SetActive(true);
    }

    public void ShowDialog()
    {
        if (currentDialog == null)
        characterNameText.text = currentDialog.characterName;       // 캐릭터 이름 설정

        if(useTypewriterEffect)
        {
            StartTypingEffect(currentDialog.text);
        }
        else
        {
            dialogText.text = currentDialog.text;                   // 대화 텍스트 설정
        }


        // 초상화 설정 (새로 추가된 부분)
        if (currentDialog.portrait != null)
        {
            portraitImage.sprite = currentDialog.portrait;
            portraitImage.gameObject.SetActive(true);
        }
        else if(!string.IsNullOrEmpty(currentDialog.portraitPath))
        {
            // Resources 폴더에서 이미지 로드
            Sprite portrait = Resources.Load<Sprite>(currentDialog.portraitPath);
            
            if(portrait != null)
            {
                portraitImage.sprite = portrait;
                portraitImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"Portrait not founf at path : {currentDialog.portraitPath}");
                portraitImage.gameObject.SetActive(false);
            }
        }
        else
        {
            portraitImage.gameObject.SetActive(false);
        }
        
    }

    public void CloseDialog()                                       // 대화 종료
    {
        dialogPanel.SetActive(false);
        currentDialog = null;
        StopTypingEffect();                                         // 타이핑 효과 중지 추가
    }

    public void NextDialog()
    {
        if (isTyping)                                               // 타이핑 중이면 타이핑 완료 처리
        {
            StopTypingEffect();
            dialogText.text = currentDialog.text;
            isTyping = false;
            return;
        }

        if (currentDialog != null && currentDialog.nextId > 0)
        {
            DialogSO nextDialog = dialogDatabase.GetDialogById(currentDialog.nextId);
            if (nextDialog != null)
            {
                currentDialog = nextDialog;
                ShowDialog();
            }
            else
            {
                CloseDialog();
            }
        }

        CloseDialog();
    }

    private IEnumerator TypeText(string text)
    {
        dialogText.text = "";
        foreach (char c in text)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    private void StopTypingEffect()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    // 타이핑 효과 함수 시작
    private void StartTypingEffect(string text)
    {
        isTyping = true;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(text));
    }
    
}
