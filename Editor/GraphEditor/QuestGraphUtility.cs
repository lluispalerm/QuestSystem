using UnityEngine.UIElements;
using UnityEditor.UIElements;

public static class QuestGraphUtility
{
    public static TextField CreateTextField(string label, string variableToModify, string defaultValue = "")
    {
        var field = new TextField(label)
        {
            value = defaultValue
        };

        field.RegisterValueChangedCallback(evt =>
        {
            variableToModify = evt.newValue;
        });

        return field;
    }

    public static IntegerField CreateIntegerField(string label, int variableToModify, int defaultValue =  0) 
    {
        var field = new IntegerField(label)
        {
            value = defaultValue
        };

        field.RegisterValueChangedCallback(evt =>
        {
            variableToModify = evt.newValue;
        });

        return field;
    }
}
