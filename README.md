# Evacuation-Simulator
C# Project

Program allows to see simple geometry room evacuation based on cellular automata with few trivial rules. 
We define 3 different type of cells:
1) floor cells - each can contain a single pedestrian
2) door cells - represent room's exit
3) walls - behave as obstacles, cannot be occupied by pedestrians

STATIC FLOOR FIELD
In order to generate floor field we assign exit cells with value 0, walls as 500. Horizontal and vertical neighbours 
of a cell with a floor fied value v are assigned with value v + 1. Diagonal neighbours are assigned wih value v + 1.5.
If the above procedure results in conflict and two cells try to assign a different value to the same neighbouring cell,
then the lowest value is always used.

PEDESTRIANS:
They move in an asynchronous fashion using shuffle update. At the beginning of each time step a permutation of 
pedestrians present in the system is created. This permutation governs the order in which the pedestrian
move. A pedestrians can move in all directions, including diagonal ones. As a result, we use Moore neighbourhood 
and take into account eight neighbouring cells. A pedestrian always chooses the neighbouring cell with the lowest 
floor field value.

In order to create more realistic simulation, we added a 'panic parameter', which can be set from 0 to 1. Panic parameter 
is a probability of a pedestrian staying at the same cell at the next time step.

