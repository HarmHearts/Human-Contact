using HC.Data;
using HC.ScriptableObjects;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : SerializedMonoBehaviour
{
    public List<HumanObject> HumanAssignment;
    public HumanController HumanController;

    public Transform TileRoot;
    
    public List<Human> Humans;
    public Human SelectedHuman;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (HumanObject humanObject in HumanAssignment)
        {
            HumanController controller = Instantiate(HumanController);
            HumanObject obj = Instantiate(humanObject, TileRoot);

            controller.Human = obj.Human;
            Humans.Add(controller.Human);
        }

        SelectedHuman = Humans[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchSelectedHuman()
    {
        index = index + 1;
        if (index >= Humans.Count) index = 0;

        SelectedHuman = Humans[index];
    }
}
