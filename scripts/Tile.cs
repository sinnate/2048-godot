using Godot;
using System;
using System.Collections.Generic;
public class Tile : Node2D
{
        private int score = 0;
        private int MaxComb = 0;
        private struct GridStruct {
        public int x;
        public int y;
        public Godot.Control res ;
        public GridStruct(int nx,int ny,Godot.Control Path){
            x = nx;
            y = ny;
            res = Path;
            res.RectPosition = new Vector2(x*offeset,y*offeset);
            res.Call("setCell",0);
        }
    }
    private List<GridStruct> Grid;
 
    private static Godot.PackedScene Cell = (PackedScene)ResourceLoader.Load("res://assets/cell.res");
    private const int offeset = 70;
	private int Size = 4;
    public override void _Ready()
    {
        CreateGrid();
		GenerateNumber();
		GenerateNumber();
    }
    private void HighestCombo(int value){
        if (value*value > MaxComb){
            MaxComb = value*value;
        }
        if(MaxComb == 2048){
            GetTree().Paused = true;
			GetNode<Label>("GameOver/End_label").Text = "YOU WIN";
            GetNode<Control>("GameOver").Show();
        }
    }
    private void UpdateScore(int value){
        switch(value){
            case 2 :
                score+=10;
                break;
            case 4 :
                score+=35;
                break;
            case 8 :
                score+=50;
                break;
            case 16:
                score+=75;
                break;
            case 128:
                score+=150;
                 break;
            case 256:
                score+=200;
                 break;
            case 512:
                score+=350;
                 break;
            case 1024:
                score+=500;
                 break;
            case 2048:
                score+=1000;
                 break;
            default:
                score+=100;
                 break;
        }
        HighestCombo(value);
        GetNode<Label>("Score_label").Text = ("score : "+score);
    }
    private void GameOver()
    {   GetTree().Paused = true;
        GetNode<Label>("GameOver/End_label").Text = "GAME OVER";
        GetNode<Control>("GameOver").Show();
    }
	private void CreateGrid()
	{
        Grid = new List<GridStruct>();
        var i = 0;
        for(int x=0;x<Size;x++)
        {
		    for(int y=0;y<Size;y++)
	    	{
                
                Grid.Add(new GridStruct(x,y,(Godot.Control)Cell.Instance()));
                GetNode("Grid/Spawn").AddChild(Grid[i].res);
                i++;
			    //GD.Print(Grid[x+y].Call("GetValue") );
		    }
		}
	}
    private void GenerateNumber(){
       List<int> Option = new List<int>();
        var random = new Random();
        for(int i=0;i<Grid.Count;i++)
        {
            if ((int)Grid[i].res.Call("GetValue") == 0)
            {
                Option.Add(i);
            }
		}
        if (Option.Count > 0)
        {   var nrandom = random.Next(Option.Count);
            var CN = random.Next(0,100);
            //GD.Print(CN);
            Grid[Option[nrandom]].res.Call("setCell",CN >50 ? 2:4);
        }
        else
         GameOver();

    }
	
    public override void _UnhandledInput(InputEvent @event)
    {
    if (@event is InputEventKey eventKey)
        if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
            GetTree().Quit();   
    }

    public override void _Process(float delta)
    {
    if (Input.IsActionJustReleased("slide_up"))
        {	
            Slide_UP();
            GenerateNumber();
        }
    if (Input.IsActionJustReleased("slide_down"))
    {

        Slide_DOWN();
        GenerateNumber();
    }
    if (Input.IsActionJustReleased("slide_left"))
    {

        Slide_left();
        GenerateNumber();
    }
     if (Input.IsActionJustReleased("slide_right"))
    {

        Slide_right();
        GenerateNumber();
    }
    }

