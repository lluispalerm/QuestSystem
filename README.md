# QuestSystem
This is a premade Quest system with saves and a custom graph editor

For a demo of this tool go to: https://github.com/lluispalerm/QuestSystem-Demo

## User manual

### Conceps
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


### Editor Tools

### Save system
## Code documentation
If you want to modify or contribute to this tool check the wiki for more information: wiki
