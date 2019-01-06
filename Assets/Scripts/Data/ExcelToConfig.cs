using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

public static class ExcelToConfig
{
    [MenuItem("Excel/Define Class")]
    static void GenerateDefine()
    {
        //生成导表类
        string excelPath = Application.dataPath + "/Excel/";
        string scriptPath = Application.dataPath + "/Scripts/Data/";
        //获取所有的excel文件
        string[] excelFiles = Directory.GetFiles(excelPath);
        for (int i = 0; i < excelFiles.Length; i++)
        {
            string[] fileFolders = excelFiles[i].Split('/');
            string[] filenames = fileFolders[fileFolders.Length - 1].Split('.');
            if (filenames[filenames.Length - 1] != "xlsx")
                continue;
            string filename = fileFolders[fileFolders.Length - 1];
            using (FileStream fs = new FileStream(excelPath + filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (ExcelPackage excel = new ExcelPackage(fs))
                {
                    ExcelWorksheets workSheets = excel.Workbook.Worksheets;
                    for (int j = 1; j <= workSheets.Count; j++)
                    {
                        ExcelWorksheet workSheet = workSheets[j];
                        string scriptName = workSheet.Name;
                        StringBuilder code = new StringBuilder();
                        code.Append("/*\r\n\t该文件是由脚本自动生成，请勿手动修改\r\n*/\r\n\r\n");
                        code.Append("using System;\r\n\r\n[Serializable]\r\npublic class " + scriptName + "\r\n{\r\n");
                        int lastCol = workSheet.Dimension.End.Column;
                        for (int col = 1; col <= lastCol; col++)
                        {
                            string attrDesc = workSheet.Cells[1, col].Text;
                            string attrName = workSheet.Cells[2, col].Text;
                            string sttrType = workSheet.Cells[3, col].Text;
                            if(attrDesc.Length > 0 && attrName.Length > 0 && sttrType.Length > 0)
                                code.Append("\t//" + attrDesc + "\r\n\tpublic " + sttrType + " " + attrName + ";\r\n\r\n");
                        }
                        code.Append("}");
                        string codePath = scriptPath + scriptName + ".cs";
                        //保存
                        using (StreamWriter sw = new StreamWriter(codePath, false))
                        {
                            sw.Write(code.ToString());
                            sw.Flush();
                            sw.Close();
                        }
                    }
                }
            }
        }
        Debug.Log("导表类生成成功");
    }

    [MenuItem("Excel/Generate Config")]
    static void LoadData()
    {
        //生成导表数据
        string excelPath = Application.dataPath + "/Excel/";
        string configPath = Application.streamingAssetsPath + "/Config/";
        //获取所有的excel文件
        string[] excelFiles = Directory.GetFiles(excelPath);
        for (int i = 0; i < excelFiles.Length; i++)
        {
            string[] fileFolders = excelFiles[i].Split('/');
            string[] filenames = fileFolders[fileFolders.Length - 1].Split('.');
            if (filenames[filenames.Length - 1] != "xlsx")
                continue;
            string filename = fileFolders[fileFolders.Length - 1];
            using (FileStream fs = new FileStream(excelPath + filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (ExcelPackage excel = new ExcelPackage(fs))
                {
                    ExcelWorksheets workSheets = excel.Workbook.Worksheets;
                    for (int j = 1; j <= workSheets.Count; j++)
                    {
                        ExcelWorksheet workSheet = workSheets[j];
                        List<object> dataList = new List<object>();
                        string scriptName = workSheet.Name;
                        int lastCol = workSheet.Dimension.End.Column;
                        int lastRow = workSheet.Dimension.End.Row;
                        string[] tableFields = new string[lastRow];
                        Type t = Type.GetType(scriptName);
                        for (int col = 0; col < lastCol; col++)
                        {
                            tableFields[col] = workSheet.Cells[2, col + 1].Text;
                        }
                        for (int row = 4; row <= lastRow; row++)
                        {
                            object o = Activator.CreateInstance(t);
                            for (int col = 1; col <= lastCol; col++)
                            {
                                System.Reflection.FieldInfo info = o.GetType().GetField(tableFields[col - 1]);
                                string val = workSheet.Cells[row, col].Text;
                                if (info.FieldType == typeof(int))
                                    info.SetValue(o, int.Parse(val));
                                else if (info.FieldType == typeof(float))
                                    info.SetValue(o, float.Parse(val));
                                else
                                    info.SetValue(o, val);
                            }
                            dataList.Add(o);
                        }
                        MemoryStream memorySystem = new MemoryStream();
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(memorySystem, dataList);//把字典序列化成流
                        byte[] dictData = memorySystem.GetBuffer();
                        FileStream fileStream = new FileStream(configPath + scriptName + ".cfg", FileMode.Create);
                        fileStream.Write(dictData, 0, dictData.Length);
                        fileStream.Close();
                    }
                }
            }
        }
        Debug.Log("导表数据生成成功");
    }
}
    