    private List<int> CheckCol(int x){
    List<int> Col = new List<int>();
        var random = new Random();
        for(int i=0;i<Grid.Count;i++)
        {
            if (Grid[i].x == x)
            {
                Col.Add(i);
            }
		} 
        return Col;
    }
    private List<int> CheckLine(int y){
    List<int> Line = new List<int>();
        var random = new Random();
        for(int i=0;i<Grid.Count;i++)
        {
            if (Grid[i].y == y)
            {
                Line.Add(i);
            }
		} 
        return Line;
    }
    private void Slide_UP()
    {
        for(int s = 0; s<Size;s++){
            List<int> Col = CheckCol(s);
		for(int i=Col.Count-1;i>=0;i--)
        { 
            if ((int)Grid[Col[i]].res.Call("GetValue") != 0)
            {   
                if (i-1 >=0){
                   
                    if ((int)Grid[Col[i - 1]].res.Call("GetValue") == 0)
                    {   Grid[Col[i - 1]].res.Call("setCell",Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i]].res.Call("setCell",0);

                       
                    }
                    else if ((int)Grid[Col[i - 1]].res.Call("GetValue") == (int)Grid[Col[i]].res.Call("GetValue") )
                    {
                        UpdateScore((int)Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i - 1]].res.Call("setCell",(int)Grid[Col[i]].res.Call("GetValue")+(int)Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i]].res.Call("setCell",0);
                        
                    }
                }
            }
        }
        }

    }
    private void Slide_left()
    {
        for(int s = 0; s<Size;s++){
            List<int> Col = CheckLine(s);
		for(int i=Col.Count-1;i>=0;i--)
        { 
            if ((int)Grid[Col[i]].res.Call("GetValue") != 0)
            {   
                if (i-1 >=0){
                   
                    if ((int)Grid[Col[i - 1]].res.Call("GetValue") == 0)
                    {  
                        Grid[Col[i - 1]].res.Call("setCell",Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i]].res.Call("setCell",0);
                    }
                    else if ((int)Grid[Col[i - 1]].res.Call("GetValue") == (int)Grid[Col[i]].res.Call("GetValue") )
                    {
                         UpdateScore((int)Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i - 1]].res.Call("setCell",(int)Grid[Col[i]].res.Call("GetValue")+(int)Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i]].res.Call("setCell",0);
                    }
                }
            }
        }
        }

    }
        private void Slide_DOWN()
    {
        for(int s = 0; s<Size;s++){
            List<int> Col = CheckCol(s);
		for(int i=0;i<Col.Count;i++)
        { 
            if ((int)Grid[Col[i]].res.Call("GetValue") != 0)
            {    
                if (i+1 <Col.Count){
                   
                    if ((int)Grid[Col[i + 1]].res.Call("GetValue") == 0)
                    {  
                        Grid[Col[i + 1]].res.Call("setCell",Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i]].res.Call("setCell",0);

                    }
                    else if ((int)Grid[Col[i + 1]].res.Call("GetValue") == (int)Grid[Col[i]].res.Call("GetValue") )
                    {
                         UpdateScore((int)Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i + 1]].res.Call("setCell",(int)Grid[Col[i]].res.Call("GetValue")+(int)Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i]].res.Call("setCell",0);
                    }
                }
            }
        }
        }

    }
    private void Slide_right(){
                for(int s = 0; s<Size;s++){
            List<int> Col = CheckLine(s);
		for(int i=0;i<Col.Count;i++)
        { 
            if ((int)Grid[Col[i]].res.Call("GetValue") != 0)
            {    
                if (i+1 <Col.Count){
                   
                    if ((int)Grid[Col[i + 1]].res.Call("GetValue") == 0)
                    {  
                        Grid[Col[i + 1]].res.Call("setCell",Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i]].res.Call("setCell",0);

                    }
                    else if ((int)Grid[Col[i + 1]].res.Call("GetValue") == (int)Grid[Col[i]].res.Call("GetValue") )
                    {
                         UpdateScore((int)Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i + 1]].res.Call("setCell",(int)Grid[Col[i]].res.Call("GetValue")+(int)Grid[Col[i]].res.Call("GetValue"));
                        Grid[Col[i]].res.Call("setCell",0);
                    }
                }
            }
        }
        }
    }

}
