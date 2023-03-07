 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MatrixUtility {

    public string name;
    private int maxRow;
    private int maxCol;
    private int[,] item;

    public MatrixUtility (int newMaxRow,int newMaxCol, string label) {

        maxRow = newMaxRow;
        maxCol = newMaxCol;
        name = label;
        PopulateMatrix ();
    }
    public void PopulateMatrix () 
    {
        item = new int[maxRow,maxCol];

        for (int row = 0 ; row < maxRow ; row++) for (int column = 0; column < maxCol; column++) item[row, column] = -1;
    }
    public int[,] ShowFullList () => item;

    public void SetItem(List<Vector2> list, int id) 
    {

        if (id >= 0) 
        {            
            foreach (var location in list)
            {
                int row = (int) location.x;
                int column = (int) location.y;
                item[row,column] = id;
            }
        }
        else Debug.LogWarning("Do not use id number above 0 Zero");  
    }
    public void ClearItemOnMatrix (int id) 
    {
        if (id >= 0) for (int row = 0; row < maxRow; row++) for (int column = 0; column < maxCol; column++) if (item[row, column] == id) item[row, column] = -1;
    }
    public List<Vector2> FindLocationById(int id) 
    {
        List<Vector2> listResult = new List<Vector2> ();

        if(id >= 0) for (int row = 0; row < maxRow; row++) for (int column = 0; column < maxCol; column++) if (item[row, column] == id) listResult.Add(new Vector2(row, column));
        return listResult;
    }
    public List<Vector2> LookForFreeArea(int number) 
    {
        List<Vector2> listResult = new List<Vector2>();

        if (number == 1) 
        {
            listResult = LookForAreaHorizontalPriority (number,1);
            return listResult;
        }
        if (number == 2) 
        {
            listResult = LookForAreaHorizontalPriority (number,1);

            if (listResult.Count > 1) return listResult;
            else 
            {
                listResult = LookForAreaVerticalPriority (number,1);
                if (listResult.Count > 1) return listResult;
            }
        }
        if (number == 3) 
        {
            listResult = LookForAreaHorizontalPriority (number,1);

            if (listResult.Count > 2) return listResult;
            else 
            {
                listResult = LookForAreaVerticalPriority (number,1);
                if (listResult.Count > 2) return listResult;
            }
        }
        if (number == 5) {

            listResult = LookForAreaHorizontalPriority (number,1);

            if (listResult.Count > 4) return listResult;
            else 
            {
                listResult = LookForAreaVerticalPriority (number,1);
                if (listResult.Count > 4) return listResult;
            }
        }
        if (number == 4) 
        {
            listResult = LookForAreaHorizontalPriority (2,2);

            if (listResult.Count > 3) return listResult;
            else 
            {
                listResult = LookForAreaHorizontalPriority (4,1);
                if (listResult.Count > 3) return listResult;
                else 
                {
                    listResult = LookForAreaVerticalPriority (4,1);
                    if (listResult.Count > 3) return listResult;
                }           
            }
        }
        if (number == 6) 
        {
            listResult = LookForAreaHorizontalPriority (3,2);

            if (listResult.Count > 5) return listResult;
            else 
            {
                listResult = LookForAreaVerticalPriority (2,3);
                if (listResult.Count > 5) return listResult;
            }
        }
        if (number == 8) 
        {
            listResult = LookForAreaHorizontalPriority (4,2);

            if (listResult.Count > 7) return listResult;
            else 
            {
                listResult = LookForAreaVerticalPriority (4,2);

                if (listResult.Count > 5) return listResult;
            }
        }
        return listResult;
    }

    List<Vector2> LookForAreaHorizontalPriority (int numberHorizontal, int deep) 
    {
        List<Vector2> listResult = new List<Vector2> ();        
        int colSelect = 0;
        
        for (int row = 0 ; row < maxRow ; row++) 
        {
            for (int column = 0 ; column < maxCol ; column++) 
            {
                if (item[row,column] == -1) 
                {
                    colSelect = column;

                    if (maxCol - column >= numberHorizontal) 
                    {
                        for (int i = colSelect ; i < maxCol ; i++) 
                        {                 
                            if (VerticalValidation (row,i,deep)) 
                            {
                                for (int add = 0 ; add < deep ; add++) 
                                {
                                    if (row + add < maxRow) 
                                    {
                                        if (item[row + add,i] == -1) 
                                        {
                                            listResult.Add (new Vector2(row + add,i));
                                            int w = row + add;
                                        }
                                        else listResult.Clear();                                      
                                    }
                                    if (listResult.Count == numberHorizontal * deep) return listResult;                              
                                    if (i == maxCol ) listResult.Clear();
                                }
                            }                                                                                               
                            else 
                            {
                                listResult.Clear ();

                                column = i;

                                if (maxCol - column <= numberHorizontal) column = maxCol;

                                break;
                            }
                        }
                    }
                }
                listResult.Clear ();
            }
        }
        return listResult;
    }
    private List<Vector2> LookForAreaVerticalPriority (int numberVertical,int deep) 
    {
        List<Vector2> listResult = new List<Vector2> ();
        
        int rowSelect = 0;

        for (int column = 0 ; column < maxCol ; column++) 
        {
            for (int row = 0 ; row < maxRow ; row++) 
            {
                if (item[row,column] == -1) 
                {
                    rowSelect = row;

                    if (maxRow - row >= numberVertical) 
                    {
                        for (int i = rowSelect ; i < maxRow ; i++) 
                        {
                            if (HorizontalValidation (i,column,deep)) 
                            {
                                for (int add = 0 ; add < deep ; add++) 
                                {
                                    if (column + add < maxCol) 
                                    {
                                        if (item[i,column + add] == -1) 
                                        {
                                            listResult.Add (new Vector2 (i,column + add));

                                            int w = column + add;
                                        }
                                        else listResult.Clear();
                                    }                                
                                }

                                if (listResult.Count == numberVertical * deep) return listResult;
                                if (i == maxRow )  listResult.Clear();
                            }
                            else 
                            {
                                listResult.Clear ();

                                row = i;

                                if (maxRow - row <= numberVertical) row = maxRow;
                                break;
                            }
                        }
                    }
                    else listResult.Clear();
                }
            }
        }
        return listResult;
    }
    private bool VerticalValidation (int currentRow,int currentColumn,int deepVertical) 
    {
        bool result = true;

        if (deepVertical == 1) return result;
        
        if (deepVertical + currentRow > maxRow) return false;

        int limit = deepVertical + currentRow;
    
        for (int row = currentRow ; row < limit ; row++) result = result && (item[row, currentColumn] == -1);

        return result;
    }
    private bool HorizontalValidation (int currentRow,int currentColumn,int deepHorizontal) 
    {
        bool result = true;

        if (deepHorizontal == 1) return result;

        if (deepHorizontal + currentColumn > maxCol) return false;

        int limit = deepHorizontal + currentColumn;

        for (int column = currentColumn ; column < limit ; column++) result = result && (item[currentRow, column] == -1);
        return result;
    }
}