# QuestSystem
This is a Quest system with saves and a custom graph editor for Unity

You can import directly this package into your unity project by going "Window > Package Manager > + > Add package from git url" and pasting this: https://github.com/lluispalerm/QuestSystem.git (is the same one you can get from github if you click the green button code and get the https url)

For a demo project of this tool go to: https://github.com/lluispalerm/QuestSystem-Demo

## User manual

### Conceps
All of this are scriptable objects exept the manager

#### Quest Objective
The quest objectives are the minimun unit of the quest. It tracks how many interactions are requaired to complete itself and holds information about it. The objectives can be hidden from the player if they are suposed to be secrets.
#### Node Quest
The nodes of a quest can have multiple objectives. When all of of them are completed or the objective is an important one (autoExitOnCompleted) the state of the quest will go to the next node especified. It also has and extra field for am text file in the case you want to use some sort of dialog system.
#### Quest
A quest has 1 initial point but can have endless paths. If you use the graphic tool provided by this project inside unity it will also save the information of the nodes. If you use the a day counting for your game you can specify when in those days the quest will be avaliable. You can also say if the quest is a main quest or not.
#### Quest Log 
You can only have one quest log per game. It keeps track of all de current assigned quest, the completed ones and , even the failed ones. You can also keep track of the day of the game here. 
#### Quest Manager 
All the previos points where "just data", the manager is the one that will update an all of the parameters.

### Scene
#### Quest On Object World
This script will have a list to objects that need to be active in the specified poinst of the quest (When a node Quest is current)
#### Quest Giver
A monobehaviour to atach to a NPC or a triger and start a Quest. It also has and extra text file field.

#### Quest Updater
A monobehaviour to atach to a NPC or a triger that updates the the estate of a Quest. Normally it will update and objective, if its compled it updates the node, if its completed updates the quest and if the the node was an endpoint it moves the quest in the Quest Log. It also has and extra text file field.

#### IQuestInteraction
In order to interact with the updater and the giver you can use the Interact methon inplemented from the IQuestInteraction interface, or you can use a custom one.

For more information about c# interface: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface 

### Editor Tools
The package contais a graphView based editor of the quest 

![Nodes Gif](https://lh3.googleusercontent.com/keep-bbsk/AJ5RgYBBqoUjVYWHNMsPB4YmpN7Bdi4fsu5Bg7Sv3yyKhO7nbtO6qISLYWjtAX6NMzir119fbxyyWAyx_ja5wegoed4MeCWbmDKXTi1xtwKnVnj5hu4c=s972)

### Save system
When you update and finish an objective there is a save system (By default is disabled in the editor because its alredy saved on the scriptable assets).

## Code documentation
If you want to modify or contribute to this tool check the wiki for more information: wiki
