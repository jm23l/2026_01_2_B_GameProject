using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "DialogDatabaseSo", menuName = "Dialog System/DialogDatabaseSo")]
public class DialogDatabaseSo : ScriptableObject
{
    public List<DialogSO> dialogs = new List<DialogSO>();

    private Dictionary<int, DialogSO> dialogById;                       // 캐싱을 위한 딕셔너리 사용

    public void Initailize()
    {
        dialogById = new Dictionary<int, DialogSO>();

        foreach (var dialog in dialogs)
        {
            if(dialog != null)
            {
                dialogById[dialog.id] = dialog;
            }
        }
    }

    public DialogSO GetDialogById(int id)
    {
        if(dialogById == null)
        {
            Initailize();
        }

        if(dialogById.TryGetValue(id, out DialogSO dialog))
        {
            return dialog;
        }

        return null; 
    }
}
