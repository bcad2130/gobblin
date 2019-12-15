using System.Collections;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    public string state;
    public List<Turn> turns;
    public int turnNumber;

    void Start() 
    {
        state = "";
        turns = new List<Turn>();
        turnNumber = 0;
    
    }

    public void NextTurn() 
    {
        Unit currentUnit = turn[turnNumber].unit;
        turnNumber++;
        if(currentUnit.team == "player") {
            state = "choose";
            ChooseMove(currentUnit);
        } else if(currentUnit.team == "enemy") {
            state = "enemy turn";
            // EnemyAction(currentUnit);
            currentUnit.RandomMove();
        } else {
            print("Uh oh");
        }

        // foreach (Turn turn in turns)
        // {
        //     if(turn.unit.team == "player") {
        //         state = "choose";
        //     }
        // }
    }

    private void ChooseMove(Unit currentUnit) { //should this be in the unit class?
        //start co routine, show moves for that unit

        currentUnit.ShowMoves(currentUnit.moves);
    }



    private void EnemyAction(Unit currentUnit) { //should this be in the unit class?
        //TODO: choose a random move, eventually i'll set odds of each move getting chosen too
        Move currentMove = currentUnit.moves[0];
    }

    void Update() 
    {
        if(state == "choose") {
            //check for controller input
        }
    }
}

private class Turn : MonoBehaviour
{
    public Unit unit;
    // public int turnNumber;

}

private class Unit : MonoBehaviour
{
    public BattleManager bm;
    public string team;
    public List<Move> moves;
    public List<Status> currentStatusEffects;

    void Start() {
        moves = new List<Move>();

    }
    
    public void RandomMove() {
        //TODO: choose a random move, eventually i'll set odds of each move getting chosen too
        Move currentMove = moves[0];
        
        //TODO: choose a random target
        List<Unit> targets = new List<Unit>();

        Action(this,currentMove,targets);
    }

    private void ShowMoves(List<Move> moves) { // should this be in the battle manager?
        //TODO: display moves in a box, with cursor

        Move selectedMove = moves[0];

        //TODO: choose target, if applicable
        List<Unit> targets = new List<Unit>();


        Action(this,selectedMove,targets);
    }

    private void Action(Unit actor, Move selectedMove, List<Unit> targets) {
        //TODO: somehow, this has to do any sort of action, from the actor to the targets

        //and then we do the animation

        //and then its back to the next turn
        bm.NextTurn();
    }
}

public class Move : MonoBehaviour 
{
    public string targets; //can be self, allies, enemies, or all (maybe random ally, random enemy, random all)

    public int attackDamage;
    public int healDamage;
    public List<Status> afflictStatusEffects; //this could also be its own class, status effects, and a unit would have a list of these
    public List<Status> cureStatusEffects;

}