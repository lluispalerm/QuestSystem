using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace QuestSystem.QuestEditor
{
    [System.Serializable]
    public class QuestObjectiveGraph : VisualElement
    {
        public string keyName;
        public int maxItems;
        public int actualItems;
        public string description;
        public bool hiddenObjective;
        public bool autoExitOnCompleted;


        public QuestObjectiveGraph(string key = "", int max = 0, int actual = 0, string des = "", bool hiddenObjectiveDefault = false, bool autoExitOnCompletedDefault = false)
        {

            //keyName
            var propertyKeyNameField = new TextField("keyName:")
            {
                value = key
            };

            propertyKeyNameField.RegisterValueChangedCallback(evt =>
            {
                keyName = evt.newValue;
            });


            this.Add(propertyKeyNameField);


            // Max items
            var propertyMaxItemsField = new IntegerField("maxItems:")
            {
                value = max
            };

            propertyMaxItemsField.RegisterValueChangedCallback(evt =>
            {
                maxItems = evt.newValue;
            });

            this.Add(propertyMaxItemsField);

            //  actualItems
            var propertyActualItemsField = new IntegerField("actualItems:")
            {
                value = actual
            };

            propertyActualItemsField.RegisterValueChangedCallback(evt =>
            {
                actualItems = evt.newValue;
            });

            this.Add(propertyActualItemsField);

            //keyName
            var propertyDescriptionField = new TextField("description:")
            {
                value = des
            };

            propertyDescriptionField.RegisterValueChangedCallback(evt =>
            {
                description = evt.newValue;
            });

            this.Add(propertyDescriptionField);

            //Hidden hiddenObjective
            var propertyHiddenField = new Toggle();
            propertyHiddenField.label = "Hidden objective";

            propertyHiddenField.RegisterValueChangedCallback(evt =>
            {
                hiddenObjective = evt.newValue;
            });
            propertyHiddenField.SetValueWithoutNotify(hiddenObjectiveDefault);

            this.Add(propertyHiddenField);

            //Auto ecit on complete
            var propertyAutoExitField = new Toggle();
            propertyAutoExitField.label = "Auto exit on complete";
            
            propertyAutoExitField.RegisterValueChangedCallback(evt =>
            {
                autoExitOnCompleted = evt.newValue;
            });
            propertyAutoExitField.SetValueWithoutNotify(autoExitOnCompletedDefault);

            this.Add(propertyAutoExitField);




        }

    }
}