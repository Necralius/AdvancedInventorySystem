 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MatrixUtility 
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //MatrixUtility - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Main Data Declaration -
    public string name;
    private int maxRow;
    private int maxCol;
    private int[,] item;
    #endregion

    #region - MatrixUtility Class Constructor -
    public MatrixUtility(int newMaxRow,int newMaxCol, string label)//This statment represent an MatrixUtility Constructor
    {
        maxRow = newMaxRow;
        maxCol = newMaxCol;
        name = label;
        PopulateMatrix();
    }
    #endregion

    #region - Matrix Population -
    public void PopulateMatrix()//This function represent the instatiation of an bidimensional array (Matrix instatiation)
    {
        item = new int[maxRow,maxCol];

        for (int row = 0 ; row < maxRow ; row++) for (int column = 0; column < maxCol; column++) item[row, column] = -1;
    }
    #endregion

    #region - List Show -
    public int[,] ShowFullList() => item;
    #endregion

    #region - Item Set on Matrix -
    public void SetItem(List<Vector2> list, int id)//This method set an item on a location using an list of Vector2 as coodinates
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
    #endregion

    #region - Item Clear on Matrix -
    public void ClearItemOnMatrix(int id)//This method clear an especific item on the matrix using his id
    {
        if (id >= 0) for (int row = 0; row < maxRow; row++) for (int column = 0; column < maxCol; column++) if (item[row, column] == id) item[row, column] = -1;
    }
    #endregion

    #region - Item Search -
    public List<Vector2> FindLocationById(int id)//This method returns an list of Vector2 that represents the complete item coordinate
    {
        List<Vector2> listResult = new List<Vector2> ();

        if (id >= 0) for (int row = 0; row < maxRow; row++) for (int column = 0; column < maxCol; column++) if (item[row, column] == id) listResult.Add(new Vector2(row, column));
        return listResult;
    }
    #endregion

    #region - Item Space Find -
    public List<Vector2> LookForFreeArea(int number)
    {
        //This method search an space for an item storage, based on the item size, and considering that the priority is horizontal space, if horizontal space has not finded, the algorithimum search for vertical space, and returns an list of Vector2 coordinates to the item ocupe the space
        
        List<Vector2> listResult = new List<Vector2>();

        if (number == 1)
        {
            listResult = LookForAreaHorizontalPriority(number, 1);
            return listResult;
        }
        if (number == 2)
        {
            listResult = LookForAreaHorizontalPriority(number, 1);

            if (listResult.Count > 1) return listResult;
            else
            {
                listResult = LookForAreaVerticalPriority(number, 1);
                if (listResult.Count > 1) return listResult;
            }
        }
        if (number == 3)
        {
            listResult = LookForAreaHorizontalPriority(number, 1);

            if (listResult.Count > 2) return listResult;
            else
            {
                listResult = LookForAreaVerticalPriority(number, 1);
                if (listResult.Count > 2) return listResult;
            }
        }
        if (number == 5)
        {

            listResult = LookForAreaHorizontalPriority(number, 1);

            if (listResult.Count > 4) return listResult;
            else
            {
                listResult = LookForAreaVerticalPriority(number, 1);
                if (listResult.Count > 4) return listResult;
            }
        }
        if (number == 4)
        {
            listResult = LookForAreaHorizontalPriority(2, 2);

            if (listResult.Count > 3) return listResult;
            else
            {
                listResult = LookForAreaHorizontalPriority(4, 1);
                if (listResult.Count > 3) return listResult;
                else
                {
                    listResult = LookForAreaVerticalPriority(4, 1);
                    if (listResult.Count > 3) return listResult;
                }
            }
        }
        if (number == 6)
        {
            listResult = LookForAreaHorizontalPriority(3, 2);

            if (listResult.Count > 5) return listResult;
            else
            {
                listResult = LookForAreaVerticalPriority(2, 3);
                if (listResult.Count > 5) return listResult;
            }
        }
        if (number == 8)
        {
            listResult = LookForAreaHorizontalPriority(4, 2);

            if (listResult.Count > 7) return listResult;
            else
            {
                listResult = LookForAreaVerticalPriority(4, 2);

                if (listResult.Count > 5) return listResult;
            }
        }
        return listResult;
    }
    #endregion

    #region - Horizontal Area Searcher -
    List<Vector2> LookForAreaHorizontalPriority(int numberHorizontal, int deep)//This method search an space on the matrix (bidimensional array), considering the horizontal priority
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
    #endregion

    #region - Vertical Area Searcher -
    private List<Vector2> LookForAreaVerticalPriority(int numberVertical,int deep)//This method search an space on the matrix (bidimensional array), considering the vertical priority
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
    #endregion

    #region - Space Validation -
    private bool VerticalValidation(int currentRow,int currentColumn,int deepVertical)//This method returns if the vertical space passed in the arguments is valid
    {
        bool result = true;

        if (deepVertical == 1) return result;
        
        if (deepVertical + currentRow > maxRow) return false;

        int limit = deepVertical + currentRow;
    
        for (int row = currentRow ; row < limit ; row++) result = result && (item[row, currentColumn] == -1);

        return result;
    }
    private bool HorizontalValidation(int currentRow, int currentColumn, int deepHorizontal)//This method returns if the horizontal space passed in the arguments is valid
    {
        bool result = true;

        if (deepHorizontal == 1) return result;

        if (deepHorizontal + currentColumn > maxCol) return false;

        int limit = deepHorizontal + currentColumn;

        for (int column = currentColumn ; column < limit ; column++) result = result && (item[currentRow, column] == -1);
        return result;
    }
    #endregion
}