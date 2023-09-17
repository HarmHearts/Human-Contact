using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine;

namespace HC.Data
{
    [CreateAssetMenu(fileName = "Body Plan", menuName = "HC/Body Plan")]
    public class BodyPlan : SerializedScriptableObject
    {
        private static bool _toggleData;
        private static DynamicLimbBodyPart _selectedPart;

        [Button(ButtonSizes.Medium)]
        private static void ToggleData()
        {
            _toggleData = !_toggleData;
        }

        [TableMatrix(DrawElementMethod = "DrawBodyTable", SquareCells = true, HorizontalTitle = "Body Plan")]
        [OdinSerialize] public BodyPart[,] BodyPartGrid = new BodyPart[12,12];

        [OnInspectorInitAttribute("Init")]
        private void Init()
        {
        }

        public void AddToBody(IBody body)
        {
            for (int j = 0; j < BodyPartGrid.GetLength(1); j++)
            {
                for (int i = 0; i < BodyPartGrid.GetLength(0); i++)
                {
                    if (BodyPartGrid[i,j] != null) 
                    {
                        BodyPartGrid[i, j].Body = body;
                        BodyPartGrid[i, j].RelativePosition = new Vector2Int(i, j) - body.Anchor;
                    }
                }
            }
        }

        static BodyPart DrawBodyTable(Rect rect, BodyPart value)
        {
            if (_toggleData)
            {
                return DrawPureTable(rect, value);
            }else
            {
                return DrawDataTable(rect, value);
            }

        }

        static BodyPart DrawDataTable(Rect rect, BodyPart value)
        {
            Rect top = new Rect(rect.position, new Vector2(rect.size.x, 16));
            Rect enumrect = new Rect(rect.position + new Vector2(0, 16), new Vector2(rect.size.x, 16));
            Rect sprite = new Rect(rect.position + new Vector2(0, 32), new Vector2(rect.size.x, rect.size.y - 32));

            BodyPart res = (BodyPart)SirenixEditorFields.PolymorphicObjectField(top, GUIContent.none, value, typeof(BodyPart), true);

            if (res != null)
            {
                if (res is StaticBodyPart)
                {
                    res.Type = (BodyPartType)SirenixEditorFields.EnumDropdown(enumrect, res.Type);
                    res.Sprite = (Sprite)SirenixEditorFields.UnityPreviewObjectField(sprite, GUIContent.none, res.Sprite, typeof(Sprite), false);
                } else if (res is DynamicLimbBodyPart)
                {
                    if (SirenixEditorGUI.IconButton(sprite, EditorIcons.ArrowLeft, "Go to Dynamic Part"))
                    {
                        OdinEditorWindow.InspectObject(res);
                    }
                }
            }

            value = res;

            return value;
        }

        static BodyPart DrawPureTable(Rect rect, BodyPart value)
        {
            if (value != null && value is StaticBodyPart)
            {
                value.Sprite = (Sprite)SirenixEditorFields.UnityPreviewObjectField(rect, GUIContent.none, value.Sprite, typeof(Sprite), false);
            }

            return value;
        }
    }
}